using MassTransit;
using MassTransit.AzureServiceBusTransport;
using Microsoft.ServiceBus;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace AbandonCartApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        static IBusControl _bus;
        static BusHandle _busHandle;

        public static IBus Bus
        {
            get { return _bus; }
        }

        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            _bus = MassTransit.Bus.Factory.CreateUsingAzureServiceBus(x =>
            {
                var serviceUri = ServiceBusEnvironment.CreateServiceUri("sb", ConfigurationManager.AppSettings["AzureSbNamespace"], "");

                var host = x.Host(serviceUri, h =>
                {
                    h.TokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider(ConfigurationManager.AppSettings["AzureSbKeyName"], ConfigurationManager.AppSettings["AzureSbSharedAccessKey"], TimeSpan.FromDays(1), TokenScope.Namespace);
                });
            });


            _busHandle = MassTransit.Util.TaskUtil.Await<BusHandle>(() => _bus.StartAsync());

        }

        protected void Application_End()
        {
            if (_busHandle != null)
                _busHandle.Stop(TimeSpan.FromSeconds(30));
        }
    }
}
