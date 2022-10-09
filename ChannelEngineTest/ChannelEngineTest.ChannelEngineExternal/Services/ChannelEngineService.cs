﻿using System;
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
        
        private async Task<IReadOnlyList<Order>> GetOrdersInProgress()
        {
            var request = GetRequestString("orders", new Dictionary<string, string>()
            {
                { "statuses", OrderStatus.IN_PROGRESS.ToString() }
            });

            var response = await _client.GetAsync(request);
            var result = await GetResult<List<Order>>(response);

            return result;
        }

        private async Task<T> GetResult<T>(HttpResponseMessage response)
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

                return result.Content;
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