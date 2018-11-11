using AbandonCart.CartTracking;
using MassTransit;
using MassTransit.AzureServiceBusTransport;
using MassTransit.EntityFrameworkIntegration;
using MassTransit.EntityFrameworkIntegration.Saga;
using MassTransit.QuartzIntegration;
using MassTransit.Saga;
using Microsoft.ServiceBus;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;
using Topshelf.Logging;

namespace AbandonCartStateMachine
{
    public class TrackingService : ServiceControl
    {
        readonly LogWriter _log = HostLogger.Get<TrackingService>();
        readonly IScheduler _scheduler;

        IBusControl _busControl;
        BusHandle _busHandle;
        ShoppingCartStateMachine _machine;
        Lazy<ISagaRepository<ShoppingCart>> _repository;

        public TrackingService()
        {
            _scheduler = CreateSchedular();

        }

        private IScheduler CreateSchedular()
        {
            var factory = new StdSchedulerFactory();
            var scheduler = MassTransit.Util.TaskUtil.Await<IScheduler>(() => factory.GetScheduler());
            return scheduler;
        }

        public bool Start(HostControl hostControl)
        {
            _log.Info("Creating bus...");
            _machine = new ShoppingCartStateMachine();
            SagaDbContextFactory sagaDbContextFactory = () => new SagaDbContext<ShoppingCart, ShoppingCartMap>(SagaDbContextFactoryProvider.ConnectionString);
            _repository = new Lazy<ISagaRepository<ShoppingCart>>(() => new EntityFrameworkSagaRepository<ShoppingCart>(sagaDbContextFactory));

            _busControl = Bus.Factory.CreateUsingAzureServiceBus(x =>
            {
                var serviceUri = ServiceBusEnvironment.CreateServiceUri("sb", ConfigurationManager.AppSettings["AzureSbNamespace"], "");

                var host = x.Host(serviceUri, h =>
                {
                    h.OperationTimeout = TimeSpan.FromMinutes(5);
                    h.TokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider(ConfigurationManager.AppSettings["AzureSbKeyName"], ConfigurationManager.AppSettings["AzureSbSharedAccessKey"], TimeSpan.FromDays(1), TokenScope.Namespace);
                });
                
                x.ReceiveEndpoint(host, "shopping_cart_state", e =>
                {
                    e.PrefetchCount = 8;
                    e.StateMachineSaga(_machine, _repository.Value);
                });

                x.ReceiveEndpoint(host, ConfigurationManager.AppSettings["SchedulerQueueName"], e =>
                {
                    e.PrefetchCount = 1;
                    x.UseMessageScheduler(e.InputAddress);

                    e.Consumer(() => new ScheduleMessageConsumer(_scheduler));
                    e.Consumer(() => new CancelScheduledMessageConsumer(_scheduler));
                });
            });

            _log.Info("Starting bus...");

            try
            {
                _busHandle = MassTransit.Util.TaskUtil.Await<BusHandle>(() => _busControl.StartAsync());

                _scheduler.JobFactory = new MassTransitJobFactory(_busControl);

                _scheduler.Start();
            }
            catch (Exception)
            {
                _scheduler.Shutdown();
                throw;
            }

            return true;

        }

        public bool Stop(HostControl hostControl)
        {
            _log.Info("Stopping bus...");

            _scheduler.Standby();

            if (_busHandle != null)
                _busHandle.Stop();

            _scheduler.Shutdown();

            return true;
        }
    }
}
