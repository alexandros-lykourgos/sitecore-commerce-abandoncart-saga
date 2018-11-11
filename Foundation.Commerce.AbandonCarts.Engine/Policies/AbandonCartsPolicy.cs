// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SamplePolicy.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2017
// </copyright>
// <summary>
//   SamplePolicy policy.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Foundation.Commerce.AbandonCarts.Engine.Policies
{
    using MassTransit;
    using MassTransit.AzureServiceBusTransport;
    using MassTransit.EntityFrameworkIntegration;
    using MassTransit.EntityFrameworkIntegration.Saga;
    using MassTransit.QuartzIntegration;
    using MassTransit.Saga;
    using Microsoft.ServiceBus;
    using Quartz;
    using Quartz.Impl;
    using Sitecore.Commerce.Core;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Tracking;

    public class AbandonCartsPolicy : Policy
    {
        IBusControl _busControl;
        private readonly Dictionary<string, IBusControl> _busCache = new Dictionary<string, IBusControl>();
        
        public string ConnectionString { get; set; } = "Data Source=(local);Initial Catalog=AbadonCart.ShoppingCart;Integrated Security=True";

        public string AzureSbNamespace { get; set; } = "testarif";

        public string AzureSbKeyName { get; set; } = "RootManageSharedAccessKey";

        public string AzureSbSharedAccessKey { get; set; } = "M4X57WeecEfb+Ut5lgrMPNcUjTD711Yn3JfP+5kel/0=";

        public string SchedulerQueueName { get; set; } = "abandon_cart_schedular";

        public string StateQueueName { get; set; } = "shopping_cart_state";

        public string AddItemUrl { get; set; } = "http://localhost:50546/api/AbandonCarts/UpdateCarts";

        public string OrderCreatedUrl { get; set; } = "http://localhost:50546/api/AbandonCarts/OrderCreated";

        public AbandonCartsPolicy()
        {

        }

        public async Task<IBusControl> GetBus(CommercePipelineExecutionContext context)
        {

            if (_busCache.ContainsKey(AzureSbSharedAccessKey))
            {
                _busControl = _busCache[AzureSbSharedAccessKey];
            }
            else
            {
                _busControl = Bus.Factory.CreateUsingAzureServiceBus(x =>
                {
                    var serviceUri = ServiceBusEnvironment.CreateServiceUri("sb", this.AzureSbNamespace, "", true);


                    var host = x.Host(serviceUri, h =>
                    {
                        h.TokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider(this.AzureSbKeyName, this.AzureSbSharedAccessKey, TimeSpan.FromDays(1), TokenScope.Namespace);
                    });
                });

                _busCache.Add(AzureSbSharedAccessKey, _busControl);
                var busHandle = MassTransit.Util.TaskUtil.Await<BusHandle>(() => _busControl.StartAsync());
            }

            return await Task.FromResult(_busControl);
        }

    }
}
