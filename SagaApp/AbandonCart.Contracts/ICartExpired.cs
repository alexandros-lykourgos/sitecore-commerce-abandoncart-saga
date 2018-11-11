using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbandonCart.Contracts
{
    public interface ICartExpired
    {
        Guid CartId { get; }
    }
}
