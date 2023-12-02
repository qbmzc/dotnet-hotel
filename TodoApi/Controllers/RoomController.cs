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
    public class RoomController : ControllerBase
    {
        private readonly TodoContext _context;

        public RoomController(TodoContext context)
        {
            _context = context;
        }

        // GET: api/Room
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Room>>> GetRoom()
        {
            return await _context.Room.ToListAsync();
        }

        // GET: api/Room/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Room>> GetRoom(long id)
        {
            var Room = await _context.Room.FindAsync(id);

            if (Room == null)
            {
                return NotFound();
            }

            return Room;
        }

        // PUT: api/Room/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRoom(long id, Room Room)
        {
            if (id != Room.Id)
            {
                return BadRequest();
            }

            _context.Entry(Room).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoomExists(id))
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

        // POST: api/Room
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Room>> PostRoom(Room Room)
        {
            _context.Room.Add(Room);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRoom", new { id = Room.Id }, Room);
        }

        // DELETE: api/Room/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoom(long id)
        {
            var Room = await _context.Room.FindAsync(id);
            if (Room == null)
            {
                return NotFound();
            }

            _context.Room.Remove(Room);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RoomExists(long id)
        {
            return _context.Room.Any(e => e.Id == id);
        }
    }
}
