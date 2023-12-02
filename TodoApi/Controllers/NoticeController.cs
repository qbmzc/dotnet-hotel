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
    public class NoticeController : ControllerBase
    {
        private readonly TodoContext _context;

        public NoticeController(TodoContext context)
        {
            _context = context;
        }

        // GET: api/Notice
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Notice>>> GetNotice()
        {
            return await _context.Notice.ToListAsync();
        }

        // GET: api/Notice/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Notice>> GetNotice(long id)
        {
            var notice = await _context.Notice.FindAsync(id);

            if (notice == null)
            {
                return NotFound();
            }

            return notice;
        }

        // PUT: api/Notice/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNotice(long id, Notice notice)
        {
            if (id != notice.Id)
            {
                return BadRequest();
            }

            _context.Entry(notice).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NoticeExists(id))
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

        // POST: api/Notice
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Notice>> PostNotice(Notice notice)
        {
            _context.Notice.Add(notice);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNotice", new { id = notice.Id }, notice);
        }

        // DELETE: api/Notice/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotice(long id)
        {
            var notice = await _context.Notice.FindAsync(id);
            if (notice == null)
            {
                return NotFound();
            }

            _context.Notice.Remove(notice);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool NoticeExists(long id)
        {
            return _context.Notice.Any(e => e.Id == id);
        }
    }
}
