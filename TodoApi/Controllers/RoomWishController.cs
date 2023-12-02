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
    public class RoomWishController : ControllerBase
    {
        private readonly TodoContext _context;

        public RoomWishController(TodoContext context)
        {
            _context = context;
        }

        // GET: api/RoomWish
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoomWish>>> GetRoomWish()
        {
            return await _context.RoomWish.ToListAsync();
        }

        // GET: api/RoomWish/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RoomWish>> GetRoomWish(long id)
        {
            var RoomWish = await _context.RoomWish.FindAsync(id);

            if (RoomWish == null)
            {
                return NotFound();
            }

            return RoomWish;
        }

        // PUT: api/RoomWish/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRoomWish(long id, RoomWish RoomWish)
        {
            if (id != RoomWish.Id)
            {
                return BadRequest();
            }

            _context.Entry(RoomWish).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoomWishExists(id))
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

        // POST: api/RoomWish
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RoomWish>> PostRoomWish(RoomWish RoomWish)
        {
            _context.RoomWish.Add(RoomWish);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRoomWish", new { id = RoomWish.Id }, RoomWish);
        }

        // DELETE: api/RoomWish/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoomWish(long id)
        {
            var RoomWish = await _context.RoomWish.FindAsync(id);
            if (RoomWish == null)
            {
                return NotFound();
            }

            _context.RoomWish.Remove(RoomWish);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RoomWishExists(long id)
        {
            return _context.RoomWish.Any(e => e.Id == id);
        }
    }
}
