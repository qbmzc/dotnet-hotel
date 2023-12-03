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
    public class ClassificationController : ControllerBase
    {
        private readonly TodoContext _context;

        public ClassificationController(TodoContext context)
        {
            _context = context;
        }

        
        // GET: api/Room
        [HttpGet("list")]
        public async Task<ActionResult<APIResponse<IEnumerable<Classification>>>> GetClassification()
        {
            List<Classification> classifications = await _context.Classification.ToListAsync();

            return Ok(new APIResponse<IEnumerable<Classification>> { Data = classifications });
        }
        // GET: api/Classification/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Classification>> GetClassification(long id)
        {
            var classification = await _context.Classification.FindAsync(id);

            if (classification == null)
            {
                return NotFound();
            }

            return classification;
        }

        // PUT: api/Classification/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClassification(long id, Classification classification)
        {
            if (id != classification.Id)
            {
                return BadRequest();
            }

            _context.Entry(classification).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClassificationExists(id))
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

        // POST: api/Classification
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Classification>> PostClassification(Classification classification)
        {
            _context.Classification.Add(classification);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetClassification", new { id = classification.Id }, classification);
        }

        // DELETE: api/Classification/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClassification(long id)
        {
            var classification = await _context.Classification.FindAsync(id);
            if (classification == null)
            {
                return NotFound();
            }

            _context.Classification.Remove(classification);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ClassificationExists(long id)
        {
            return _context.Classification.Any(e => e.Id == id);
        }
    }
}
