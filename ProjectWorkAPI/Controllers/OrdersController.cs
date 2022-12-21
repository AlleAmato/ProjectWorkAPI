using ProjectWorkAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using ProjectWorkAPI.Dto;

namespace ProjectWorkAPI.Controllers
{
    [RoutePrefix("api/v1/orders")]
    public class OrdersController : ApiController
    {
        [HttpGet]
        [Route("")]
        [Authorize(Roles = "Azienda,Admin")]
        public async Task<IHttpActionResult> GetAllOrders()
        {
            var mapper = new Mapper(new MapperConfiguration(c => c.AddProfile<DtoMappingProfile>()));

            using (var context = new DatabaseContext())
            {
                return Ok(mapper.Map<List<OrderDto>>(await context.Orders
                    .Include(i => i.OrderItems)
                    .ToListAsync()));
            }
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(Roles = "Azienda,Admin")]
        public async Task<IHttpActionResult> GetOneOrder(string id)
        {
            var mapper = new Mapper(new MapperConfiguration(c => c.AddProfile<DtoMappingProfile>()));

            using (var context = new DatabaseContext())
            {
                var order = await context.Orders
                    .Include(i => i.OrderItems)
                    .FirstOrDefaultAsync(q => q.Id == id);

                if (order == null) return NotFound();
                return Ok(mapper.Map<OrderDto>(order));
            }
        }

        [HttpPost]
        [Route("")]
        [Authorize(Roles ="Rivenditore,Admin")]
        public async Task<IHttpActionResult> AddNewOrder([FromBody] OrderDto newOrderDto)
        {
            var mapper = new Mapper(new MapperConfiguration(c => c.AddProfile<DtoMappingProfile>()));

            using (var context = new DatabaseContext())
            {
                try
                {
                    var newOrder = mapper.Map<Order>(newOrderDto);

                    var identity = (ClaimsIdentity)User.Identity;

                    newOrder.Id = await GetNewOrderId(context);
                    newOrder.ResellerId = identity.Claims.FirstOrDefault(q => q.Type == ClaimTypes.NameIdentifier).Value;
                    newOrder.SendDate = DateTime.Now;
                    newOrder.OrderState = await context.OrderStates.FirstOrDefaultAsync(q => q.Id == 20);

                    context.Orders.Add(newOrder);
                    await context.SaveChangesAsync();
                    return Created(new Uri($"{Request.RequestUri}/{newOrder.Id}"), mapper.Map<OrderDto>(newOrder));
                }
                catch(Exception)
                {
                    return InternalServerError();
                }

            }
        }

        private async Task<string> GetNewOrderId(DatabaseContext context)
        {
            Random rnd = new Random();
            MD5 md5 = MD5.Create();

            string hash = "";
            bool found = false;

            do
            {
                hash = BitConverter.ToString(md5.ComputeHash(BitConverter.GetBytes(rnd.Next(100000)))).Replace("-", "");
                found = (await context.Orders.FirstOrDefaultAsync(q => q.Id == hash)) == null;

            } while (!found);
            return hash;
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> DeleteOrder(string id)
        {
            using (var context = new DatabaseContext())
            {
                var order = await context.Orders
                    .Include(i=>i.OrderItems)
                    .Include(i=>i.OrderState)
                    .FirstOrDefaultAsync(q => q.Id == id);

                if (order == null) return NotFound();
                if (order.OrderState == null || order.OrderState.Id > 20) return BadRequest();

                try
                {
                    context.OrderItems.RemoveRange(order.OrderItems);
                    context.Orders.Remove(order);
                    await context.SaveChangesAsync();
                    return Ok() ;
                }
                catch (Exception)
                {
                    return InternalServerError();
                }
            }
        }

        [HttpGet]
        [Route("{id}/state")]
        [Authorize(Roles = "Rivenditore,Admin")]
        public async Task<IHttpActionResult> GetOrderState(string id)
        {
            var mapper = new Mapper(new MapperConfiguration(c => c.AddProfile<DtoMappingProfile>()));

            using (var context = new DatabaseContext())
            {
                var state = await context.OrderStates
                    .FirstOrDefaultAsync(q => q.Orders.Any(o => o.Id == id));

                if (state == null) return NotFound();

                return Ok(mapper.Map<OrderStateDto>(state));
            }
        }

        [HttpPut]
        [Route("{id}/state")]
        [Authorize(Roles = "Azienda,Admin")]
        public async Task<IHttpActionResult> SetOrderState(string id, [FromBody]OrderStateDto osDto)
        {
            var mapper = new Mapper(new MapperConfiguration(c => c.AddProfile<DtoMappingProfile>()));

            using (var context = new DatabaseContext())
            {
                var os = mapper.Map<OrderState>(osDto);

                var order = await context.Orders.Include(i=>i.OrderState)
                    .FirstOrDefaultAsync(q => q.Id == id);

                var state = await context.OrderStates.FirstOrDefaultAsync(q => q.Id == os.Id);

                if (order == null || state == null) return NotFound();

                if (order.OrderState != null && order.OrderState.Id > state.Id) return BadRequest();

                try
                {
                    order.OrderState = state;
                    if (state.Id == 30) order.StartProductionDate = DateTime.Now;
                    if (state.Id == 40) order.StopProductionDate = DateTime.Now;
                    await context.SaveChangesAsync();
                    return Ok();
                }
                catch(Exception)
                {
                    return InternalServerError();
                }
            }
        }
    }
}