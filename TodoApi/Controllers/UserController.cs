using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Identity;
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
        public async Task<ActionResult<APIResponse<User>>> DetailUser(long id)
        {
            var user = await _context.User.FindAsync(id);

            if (user == null)
            {
                return NotFound(new APIResponse<User> { Code = 404, Msg = "user is not found" });
            }

            return Ok(new APIResponse<User>() { Data = user });
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
        [HttpPost("updateUserInfo")]
        public async Task<ActionResult<APIResponse<User>>> PutUser(User user)
        {


            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(user.Id))
                {
                    return NotFound(new APIResponse<User> { Code = 404, Msg = "user is not found" });
                }
                else
                {
                    throw;
                }
            }

            return Ok(new APIResponse<User> { });
        }
        //后台用户登录
        [HttpPost("login")]
        public async Task<ActionResult<APIResponse<User>>> Login([FromForm] User user)
        {
            if (user.Username == null || user.Password == null)
            {
                return Ok(new APIResponse<User> { Code = 405, Msg = "username or password is null" });
            }
            string encryptPwd = EncryptUtils.Encrypt(user.Password);
            user.Password = encryptPwd;
            User responseUser = await _context.User.FirstOrDefaultAsync(u => u.Username == user.Username && u.Password == encryptPwd);
            if (responseUser == null)
            {
                return Ok(new APIResponse<User> { Code = 404, Msg = "user is not found" });
            }

            responseUser.Password = null;
            responseUser.RePassword = null;
            return Ok(new APIResponse<User> { Data = responseUser });

        }

        //用户登录
        [HttpPost("userLogin")]
        public async Task<ActionResult<APIResponse<User>>> UserLogin([FromForm] User user)
        {
            if (user.Username == null || user.Password == null)
            {
                return Ok(new APIResponse<User> { Code = 400, Msg = "username or password is null" });
            }
            string encryptPwd = EncryptUtils.Encrypt(user.Password);
            // User user2 = await _context.User.FirstOrDefaultAsync(u => u.Username == user.Username);


            User responseUser = await _context.User.FirstOrDefaultAsync(u => u.Username == user.Username && u.Password == encryptPwd);
            if (responseUser == null)
            {
                return Ok(new APIResponse<User> { Code = 404, Msg = "user is not found" });
            }

            responseUser.Password = null;
            responseUser.RePassword = null;
            return Ok(new APIResponse<User> { Code = 200, Msg = "success", Data = responseUser });

        }

        //创建管理员
        // POST: api/Tag
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("create")]
        public async Task<ActionResult<APIResponse<User>>> PostUser(User user)
        {

            string encryptPwd = EncryptUtils.Encrypt(user.Password);
            user.Password = encryptPwd;
            user.Token = encryptPwd;
            user.Status = "0";
            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new APIResponse<User>() { Data = user });
        }

        //创建管理员
        // POST: api/Tag
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("updatePwd")]
        public async Task<ActionResult<APIResponse<User>>> UpdatePwd(long userId, string password, string newPassword)
        {
            //查询
            var user = await _context.User.FindAsync(userId);
            if (user == null)
            {
                return Ok(new APIResponse<User> { Msg = "user is null", Code = 404 });
            }
            if (user.Status != "1")
            {
                return Ok(new APIResponse<User> { Msg = "Illegal Operations", Code = 413 });

            }
            string oldPwd = EncryptUtils.Encrypt(password);
            if (oldPwd != user.Password)
            {
                return Ok(new APIResponse<User> { Msg = "The original password is incorrect", Code = 413 });

            }

            string newPwd = EncryptUtils.Encrypt(newPassword);
            user.Password = newPwd;
            _context.Entry(user).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(user.Id))
                {
                    return Ok(new APIResponse<User> { Code = 404, Msg = "not found" });
                }
                else
                {
                    throw;
                }
            }

            return Ok(new APIResponse<User> { });
        }
        // POST: api/User
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("userRegister")]
        public async Task<ActionResult<APIResponse<User>>> RegUser(User user)
        {
            if (user.RePassword == null || user.Username == null || user.Password == null)
            {
                return Ok(new APIResponse<User> { Code = 405, Msg = "invalid parameter" });
            }
            //查重复
            User user2 = await _context.User.FirstOrDefaultAsync(u => u.Username == user.Username);
            if (user2 != null)
            {
                return Ok(new APIResponse<User> { Code = 405, Msg = "repeat of user name" });
            }
            // public static final int NormalUser = 1;
            // public static final int DemoUser = 2;
            // public static final int AdminUser = 3;
            user.Role = "1";
            DateTime currentTime = DateTime.Now;
            string timeString = currentTime.ToString("yyyy-MM-dd HH:mm:ss");
            user.CreateTime = timeString;

            string encryptPwd = EncryptUtils.Encrypt(user.Password);
            user.Password = encryptPwd;
            user.Token = encryptPwd;
            user.Status = "0";

            _context.User.Add(user);
            await _context.SaveChangesAsync();
            user.Password = null;
            user.RePassword = null;
            return Ok(new APIResponse<User> { Data = user });

        }

        private async Task<ActionResult<User>> GetUserById(long id)
        {

            var resuser = await _context.User.FindAsync(id);
            resuser.Password = null;
            return resuser;
        }

        // DELETE: api/User/5
        [HttpPost("delete")]
        public async Task<ActionResult<APIResponse<User>>> DeleteUser(long id)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return Ok(new APIResponse<User>() { Msg = "delete success" });
        }

        private bool UserExists(long id)
        {
            return _context.User.Any(e => e.Id == id);
        }
    }
}
