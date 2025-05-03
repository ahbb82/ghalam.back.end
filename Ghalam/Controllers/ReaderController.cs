using Ghalam.dto;
using Ghalam.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ghalam.Controllers
{
    [Route("api/story")]
    [ApiController]
    public class ReaderController(GhalamContext context) : ControllerBase
    {
        private readonly GhalamContext db = context;

        [HttpGet("GetStory")]
        public async Task<IActionResult> GetStories()
        {

            var stories = await db.Stories.Select(x => new {id=x.PkStory,title=x.Title,text=x.Text}).ToListAsync();
            return Ok(new {success = true, message = "", body = stories});
        }

        [HttpPost("LikeCounter")]
        public async Task<IActionResult> LikeCounter(GetWriterStorieDTO ID)
        {
            var count = await db.LikedStories.CountAsync(x => x.FkStory == ID.WriterID);
            return Ok(new { success = true, message = "", body = count });
        }

        [HttpPost("IsLiked")]
        public async Task<IActionResult> IsLiked(LikingDTO IL)
        {
            bool is_liked;
            var Liked = await db.LikedStories.FirstOrDefaultAsync(x => x.FkUser == IL.ReaderID && x.FkStory == IL.StoryID);
            if (Liked == null)
            {
                is_liked = false;
            }else{
                is_liked = true;
            }
            return Ok(new { success = true, message = "", body = is_liked});
        }

        [HttpPost("Liking")]
        public async Task<IActionResult> Liking(LikingDTO liking)
        {
            await db.LikedStories.AddAsync(new LikedStory { FkStory = liking.StoryID, FkUser = liking.ReaderID});
            db.SaveChanges();
            return Ok(new {success = true, message = "داستان لایک شد", body =true});
        }

        [HttpPost("DisLiking")]
        public async Task<IActionResult> DisLiking(LikingDTO liking)
        {
            await db.LikedStories
                .Where(x => x.FkUser == liking.ReaderID)
                .Where(x => x.FkStory == liking.StoryID)
                .ExecuteDeleteAsync();
            db.SaveChanges();
            return Ok(new {success = true, message = "داستان دیس لایک شد" ,body = false});
        }
    }


}
