using AbandonCart.Contracts;
using Automatonymous;
using System;

namespace AbandonCart.CartTracking
{
    public class ShoppingCartStateMachine: MassTransitStateMachine<ShoppingCart>
    {
        public ShoppingCartStateMachine()
        {
            InstanceState(x => x.CurrentState);
            Event(() => ItemAdded, x => x.CorrelateBy(cart => cart.UserId, context => context.Message.UserId).SelectId(context => context.Message.CartId));
            Event(() => OrderSubmitted, x => x.CorrelateById(context => context.Message.CartId));
            Schedule(() => CartExpired, x => x.ExpirationId, x =>
              {
                  x.Delay = TimeSpan.FromMinutes(15);
                  x.Received = e => e.CorrelateById(context => context.Message.CartId);
              });

            Initially(
                When(ItemAdded)
                .Then(context =>
                {
                    context.Instance.Created = context.Data.Timestamp;
                    context.Instance.Updated = context.Data.Timestamp;
                    context.Instance.UserId = context.Data.UserId;
                })
                .ThenAsync(context => Console.Out.WriteLineAsync($"Item Added: {context.Data.UserId} to {context.Instance.CorrelationId}"))
                .Schedule(CartExpired, context => new CartExpiredEvent(context.Instance))
                .TransitionTo(Active)
                );

            During(Active, When(OrderSubmitted).Then(context =>
            {
                if (context.Data.Timestamp > context.Instance.Updated)
                    context.Instance.Updated = context.Data.Timestamp;
                context.Instance.OrderId = context.Data.OrderId;
            })
            .ThenAsync(context => Console.Out.WriteLineAsync($"Cart Submitted: {context.Data.UserId} to {context.Instance.CorrelationId}"))
            .Unschedule(CartExpired)
            .TransitionTo(Ordered),
            When(ItemAdded)
            .Then(context =>
            {

                if (context.Data.Timestamp > context.Instance.Updated)
                    context.Instance.Updated = context.Data.Timestamp;
            })
            .ThenAsync(context => Console.Out.WriteLineAsync($"Item Added: {context.Data.UserId} to {context.Instance.CorrelationId}"))
            .Schedule(CartExpired, context => new CartExpiredEvent(context.Instance)),
            When(CartExpired.Received)
            .ThenAsync(context => Console.Out.WriteLineAsync($"Item Expired: {context.Instance.CorrelationId}"))
            .Publish(context => new CartRemovedEvent(context.Instance))
            .Finalize()
            );

            SetCompletedWhenFinalized();
        }
        public State Active { get; private set; }
        public State Ordered { get; private set; }

        public Schedule<ShoppingCart, ICartExpired> CartExpired { get; private set; }

        public Event<ICartItemAdded> ItemAdded { get; private set; }
        public Event<IOrderSubmitted> OrderSubmitted { get; set; }

        class CartExpiredEvent : ICartExpired
        {
            readonly ShoppingCart _instance;
            public CartExpiredEvent(ShoppingCart instance)
            {
                _instance = instance;
            }

            public Guid CartId => _instance.CorrelationId;
        }

        class CartRemovedEvent: ICartRemoved
        {
            readonly ShoppingCart _instance;

            public CartRemovedEvent(ShoppingCart instance)
            {
                _instance = instance;
            }

            public Guid CartId => _instance.CorrelationId;

            public string UserId => _instance.UserId;
        }
    }
}
