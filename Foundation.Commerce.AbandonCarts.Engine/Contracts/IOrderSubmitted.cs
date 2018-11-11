using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Foundation.Commerce.AbandonCarts.Engine.Contracts
{
    public interface IOrderSubmitted
    {
        Guid OrderId { get; set; }
        DateTime Timestamp { get; set; }
        Guid CartId { get; set; }
        string UserId { get; set; }
    }
}
