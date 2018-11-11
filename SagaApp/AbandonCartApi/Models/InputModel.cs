using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbandonCartApi.Models
{
    public class InputModel
    {
        public string UserId { get; set; }
        public DateTime Timestamp { get; set; }
        public Guid CartId { get; set; }
    }
}