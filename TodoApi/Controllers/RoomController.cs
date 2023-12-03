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
        [HttpGet("list")]
        public async Task<ActionResult<APIResponse<IEnumerable<Room>>>> GetRoom()
        {
            List<Room> rooms = await _context.Room.ToListAsync();

            return Ok(new APIResponse<IEnumerable<Room>> { Data = rooms });
        }

        // GET: api/Room/5
        [HttpGet("detail")]
        public async Task<ActionResult<APIResponse<Room>>> GetRoom(long id)
        {
            var room = await _context.Room.FindAsync(id);

            if (room == null)
            {
                    return Ok(new APIResponse<Room> { Code = 404, Msg = "Not Found" });
            }

            return Ok(new APIResponse<Room> { Data = room });
        }

        // PUT: api/Room/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("update")]
        public async Task<ActionResult<APIResponse<Room>>> PutRoom(Room room)
        {

            _context.Entry(room).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoomExists(room.Id))
                {
                    return Ok(new APIResponse<Room> { Code = 404, Msg = "Not Found" });
                }
                else
                {
                    throw;
                }
            }

            return Ok(new APIResponse<Room> { });
        }

        // POST: api/Room
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("create")]
        public async Task<ActionResult<APIResponse<Room>>> PostRoom(Room room, IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", file.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                room.Cover = file.FileName;
            }
            _context.Room.Add(room);
            await _context.SaveChangesAsync();

            return Ok(new APIResponse<Room> { Data=room});
        }

        // DELETE: api/Room/5
        [HttpPost("delete")]
        public async Task<ActionResult<APIResponse<Room>>> DeleteRoom(string ids)
        {
            string[] strings = ids.Split(",");
            for (int i = 0; i < strings.Length; i++)
            {
                var Room = await _context.Room.FindAsync(ids[i]);
                if (Room == null)
                {
                    return Ok(new APIResponse<Room> { Code = 404, Msg = "Not Found" });

                }

                _context.Room.Remove(Room);
            }

            await _context.SaveChangesAsync();

            return Ok(new APIResponse<Room> { });
        }

        private bool RoomExists(long id)
        {
            return _context.Room.Any(e => e.Id == id);
        }
    }
}
