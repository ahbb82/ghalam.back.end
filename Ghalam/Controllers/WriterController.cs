using Ghalam.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Ghalam.dto;
using Microsoft.EntityFrameworkCore;

namespace Ghalam.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WriterController(GhalamContext context) : ControllerBase
    {
        private readonly GhalamContext db = context;

        [HttpPost("GetWriterStories")]
        public async Task<IActionResult> GetWriterStories(GetWriterStorieDTO Writer)
        {
            var writerStories = await db.Stories.Where(x => x.FkWriter == Writer.WriterID).Select(x => new { id = x.PkStory, title = x.Title, text = x.Text }).ToListAsync();
            return Ok(new {success=true, message="",body = writerStories});
        }

        [HttpPost("AddStory")]
        public async Task<IActionResult> AddStory(AddStoryDTO story)
        {
            await db.Stories.AddAsync(new Story { Text = story.Text, Title = story.Title, FkWriter = story.WriterID });
            db.SaveChanges();
            return Ok(new {success = true, message = "داستان شما ذخیره شد",body = "" });
        }

    }
}
