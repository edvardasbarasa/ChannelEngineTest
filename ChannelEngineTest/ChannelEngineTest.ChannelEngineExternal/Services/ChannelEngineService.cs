using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ChannelEngineTest.ChannelEngineExternal.Models;
using ChannelEngineTest.Core.Externals;
using ChannelEngineTest.Core.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ChannelEngineTest.ChannelEngineExternal.Services
{
    public class ChannelEngineService : IChannelEngineService
    {
        private readonly HttpClient _client;
        private readonly ChannelEngineSettings _settings;

        public ChannelEngineService(HttpClient client, IOptions<ChannelEngineSettings> settings)
        {
            _client = client;
            _settings = settings.Value;
        }

        public async Task<IReadOnlyList<Product>> GetTopProductsAsync(int top)
        {
            var orders = await GetOrdersInProgress();

            var products = new Dictionary<string, Product>();

            foreach (var line in orders.SelectMany(i => i.Lines))
            {
                if (!products.ContainsKey(line.MerchantProductNo))
                {
                    products.Add(line.MerchantProductNo, new Product()
                    {
                        Gtin = line.Gtin,
                        MerchantProductNo = line.MerchantProductNo
                    });
                }

                products[line.MerchantProductNo].CountInProgress += line.Quantity;
            }

            return products.Select(i => i.Value).OrderByDescending(i => i.CountInProgress).Take(top).ToList();
        }

        public async Task SetProductStockAsync(string merchantProductNo, int stock)
        {
            var request = GetRequestString("products");

            var json = JsonConvert.SerializeObject(new SetProductStockRequest()
            {
                PropertiesToUpdate = new List<string>() { nameof(MerchantProductRequestModel.Stock) },
                MerchantProductRequestModels = new List<MerchantProductRequestModel>()
                {
                    new MerchantProductRequestModel()
                    {
                        Stock = stock,
                        MerchantProductNo = merchantProductNo
                    }
                }
            });

            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PatchAsync(request, data);
            var result = await GetResult<object>(response);
        }

        private async Task<IReadOnlyList<Order>> GetOrdersInProgress()
        {
            var page = 0;
            var lastPage = false;
            var orders = new List<Order>();

            // non stop pulling of the external api bad Idea because we can easily go to 429
            do
            {
                page++;

                var request = GetRequestString("orders", new Dictionary<string, string>()
                {
                    { "statuses", OrderStatus.IN_PROGRESS.ToString() },
                    { "page", page.ToString() }
                });

                var response = await _client.GetAsync(request);
                var result = await GetResult<List<Order>>(response);

                orders.AddRange(result.Content);

                lastPage = result.Count < result.ItemsPerPage;
            } 
            while (!lastPage);

            return orders;
        }

        private async Task<RequestRoot<T>> GetResult<T>(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                var result = JsonConvert.DeserializeObject<RequestRoot<T>>
                    (await response.Content.ReadAsStringAsync());

                if (!result.Success)
                {
                    throw new Exception(
                        $"Error while requesting ChannelEngine - request {response.RequestMessage.RequestUri} result {result.Message}");
                }

                return result;
            }

            if (response.StatusCode == HttpStatusCode.TooManyRequests)
            {
                throw new QuotaExceededException();
            }
            
            throw new Exception(
                $"Error while requesting ChannelEngine - request {response.RequestMessage.RequestUri} response status code {response.StatusCode}");
        }

        private string GetRequestString(string requestUri,
            Dictionary<string, string> parameters = default)
        {
            var builder = new StringBuilder($"/api/v2/{requestUri}?apikey={_settings.Key}");

            const string separator = "&";

            if (parameters != null)
            {
                foreach (var kvp in parameters.Where(kvp => kvp.Value != null))
                {
                    builder.AppendFormat("{0}{1}={2}", separator, WebUtility.UrlEncode(kvp.Key),
                        WebUtility.UrlEncode(kvp.Value.ToString()));
                }
            }

            return builder.ToString();
        }
    }
}