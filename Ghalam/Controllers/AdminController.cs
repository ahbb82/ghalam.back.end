using Ghalam.dto;
using Ghalam.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ghalam.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController(GhalamContext context) : ControllerBase
    {
        private readonly GhalamContext db = context;

        [HttpGet("GetUser")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await db.Users.Select(x => new { id = x.Id, name = x.UserName, isWriter = x.IsWriter }).ToListAsync();
            return Ok(new {success = true, Message = "کاربر مورد نظر یافت نشد", body = users});
        }

        [HttpPost("DeleteUser")]
        public async Task<IActionResult> DeleteUser(DeleteUserDTO id)
        {
           
            await db.LikedStories
                .Where(x => x.FkUser == id.Id)
                .ExecuteDeleteAsync();

            await db.Stories
                .Where(x => x.FkWriter == id.Id)
                .ExecuteDeleteAsync();

            var user = await db.Users.FirstOrDefaultAsync(x => x.Id == id.Id);
            if (user == null)
            {
                return Ok(new {success = false, Message = "کاربر مورد نظر یافت نشد", body = "" });
            }
            db.Users.Remove(user);

            await db.SaveChangesAsync();

            return Ok(new {success = true, Message = "کاربر و اطلاعات مرتبط حذف شدند", body = "" });
        }


        [HttpPost("DeleteStory")]
        public async Task<IActionResult> DeleteStory(DeleteStoryDTO story)
        {

            await db.LikedStories
                .Where(x => x.FkStory == story.Id)
                .ExecuteDeleteAsync();

            var rowsAffected = await db.Stories
                .Where(x => x.PkStory == story.Id)
                .ExecuteDeleteAsync();

            if (rowsAffected == 0)
            {
                return Ok(new {success = false, Message = "داستان مورد نظر یافت نشد" ,body = ""});
            }

            return Ok(new {success = true, message = "داستان حذف شد", body = "" });
        }

    }
}
