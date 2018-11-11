using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Foundation.Commerce.AbandonCarts.Engine.Contracts
{
    public interface ICartItemAdded
    {
        string UserId { get; set; }
        DateTime Timestamp { get; set; }
    }
}
