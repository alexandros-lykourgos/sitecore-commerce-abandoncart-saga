using AbandonCart.Contracts;
using AbandonCartApi.Models;
using MassTransit;
using MassTransit.AzureServiceBusTransport;
using Microsoft.ServiceBus;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace AbandonCartApi.Controllers
{
    [RoutePrefix("api/AbandonCarts")]
    public class AbandonCartsController:ApiController
    {
        [HttpGet]
        [Route("test")]
        public async Task<IHttpActionResult> Test()
        {
            await AddCartItem(new Models.InputModel { Timestamp = DateTime.UtcNow, UserId = "Arif" } );
            return Ok("Done");
        }


        [HttpPost]
        [Route("UpdateCarts")]
        public async Task<IHttpActionResult> UpdateCarts([FromBody] InputModel model)
        {
            if (!ModelState.IsValid)
            {
                return InternalServerError(new Exception("Model is not valid"));
            }

            await this.AddCartItem(model);
            return Ok();
        }

        [HttpPost]
        [Route("OrderCreated")]
        public async Task<IHttpActionResult> OrderCreated([FromBody] OrderCreateInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return InternalServerError(new Exception("Model is not valid"));
            }

            await OrderSubmitted(model);
            return Ok();
        }

        private async Task AddCartItem(InputModel model)
        {
            var bus = WebApiApplication.Bus;
            model.CartId = Guid.Parse(model.UserId.Replace("Default", "").Replace("StorefrontAU", ""));
            await bus.Publish<ICartItemAdded>(model);
        }

        private async Task OrderSubmitted(OrderCreateInputModel model)
        {
            var bus = WebApiApplication.Bus;
            model.CartId = Guid.Parse(model.UserId.Replace("Default", "").Replace("StorefrontAU", ""));
            await bus.Publish<IOrderSubmitted>(model);
        }

    }
}