using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Framework.Conditions;
using Quartz;
using MassTransit;
using MassTransit.Saga;
using Quartz.Impl;
using MassTransit.EntityFrameworkIntegration;
using MassTransit.EntityFrameworkIntegration.Saga;
using MassTransit.AzureServiceBusTransport;
using Microsoft.ServiceBus;
using MassTransit.QuartzIntegration;
using Foundation.Commerce.AbandonCarts.Engine.Contracts;
using Foundation.Commerce.AbandonCarts.Engine.Models;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace Foundation.Commerce.AbandonCarts.Engine.Pipelines.Blocks
{
    [PipelineDisplayName("AbandonCarts.UpdateAbandonCartBlock")]
    public class UpdateAbandonCartBlock : PipelineBlock<Cart, Cart, CommercePipelineExecutionContext>
    {
        public UpdateAbandonCartBlock()
        {

        }
        public override async Task<Cart> Run(Cart arg, CommercePipelineExecutionContext context)
        {
            var policy = context.GetPolicy<Policies.AbandonCartsPolicy>();

            var data = new CartItemAdded
            {
                UserId = arg.Id,
                Timestamp = DateTime.UtcNow,

            };

            using (var client = new WebClient())
            {
                client.Headers["content-type"] = "application/json";
                var stringData = JsonConvert.SerializeObject(data);
                var bytes = Encoding.UTF8.GetBytes(stringData);
                var response = await client.UploadDataTaskAsync(policy.AddItemUrl, bytes);
                var responseText = Encoding.UTF8.GetString(response);

            }

            return await Task.FromResult(arg);
        
        }
    }
}
