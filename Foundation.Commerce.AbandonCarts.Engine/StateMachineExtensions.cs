using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Foundation.Commerce.AbandonCarts.Engine
{
    public static class StateMachineExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TInstance"></typeparam>
        /// <param name="configurator"></param>
        /// <param name="stateMachine"></param>
        /// <param name="repository"></param>
        /// <param name="configure"></param>
        public static void StateMachineSaga<TInstance>(this MassTransit.IReceiveEndpointConfigurator configurator, Automatonymous.SagaStateMachine<TInstance> stateMachine, MassTransit.Saga.ISagaRepository<TInstance> repository, Action<MassTransit.Saga.SubscriptionConfigurators.ISagaConfigurator<TInstance>> configure = null) where TInstance : class, Automatonymous.SagaStateMachineInstance
        {
            Automatonymous.SubscriptionConfigurators.StateMachineSagaConfigurator<TInstance> sagaConfigurator = new Automatonymous.SubscriptionConfigurators.StateMachineSagaConfigurator<TInstance>(stateMachine, repository, configurator);
            if (configure != null)
                configure(sagaConfigurator);
            configurator.AddEndpointSpecification(sagaConfigurator);
        }
    }
}
