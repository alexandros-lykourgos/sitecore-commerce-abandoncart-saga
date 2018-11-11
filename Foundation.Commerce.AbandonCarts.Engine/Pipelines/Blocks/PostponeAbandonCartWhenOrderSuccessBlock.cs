using Foundation.Commerce.AbandonCarts.Engine.Models;
using Newtonsoft.Json;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Commerce.Plugin.Fulfillment;
using Sitecore.Commerce.Plugin.Orders;
using Sitecore.Framework.Pipelines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Commerce.AbandonCarts.Engine.Pipelines.Blocks
{
    public class PostponeAbandonCartWhenOrderSuccessBlock : PipelineBlock<Order, Order, CommercePipelineExecutionContext>
    {
        public async override Task<Order> Run(Order arg, CommercePipelineExecutionContext context)
        {
            var policy = context.GetPolicy<Policies.AbandonCartsPolicy>();

            var data = new OrderSubmitted
            {
                UserId = arg.Name,
                Timestamp = DateTime.UtcNow,
                OrderId = Guid.Parse( arg.Id)

            };

            using (var client = new WebClient())
            {
                client.Headers["content-type"] = "application/json";
                var stringData = JsonConvert.SerializeObject(data);
                var bytes = Encoding.UTF8.GetBytes(stringData);
                var response = await client.UploadDataTaskAsync(policy.OrderCreatedUrl, bytes);
                var responseText = Encoding.UTF8.GetString(response);

            }

            return await Task.FromResult(arg);
        }
    }
}
