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
    public class ConfirmationController : ControllerBase
    {
        private readonly TodoContext _context;

        public ConfirmationController(TodoContext context)
        {
            _context = context;
        }

        // GET: api/Confirmation
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Confirmation>>> GetConfirmation()
        {
            return await _context.Confirmation.ToListAsync();
        }

        // GET: api/Confirmation/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Confirmation>> GetConfirmation(long id)
        {
            var confirmation = await _context.Confirmation.FindAsync(id);

            if (confirmation == null)
            {
                return NotFound();
            }

            return confirmation;
        }

        // PUT: api/Confirmation/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutConfirmation(long id, Confirmation confirmation)
        {
            if (id != confirmation.Id)
            {
                return BadRequest();
            }

            _context.Entry(confirmation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConfirmationExists(id))
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

        // POST: api/Confirmation
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Confirmation>> PostConfirmation(Confirmation confirmation)
        {
            _context.Confirmation.Add(confirmation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetConfirmation", new { id = confirmation.Id }, confirmation);
        }

        // POST: api/Confirmation
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("generateConfirmation")]
        public async Task<ActionResult<Confirmation>> generateConfirmation()
        {

            var confirmation = new Confirmation() { ConfirmationNumber = Guid.NewGuid().ToString() };
            _context.Confirmation.Add(confirmation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetConfirmation", new { id = confirmation.Id }, confirmation);
        }

        // DELETE: api/Confirmation/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConfirmation(long id)
        {
            var confirmation = await _context.Confirmation.FindAsync(id);
            if (confirmation == null)
            {
                return NotFound();
            }

            _context.Confirmation.Remove(confirmation);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ConfirmationExists(long id)
        {
            return _context.Confirmation.Any(e => e.Id == id);
        }
    }
}
