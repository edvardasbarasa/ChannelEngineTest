using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChannelEngineTest.Core.Commands.Orders.SetProductStockCommand;
using ChannelEngineTest.Core.Models;
using ChannelEngineTest.Core.Queries.Orders.GetTopProductsQuery;
using FluentResults;
using MediatR;

namespace ChannelEngineTest.Console
{
    public class ConsoleApplication
    {
        private readonly IMediator _mediator;

        public ConsoleApplication(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task RunAsync()
        {
            System.Console.WriteLine("Hello Channel Engine!");
            System.Console.WriteLine("Requesting top 5 products :");

            var products = await GetProducts();

            if (products == null)
            {
                return;
            }
            
            System.Console.WriteLine("Updating first product stock :");

            var productForUpdate = products.First();

            await SetProductStock(productForUpdate.MerchantProductNo, 25);

            System.Console.ReadLine();
        }

        private async Task SetProductStock(string merchantProductNo, int stock)
        {
            var setProductStockResult =
                await _mediator.Send(new SetProductStockCommand()
                {
                    Stock = stock,
                    MerchantProductNo = merchantProductNo
                });
            
            if (setProductStockResult.IsFailed)
            {
                DisplayError(setProductStockResult);
                return;
            }
            
            System.Console.WriteLine($"Product {merchantProductNo} stock updated.");
        }
        
        private async Task<IReadOnlyList<Product>> GetProducts()
        {
            var getTopProductsResult =
                await _mediator.Send(new GetTopProductsQuery()
                {
                    Top = 5
                });

            if (getTopProductsResult.IsFailed)
            {
                DisplayError(getTopProductsResult);
                return null;
            }
            
            System.Console.WriteLine();

            foreach (var product in getTopProductsResult.Value.TopProducts)
            {
                System.Console.WriteLine($" Product : {product.MerchantProductNo}");
            }
            
            System.Console.WriteLine();

            return getTopProductsResult.Value.TopProducts;
        }

        private void DisplayError<T>(Result<T> result)
        {
            if (result.IsFailed)
            {
                System.Console.WriteLine($"Error while processing reques. {result.Errors.First().Message}. Terminated");
            }
        }
        
        private void DisplayError(Result result)
        {
            if (result.IsFailed)
            {
                System.Console.WriteLine($"Error while processing reques. {result.Errors.First().Message}. Terminated");
            }
        }
    }
}