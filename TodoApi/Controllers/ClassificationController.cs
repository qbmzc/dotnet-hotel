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
        [HttpPost("update")]
        public async Task<ActionResult<APIResponse<Classification>>> PutClassification(Classification classification)
        {


            _context.Entry(classification).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClassificationExists(classification.Id))
                {
                    return Ok(new APIResponse<Classification> { Code = 404, Msg = "Not Found" });
                }
                else
                {
                    throw;
                }
            }

            return Ok(new APIResponse<Classification>() { });
        }

        // POST: api/Classification
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("create")]
        public async Task<ActionResult<APIResponse<Classification>>> PostClassification([FromForm] Classification classification)
        {
            _context.Classification.Add(classification);
            await _context.SaveChangesAsync();
            return Ok(new APIResponse<Classification> { Data = classification });

        }

        // DELETE: api/Classification/5
        [HttpPost("delete")]
        public async Task<ActionResult<APIResponse<Classification>>> DeleteClassification([FromQuery]string ids)
        {

            string[] strings = ids.Split(",");
            for (int i = 0; i < strings.Length; i++)
            {
                var room = await _context.Classification.FindAsync(long.Parse(strings[i]));
                if (room == null)
                {
                    return Ok(new APIResponse<Classification> { Code = 404, Msg = "Not Found" });

                }

                _context.Classification.Remove(room);
            }

            await _context.SaveChangesAsync();

            return Ok(new APIResponse<Classification> { });
        }

        private bool ClassificationExists(long id)
        {
            return _context.Classification.Any(e => e.Id == id);
        }
    }
}
