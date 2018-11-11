using AbandonCart.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbandonCartApi.Models
{
    public class OrderCreateInputModel : IOrderSubmitted
    {
        public Guid OrderId { get; set; }

        public DateTime Timestamp { get; set; }

        public Guid CartId { get; set; }

        public string UserId { get; set; }
    }
}