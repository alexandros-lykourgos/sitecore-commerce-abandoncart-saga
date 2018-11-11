using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Foundation.Commerce.AbandonCarts.Engine.Contracts
{
    public interface ICartExpired
    {
        Guid CartId { get; }
    }
}
