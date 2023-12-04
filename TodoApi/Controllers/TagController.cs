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
    public class TagController : ControllerBase
    {
        private readonly TodoContext _context;

        public TagController(TodoContext context)
        {
            _context = context;
        }

        // GET: api/Tag
        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<Tag>>> GetTag()
        {
            List<Tag> rooms = await _context.Tag.ToListAsync();

            return Ok(new APIResponse<IEnumerable<Tag>> { Data = rooms });
        }

        // GET: api/Tag/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tag>> GetTag(long id)
        {
            var tag = await _context.Tag.FindAsync(id);

            if (tag == null)
            {
                return NotFound();
            }

            return tag;
        }

        // PUT: api/Tag/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("update")]
        public async Task<IActionResult> PutTag(Tag tag)
        {


            _context.Entry(tag).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TagExists(tag.Id))
                {
                    return Ok(new APIResponse<Tag> { Code = 404, Msg = "Not Found" });
                }
                else
                {
                    throw;
                }
            }

            return Ok(new APIResponse<Tag> { });
        }

        // POST: api/Tag
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("create")]
        public async Task<ActionResult<APIResponse<Tag>>> PostTag(Tag tag)
        {
            tag.CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            _context.Tag.Add(tag);
            await _context.SaveChangesAsync();

            return Ok(new APIResponse<Tag> { Data = tag });
        }

        // DELETE: api/Tag/5
        [HttpPost("delete")]
        public async Task<ActionResult<APIResponse<Tag>>> DeleteTag(string ids)
        {
            string[] strings = ids.Split(",");
            for (int i = 0; i < strings.Length; i++)
            {
                var tag = await _context.Tag.FindAsync(ids[i]);
                if (tag == null)
                {
                    return Ok(new APIResponse<Tag> { Code = 404, Msg = "Not Found" });

                }

                _context.Tag.Remove(tag);
            }
            return Ok(new APIResponse<Tag>() { Msg = "delete success" });

        }

        private bool TagExists(long id)
        {
            return _context.Tag.Any(e => e.Id == id);
        }
    }
}
