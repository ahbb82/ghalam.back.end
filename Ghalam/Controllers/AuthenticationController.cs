using Ghalam.dto;
using Ghalam.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ghalam.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController(GhalamContext context) : ControllerBase
    {
        private readonly GhalamContext db = context;

        [HttpPost("WriterLogin")]
        public async Task<IActionResult> WriterLogin(LoginDTO login)
        {
            var user = await db.Users.FirstOrDefaultAsync(x => x.UserName == login.UserName);
            int num = user.Id;
            if (user == null)
            {
                return Ok(new { success = false, message = "کاربر یافت نشد", body = "" });
            }
            if (user.IsWriter == false)
            {
                return Ok(new { success = false, message = "نوع کاربری اشتباه است", body = "" });
            }
            if (user.Password == login.Password)
            {
                return Ok(new { success = true, message = "خوش آمدید", body = num });
            }
            else
            {
                return Ok(new { success = false, message = "کلمه عبور اشتباه است.", body = "" });
            }

        }
        [HttpPost("ReaderLogin")]
        public async Task<IActionResult> ReaderLogin(LoginDTO login)
        {
            var user = await db.Users.FirstOrDefaultAsync(x => x.UserName == login.UserName);
            int num = user.Id;
            if (user == null)
            {
                return Ok(new {success=false, message="کاربر یافت نشد", body = "" });
            }
            if (user.IsWriter == true)
            {
                return Ok(new { success = false, message = "نوع کاربری اشتباه است", body = "" });
            }
            if (user.Password == login.Password)
            {
                return Ok(new {success=true, message="خوش آمدید", body = num });
            }else
            {
                return Ok(new {success=false, message = "کلمه عبور اشتباه است.", body = ""});
            }
            
        }
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(RegisterDTO register)
        {
            var user = await db.Users.FirstOrDefaultAsync(x => x.UserName == register.UserName);
            if(user != null)
            {
                return Ok(new {success=false ,message="این نام کاربری قبلا انتخاب شده است", body = "" });
            }
            await db.Users.AddAsync(new User { UserName = register.UserName, Password = register.Password, IsWriter = register.isWriter });
            await db.SaveChangesAsync();
            if(register.isWriter == true)
            { 
                return Ok(new {success=true, message="مشخصات نویسنده ثبت شد", body = "" });
            }else
            {
                return Ok(new {success=true, message="مشخصات خواننده ثبت شد", body = "" });
            }
            
        }

    }
}
