﻿using AbandonCart.CartTracking;
using MassTransit.EntityFrameworkIntegration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbandonCartStateMachine
{
    public class ShoppingCartMap:SagaClassMapping<ShoppingCart>
    {
        public ShoppingCartMap()
        {
            Property(x => x.CurrentState).HasMaxLength(64);
            Property(x => x.Created);
            Property(x => x.Updated);

            Property(x => x.UserId)
                .HasMaxLength(256);

            Property(x => x.ExpirationId);
            Property(x => x.OrderId);
        }
    }
}
