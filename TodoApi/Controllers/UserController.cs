using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.SwaggerGen;
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
        public async Task<ActionResult<APIResponse<IEnumerator<User>>>> ListUser([FromQuery] string? keyword)
        {
            List<User>? rooms = null;
            if (keyword != null)
            {
                rooms = await _context.User.Where(u => u.Username == keyword).ToListAsync();

            }
            else
            {

                rooms = await _context.User.ToListAsync();
            }

            return Ok(new APIResponse<IEnumerable<User>> { Data = rooms });
        }

        // GET: api/User
        [HttpGet("detail")]
        public async Task<ActionResult<APIResponse<User>>> DetailUser(long userId)
        {
            var user = await _context.User.FindAsync(userId);

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
        public async Task<ActionResult<APIResponse<User>>> PutUser([FromForm] User user, IFormFile? avatarFile)
        {
            if (avatarFile != null && avatarFile.Length > 0)
            {
                if (avatarFile.FileName.EndsWith(".jpg") || avatarFile.FileName.EndsWith(".jpeg") || avatarFile.FileName.EndsWith(".png"))
                {
                    string v = await GetBase64StringAsync(avatarFile);
                    user.Avatar = v;
                }
                else
                {
                    return Ok(new APIResponse<Room>() { Code = 400, Msg = "Format not supported" });
                }

            }

            User? user1 = await _context.User.FindAsync(user.Id);
            if(user1==null){
                return Ok(new APIResponse<User>(){Code=404,Msg=" User IS NOT FOUND"});
            }
            user1.Email=user.Email;
            user1.City=user.City;
            user1.CardType=user.CardType;
            user1.Country=user.Country;
            user1.Mobile=user.Mobile;
            user1.PhonePrefix=user.PhonePrefix;
            user1.FirstName=user.FirstName;
            user1.LastName=user.LastName;
            user1.CreditCard=user.CreditCard;
            user1.Holder=user.Holder;
            user1.ExpirationDate=user.ExpirationDate;
            user1.State=user.State;
            user1.PostalCode=user.PostalCode;
            user1.Nickname=user.Nickname;

            _context.Entry(user1).State = EntityState.Modified;

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

          //将图片文件转换成base64字符串
        private async Task<string> GetBase64StringAsync(IFormFile image)
        {
            using (var memoryStream = new MemoryStream())
            {
                await image.CopyToAsync(memoryStream);
                return Convert.ToBase64String(memoryStream.ToArray());
            }
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
#pragma warning disable CS8600 // 将 null 字面量或可能为 null 的值转换为非 null 类型。
            User responseUser = await _context.User.FirstOrDefaultAsync(u => u.Username == user.Username && u.Password == encryptPwd);
#pragma warning restore CS8600 // 将 null 字面量或可能为 null 的值转换为非 null 类型。
            if (responseUser == null)
            {
                return Ok(new APIResponse<User> { Code = 404, Msg = "user is not found" });
            }

            if(user.Role!="0"){
                return Ok(new APIResponse<User>{Code=413,Msg="forbidden"});
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


#pragma warning disable CS8600 // 将 null 字面量或可能为 null 的值转换为非 null 类型。
            User responseUser = await _context.User.FirstOrDefaultAsync(u => u.Username == user.Username && u.Password == encryptPwd);
#pragma warning restore CS8600 // 将 null 字面量或可能为 null 的值转换为非 null 类型。
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

#pragma warning disable CS8604 // 引用类型参数可能为 null。
            string encryptPwd = EncryptUtils.Encrypt(user.Password);
#pragma warning restore CS8604 // 引用类型参数可能为 null。
            user.Password = encryptPwd;
            user.Token = encryptPwd;
            user.Status = "0";
            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new APIResponse<User>() { Data = user });
        }

        //更新密码
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
        public async Task<ActionResult<APIResponse<User>>> RegUser([FromForm] User user)
        {
            if (user.RePassword == null || user.Username == null || user.Password == null)
            {
                return Ok(new APIResponse<User> { Code = 405, Msg = "invalid parameter" });
            }
            //查重复
#pragma warning disable CS8600 // 将 null 字面量或可能为 null 的值转换为非 null 类型。
            User user2 = await _context.User.FirstOrDefaultAsync(u => u.Username == user.Username);
#pragma warning restore CS8600 // 将 null 字面量或可能为 null 的值转换为非 null 类型。
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
#pragma warning disable CS8602 // 解引用可能出现空引用。
            resuser.Password = null;
#pragma warning restore CS8602 // 解引用可能出现空引用。
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
