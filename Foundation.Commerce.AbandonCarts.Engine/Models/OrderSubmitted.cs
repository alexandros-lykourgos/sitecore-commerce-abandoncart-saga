using Foundation.Commerce.AbandonCarts.Engine.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Foundation.Commerce.AbandonCarts.Engine.Models
{
    public class OrderSubmitted : IOrderSubmitted
    {
        public Guid CartId
        {
            get;set;
        }

        public Guid OrderId
        {
            get;set;
        }

        public DateTime Timestamp
        {
            get;set;
        }

        public string UserId
        {
            get;set;
        }
    }
}
