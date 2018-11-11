using System;


namespace AbandonCart.Contracts
{
    public interface ICartItemAdded
    {
        DateTime Timestamp { get; set; }

        string UserId { get; set; }

        Guid CartId { get; set; }
    }
}

