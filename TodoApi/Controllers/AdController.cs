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
    public class AdController : ControllerBase
    {
        private readonly TodoContext _context;

        public AdController(TodoContext context)
        {
            _context = context;
        }

        // GET: api/Ad
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ad>>> GetAd()
        {
            return await _context.Ad.ToListAsync();
        }

        // GET: api/Ad/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Ad>> GetAd(long id)
        {
            var ad = await _context.Ad.FindAsync(id);

            if (ad == null)
            {
                return NotFound();
            }

            return ad;
        }

        // PUT: api/Ad/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAd(long id, Ad ad)
        {
            if (id != ad.Id)
            {
                return BadRequest();
            }

            _context.Entry(ad).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdExists(id))
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

        // POST: api/Ad
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Ad>> PostAd(Ad ad)
        {
            _context.Ad.Add(ad);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAd", new { id = ad.Id }, ad);
        }

        // DELETE: api/Ad/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAd(long id)
        {
            var ad = await _context.Ad.FindAsync(id);
            if (ad == null)
            {
                return NotFound();
            }

            _context.Ad.Remove(ad);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AdExists(long id)
        {
            return _context.Ad.Any(e => e.Id == id);
        }
    }
}
