using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Produces("application/json")]
    [Route("api/Chapters")]
    public class ChaptersController : Controller
    {
        private readonly ProjectXdatabaseContext _context;

        public ChaptersController(ProjectXdatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Chapters
        [HttpGet]
        public IEnumerable<Chapters> GetChapters()
        {
            return _context.Chapters;
        }

        // GET: api/Chapters/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetChapters([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var chapters = await _context.Chapters.SingleOrDefaultAsync(m => m.ChapterId == id);

            if (chapters == null)
            {
                return NotFound();
            }

            return Ok(chapters);
        }

        //GET: api/Chapters/story/{slug}
        [Route("story/{slug}")]
        //[HttpGet("story/{slug}")]
        public IEnumerable<Chapters> GetChapterByStory([FromRoute] string slug)
        {
            var chapter = _context.Chapters.Where(c => c.Slug == slug);
            return chapter.AsQueryable();
        }

        // GET: api/Chapters/search/S/N
        [Route("search/{story}/{keyword}")]
        public IEnumerable<Chapters> GetChaptersBySearch([FromRoute] string story, string keyword)
        {
            string name = keyword;
            int nbOfChapter;
            int.TryParse(keyword, out nbOfChapter);
            string[] keywords = keyword.Split(' ');
            if (keyword.Length>0)
            {
                int.TryParse(keywords[keyword.Length - 1], out nbOfChapter);
            }
            IQueryable<Chapters> chapters = _context.Chapters.Where(
                c => c.Story.Slug.Equals(story) && (c.ChapterTitle.Equals(name)
                || c.ChapterNumber.Equals(nbOfChapter)));
            
            return chapters.AsQueryable();
        }

        // GET: api/Chapters/range/storyslug/1/10
        [Route("range/{slug}/{start}/{end}")]
        public IEnumerable<Chapters> GetChaptersByRange([FromRoute] string slug, int start, int end)
        {
            IQueryable<Chapters> chapters = from c in _context.Chapters
                                          where c.Story.Slug.Equals(slug)
                                          orderby c.ChapterNumber
                                          select c;
            int total = chapters.Count();
            if (start < 1 || (start > end && start < total) || start > total)
            {
                start = 1;
                end = 0;
            }
            if (end > total)
            {
                end = total;
            }

            int begin = start - 1;

            return chapters.Skip(begin).Take(end - begin).AsQueryable();
        }

        // GET: api/Chapters/Storyslug/0/1/10
        [Route("{slug}/{status}/{start}/{end}")]
        public IEnumerable<Chapters> GetChaptersByStatus([FromRoute] string slug, int status, int start, int end)
        {
            IQueryable<Chapters> chapters = from c in _context.Chapters
                                            where c.Story.Slug.Equals(slug) && c.ChapterStatus.Equals(status)
                                            orderby c.ChapterNumber
                                            select c;
            int total = chapters.Count();
            if (start < 1 || (start > end && start < total) || start > total)
            {
                start = 1;
                end = 0;
            }
            if (end > total)
            {
                end = total;
            }

            int begin = start - 1;

            return chapters.Skip(begin).Take(end - begin).AsQueryable();
        }

        // GET: api/Chapters/S/1
        [Route("{story}/{number}")]
        public IActionResult GetChapterByNumber([FromRoute] string story, int number)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IQueryable<Chapters> chapters = from c in _context.Chapters
                                            where c.Story.Slug.Equals(story) && c.ChapterNumber.Equals(number)
                                            orderby c.ChapterNumber
                                            select c;

            if (chapters == null)
            {
                return NotFound();
            }

            return Ok(chapters);
        }

        // PUT: api/Chapters/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutChapters([FromRoute] int id, [FromBody] Chapters chapters)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != chapters.ChapterId)
            {
                return BadRequest();
            }

            _context.Entry(chapters).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChaptersExists(id))
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

        // POST: api/Chapters
        [HttpPost]
        public async Task<IActionResult> PostChapters([FromBody] Chapters chapters)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Chapters.Add(chapters);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetChapters", new { id = chapters.ChapterId }, chapters);
        }

        // DELETE: api/Chapters/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChapters([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var chapters = await _context.Chapters.SingleOrDefaultAsync(m => m.ChapterId == id);
            if (chapters == null)
            {
                return NotFound();
            }

            //_context.Chapters.Remove(chapters);
            chapters.ChapterStatus = -1;
            _context.Entry(chapters).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(chapters);
        }

        private bool ChaptersExists(int id)
        {
            return _context.Chapters.Any(e => e.ChapterId == id);
        }
    }
}