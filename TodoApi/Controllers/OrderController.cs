using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly TodoContext _context;

        public OrderController(TodoContext context)
        {
            _context = context;
        }

        // GET: api/Order
        [HttpGet("list")]
        public async Task<ActionResult<APIResponse<IEnumerable<Room>>>> GetOrder()
        {
            List<Order> orders = await _context.Order.ToListAsync();

            return Ok(new APIResponse<IEnumerable<Order>> { Data = orders });
        }

        // GET: api/Order
        [HttpGet("userOrderList")]
        public async Task<ActionResult<APIResponse<IEnumerable<Room>>>> GetUserOrder(long userId, string? status)
        {
            List<Order>? orders = null;
            if (status == null)
            {
                orders = await _context.Order.Where(o => o.UserId == userId).ToListAsync();

            }
            else
            {
                orders = await _context.Order.Where(o => o.UserId == userId && o.Status == status).ToListAsync();

            }

            return Ok(new APIResponse<IEnumerable<Order>> { Data = orders });
        }

        // GET: api/Order/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(long id)
        {
            var order = await _context.Order.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        // PUT: api/Order/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("update")]
        public async Task<ActionResult<APIResponse<Order>>> PutOrder([FromForm] Order order)
        {
            if (0 == order.Id)
            {
                return Ok(new APIResponse<Order> { Code = 403, Msg = "order id is null" });
            }

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(order.Id))
                {
                    return Ok(new APIResponse<Order> { Code = 404, Msg = "Not Found" });
                }
                else
                {
                    throw;
                }
            }

            return Ok(new APIResponse<Order>() { });
        }

        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("cancelOrder")]
        public async Task<ActionResult<APIResponse<Order>>> CancelOrder(long id)
        {
            if (0 == id)
            {
                return Ok(new APIResponse<Order> { Code = 403, Msg = "order id is null" });
            }

            Order? order = await _context.Order.FindAsync(id);
            if (null == order)
            {
                return Ok(new APIResponse<Order> { Code = 403, Msg = "order is null" });

            }

            order.Status = "7";//7 cancel
            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(order.Id))
                {
                    return Ok(new APIResponse<Order> { Code = 404, Msg = "Not Found" });
                }
                else
                {
                    throw;
                }
            }

            return Ok(new APIResponse<Order> { Msg = "order is canceled" });
        }

        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("payUserOrder")]
        public async Task<ActionResult<APIResponse<Order>>> PayOrder(long id)
        {
            if (0 == id)
            {
                return Ok(new APIResponse<Order> { Code = 403, Msg = "order id is null" });
            }

            Order? order = await _context.Order.FindAsync(id);
            if (null == order)
            {
                return Ok(new APIResponse<Order> { Code = 403, Msg = "order is null" });

            }
            order.OrderTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            order.PayTime = Guid.NewGuid().ToString();
            order.Status = "2";//2 支付
            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(order.Id))
                {
                    return Ok(new APIResponse<Order> { Code = 404, Msg = "Not Found" });
                }
                else
                {
                    throw;
                }
            }

            return Ok(new APIResponse<Order> { Msg = "order is canceled" });
        }

        [HttpPost("cancelUserOrder")]
        public async Task<ActionResult<APIResponse<Order>>> CancelUserOrder(long id)
        {
            if (0 == id)
            {
                return Ok(new APIResponse<Order> { Code = 403, Msg = "order id is null" });
            }

            Order? order = await _context.Order.FindAsync(id);
            if (null == order)
            {
                return Ok(new APIResponse<Order> { Code = 404, Msg = "order is null" });

            }

            order.Status = "7";//7 cancel
            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(order.Id))
                {
                    return Ok(new APIResponse<Order> { Code = 404, Msg = "Not Found" });
                }
                else
                {
                    throw;
                }
            }

            return Ok(new APIResponse<Order> { Msg = "order is canceled" });
        }

        // POST: api/Order
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("create")]
        public async Task<ActionResult<APIResponse<Order>>> PostOrder([FromForm] Order order)
        {
            order.Status = "1";
            order.OrderTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            order.OrderNumber = Guid.NewGuid().ToString();
            User? user = await _context.User.FindAsync(order.UserId);
            Console.WriteLine(user.Username);
            order.Username = user == null ? "" : user.Username;
            _context.Order.Add(order);
            await _context.SaveChangesAsync();

            var orderw = _context.Order.Find(order.Id);
            // return CreatedAtAction("GetOrder", new { id = order.Id }, order);
            return Ok(new APIResponse<Order>() { Data = orderw });
        }

        // DELETE: api/Order/5
        [HttpPost("delete")]
        public async Task<ActionResult<APIResponse<Order>>> DeleteOrder(string ids)
        {
            string[] strings = ids.Split(",");
            for (int i = 0; i < strings.Length; i++)
            {
                var room = await _context.Order.FindAsync(long.Parse(strings[i]));
                if (room == null)
                {
                    return Ok(new APIResponse<Order> { Code = 404, Msg = "Not Found" });

                }

                _context.Order.Remove(room);
            }

            await _context.SaveChangesAsync();

            return Ok(new APIResponse<Order> { });
        }

        private bool OrderExists(long id)
        {
            return _context.Order.Any(e => e.Id == id);
        }
    }
}
