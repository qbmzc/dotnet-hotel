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
    public class RoomCollectController : ControllerBase
    {
        private readonly TodoContext _context;

        public RoomCollectController(TodoContext context)
        {
            _context = context;
        }

        // GET: api/RoomCollect
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoomCollect>>> GetRoomCollect()
        {
            return await _context.RoomCollect.ToListAsync();
        }

        // GET: api/RoomCollect/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RoomCollect>> GetRoomCollect(long id)
        {
            var RoomCollect = await _context.RoomCollect.FindAsync(id);

            if (RoomCollect == null)
            {
                return NotFound();
            }

            return RoomCollect;
        }

        // PUT: api/RoomCollect/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRoomCollect(long id, RoomCollect RoomCollect)
        {
            if (id != RoomCollect.Id)
            {
                return BadRequest();
            }

            _context.Entry(RoomCollect).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoomCollectExists(id))
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

        // POST: api/RoomCollect
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RoomCollect>> PostRoomCollect(RoomCollect RoomCollect)
        {
            _context.RoomCollect.Add(RoomCollect);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRoomCollect", new { id = RoomCollect.Id }, RoomCollect);
        }

        // DELETE: api/RoomCollect/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoomCollect(long id)
        {
            var RoomCollect = await _context.RoomCollect.FindAsync(id);
            if (RoomCollect == null)
            {
                return NotFound();
            }

            _context.RoomCollect.Remove(RoomCollect);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RoomCollectExists(long id)
        {
            return _context.RoomCollect.Any(e => e.Id == id);
        }
    }
}
