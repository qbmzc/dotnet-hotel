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
        public async Task<ActionResult<APIResponse<IEnumerable<Room>>>> GetRoom(long? c,
        string? keyword)
        {
            List<Room>? rooms = null;
            if (c != null)
            {
                rooms = await _context.Room.Where(r => r.ClassificationId == c).ToListAsync();
            }
            if (keyword != null)
            {
                rooms = await _context.Room.Where(r => r.Title == keyword).ToListAsync();
            }

            rooms = await _context.Room.ToListAsync();

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
        public async Task<ActionResult<APIResponse<Room>>> PostRoom([FromForm] Room room, IFormFile imageFile)
        {

            if (imageFile != null && imageFile.Length > 0)
            {
                if (imageFile.FileName.EndsWith(".jpg") || imageFile.FileName.EndsWith(".jpeg") || imageFile.FileName.EndsWith(".png"))
                {
                    string v = await GetBase64StringAsync(imageFile);
                    room.Cover = v;
                }
                else
                {
                    return Ok(new APIResponse<Room>() { Code = 400, Msg = "Format not supported" });
                }

            }
            _context.Room.Add(room);
            await _context.SaveChangesAsync();

            return Ok(new APIResponse<Room> { Data = room });
        }

        [HttpPost("upload")]
        public async Task<IActionResult> SaveImage(IFormFile image)
        {
            if (image == null || image.Length == 0)
            {
                return BadRequest("请选择一个图片文件");
            }

            // 将图片文件转换为base64字符串
            var base64String = await GetBase64StringAsync(image);

            // 将base64字符串保存到数据库中
            var result = await SaveBase64StringToDatabase(base64String);

            if (result)
            {
                return Ok("图片上传成功");
            }
            else
            {
                return BadRequest("图片上传失败");
            }
        }
        //将图片文件转换成base64字符串
        private async Task<string> GetBase64StringAsync(IFormFile image)
        {
            using (var memoryStream = new MemoryStream())
            {
                await image.CopyToAsync(memoryStream);
                return Convert.ToBase64String(memoryStream.ToArray());
            }
        }

        private async Task<bool> SaveBase64StringToDatabase(string base64String)
        {
            Console.WriteLine(base64String);
            // 这里需要根据实际情况编写将base64字符串保存到数据库的逻辑
            // 例如，使用Entity Framework或其他ORM框架将数据插入到数据库中
            return true; // 返回true表示保存成功，返回false表示保存失败
        }

        // DELETE: api/Room/5
        [HttpPost("delete")]
        public async Task<ActionResult<APIResponse<Room>>> DeleteRoom(string ids)
        {
            string[] strings = ids.Split(",");
            for (int i = 0; i < strings.Length; i++)
            {
                var room = await _context.Room.FindAsync(long.Parse(strings[i]));
                if (room == null)
                {
                    return Ok(new APIResponse<Room> { Code = 404, Msg = "Not Found" });

                }

                _context.Room.Remove(room);
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
