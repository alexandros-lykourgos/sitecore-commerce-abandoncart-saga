using Automatonymous;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Foundation.Commerce.AbandonCarts.Engine.Tracking
{
    public class ShoppingCart:SagaStateMachineInstance
    {
        public Guid CorrelationId
        {
            get; set;
        }

        public string CurrentState { get; set; }
        public string UserId { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        /// <summary>
        /// The expiration tag for the shopping cart, which is scheduled whenever
        /// the cart is updated
        /// </summary>
        public Guid? ExpirationId { get; set; }
        public Guid? OrderId { get; set; }
    }
}
