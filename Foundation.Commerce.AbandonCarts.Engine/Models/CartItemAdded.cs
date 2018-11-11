using Foundation.Commerce.AbandonCarts.Engine.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Foundation.Commerce.AbandonCarts.Engine.Models
{
    public class CartItemAdded : ICartItemAdded
    {
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
