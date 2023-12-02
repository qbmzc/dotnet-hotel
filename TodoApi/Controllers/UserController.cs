using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;
using TodoApi.Utils;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly TodoContext _context;

        public UserController(TodoContext context)
        {
            _context = context;
        }

        // GET: api/User
        // [HttpGet]
        // public async Task<ActionResult<IEnumerable<User>>> GetUser()
        // {
        //     return await _context.User.ToListAsync();
        // }

        // GET: api/User
        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<User>>> ListUser()
        {
            return await _context.User.ToListAsync();
        }

        // GET: api/User
        [HttpGet("detail")]
        public async Task<ActionResult<User>> DetailUser(long id)
        {
            var user = await _context.User.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }
        // GET: api/User/5
        // [HttpGet("{id}")]
        // public async Task<ActionResult<User>> GetUser(long id)
        // {
        //     var user = await _context.User.FindAsync(id);

        //     if (user == null)
        //     {
        //         return NotFound();
        //     }

        //     return user;
        // }

        // PUT: api/User/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(long id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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
        //后台用户登录
        [HttpPost("login")]
        public async Task<ActionResult<User>> Login(User user)
        {
            if (user.Username == null || user.Password == null)
            {
                return BadRequest();
            }
            string encryptPwd = EncryptUtils.Encrypt(user.Password);
            user.Password = encryptPwd;
            var responseUser = await _context.User.FindAsync(user);

            if (responseUser == null)
            {
                return NotFound();
            }


            return responseUser;

        }

        //后台用户登录
        [HttpPost("userLogin")]
        public async Task<ActionResult<User>> UserLogin(User user)
        {
            if (user.Username == null || user.Password == null)
            {
                return BadRequest();
            }
            string encryptPwd = EncryptUtils.Encrypt(user.Password);
            user.Password = encryptPwd;
            var responseUser = await _context.User.FindAsync(user);

            if (responseUser == null)
            {
                return NotFound();
            }


            return responseUser;

        }
        // POST: api/User
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("userRegister")]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            if (user.RePassword == null || user.Username == null || user.Password == null)
            {
                return BadRequest();
            }
            //查重复
            User user1 = new() { Username = user.Username };
#pragma warning disable CS8600 // 将 null 字面量或可能为 null 的值转换为非 null 类型。
            User user2 = await _context.User.FindAsync(user1);
#pragma warning restore CS8600 // 将 null 字面量或可能为 null 的值转换为非 null 类型。
            if (user2 != null)
            {
                return BadRequest();
            }
            // public static final int NormalUser = 1;
            // public static final int DemoUser = 2;
            // public static final int AdminUser = 3;
            user.Role = "1";
            user.CreateTime = new DateTime().ToString();

            string encryptPwd = EncryptUtils.Encrypt(user.Password);
            user.Password = encryptPwd;
            user.Token = encryptPwd;
            user.Status = "0";

            _context.User.Add(user);
            await _context.SaveChangesAsync();


            return CreatedAtAction(nameof(DetailUser), new { id = user.Id }, user);
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(long id)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(long id)
        {
            return _context.User.Any(e => e.Id == id);
        }
    }
}
