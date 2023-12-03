using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Controllers
//预订
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly TodoContext _context;

        public ReservationController(TodoContext context)
        {
            _context = context;
        }

        // GET: api/Reservation
        [HttpGet("list")]
        public async Task<ActionResult<APIResponse<IEnumerable<Reservation>>>> GetReservation()
        {
            List<Reservation> reservations = await _context.Reservation.ToListAsync();

            return Ok(new APIResponse<IEnumerable<Reservation>> { Data = reservations });
        }

        // GET: api/Reservation
        [HttpGet("userOrderList")]
        public async Task<ActionResult<APIResponse<IEnumerable<Reservation>>>> UserOrderList([FromQuery] string userId,
        [FromQuery] string status)
        {

            List<Reservation> reservations = await _context.Reservation.Where(r => r.UserId == userId && r.Status == status).ToListAsync();

            return Ok(new APIResponse<IEnumerable<Reservation>> { Data = reservations });
        }

        // GET: api/Reservation/5
        [HttpGet("{id}")]
        public async Task<ActionResult<APIResponse<Reservation>>> GetReservation(long id)
        {
            var reservation = await _context.Reservation.FindAsync(id);

            if (reservation == null)
            {
                return Ok(new APIResponse<Room> { Code = 404, Msg = "Not Found" });
            }

            return Ok(new APIResponse<Reservation>() { });

        }

        // PUT: api/Reservation/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReservation(long id, Reservation reservation)
        {
            if (id != reservation.Id)
            {
                return BadRequest();
            }

            _context.Entry(reservation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Reservation
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("create")]
        public async Task<ActionResult<Reservation>> PostReservation(Reservation reservation)
        {
            _context.Reservation.Add(reservation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReservation", new { id = reservation.Id }, reservation);
        }

        // DELETE: api/Reservation/5
        [HttpPost("delete")]
        public async Task<ActionResult<APIResponse<Reservation>>> DeleteReservation(string ids)
        {
            string[] strings = ids.Split(",");
            for (int i = 0; i < strings.Length; i++)
            {
                var Room = await _context.Room.FindAsync(ids[i]);
                if (Room == null)
                {
                    return Ok(new APIResponse<Room> { Code = 404, Msg = "Not Found" });

                }

                _context.Room.Remove(Room);
            }

            return Ok(new APIResponse<Reservation>() { });
        }

        private bool ReservationExists(long id)
        {
            return _context.Reservation.Any(e => e.Id == id);
        }
    }
}
