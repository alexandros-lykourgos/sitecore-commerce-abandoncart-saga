using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbandonCart.Contracts
{
    public interface IOrderSubmitted
    {
        Guid OrderId { get; set; }

        DateTime Timestamp { get; set; }

        Guid CartId { get; set; }

        string UserId { get; set; }
    }
}
