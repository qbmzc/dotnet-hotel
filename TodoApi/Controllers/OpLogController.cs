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
    public class OpLogController : ControllerBase
    {
        private readonly TodoContext _context;

        public OpLogController(TodoContext context)
        {
            _context = context;
        }

        // GET: api/OpLog
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OpLog>>> GetOpLog()
        {
            return await _context.OpLog.ToListAsync();
        }

        // GET: api/OpLog/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OpLog>> GetOpLog(long id)
        {
            var opLog = await _context.OpLog.FindAsync(id);

            if (opLog == null)
            {
                return NotFound();
            }

            return opLog;
        }

        // PUT: api/OpLog/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOpLog(long id, OpLog opLog)
        {
            if (id != opLog.Id)
            {
                return BadRequest();
            }

            _context.Entry(opLog).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OpLogExists(id))
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

        // POST: api/OpLog
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<OpLog>> PostOpLog(OpLog opLog)
        {
            _context.OpLog.Add(opLog);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOpLog", new { id = opLog.Id }, opLog);
        }

        // DELETE: api/OpLog/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOpLog(long id)
        {
            var opLog = await _context.OpLog.FindAsync(id);
            if (opLog == null)
            {
                return NotFound();
            }

            _context.OpLog.Remove(opLog);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OpLogExists(long id)
        {
            return _context.OpLog.Any(e => e.Id == id);
        }
    }
}
