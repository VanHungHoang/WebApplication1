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
    [Route("api/Stories")]
    public class StoriesController : Controller
    {
        private readonly ProjectXdatabaseContext _context;

        public StoriesController(ProjectXdatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Stories
        [HttpGet]
        public IEnumerable<Stories> GetStories()
        {
            return _context.Stories;
        }
        
        // GET: api/Stories/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStories([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var stories = await _context.Stories.SingleOrDefaultAsync(m => m.StoryId == id);

            if (stories == null)
            {
                return NotFound();
            }

            return Ok(stories);
        }

        // GET: api/Stories/range/1/5
        [Route("range/{start}/{end}")]
        public IActionResult GetStoriesByRange([FromRoute] int start, int end)
        {
             IQueryable<Stories> stories = from s in _context.Stories
                                           where s.StoryStatus == 1
                                           orderby s.StoryName
                                           select s;
            int total = stories.Count();
            if (total == 0)
            {
                return NoContent();
            }
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

            stories = stories.Skip(begin).Take(end - begin).AsQueryable();

            return Ok(stories);
        }

        // GET: api/Stories/author/A/1/5
        [Route("author/{authorname}/{start}/{end}")]
        public IActionResult GetStoriesByAuthor([FromRoute] string authorname, int start, int end)
        {
            IQueryable<Stories> stories = from s in _context.Stories
                                          where (s.Author.AuthorName.Equals(authorname) || s.Author.Slug.Equals(authorname)) 
                                                && s.StoryStatus == 1
                                          orderby s.StoryName
                                          select s;
            int total = stories.Count();
            if (total == 0)
            {
                return NoContent();
            }
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

            stories = stories.Skip(begin).Take(end - begin).AsQueryable();

            return Ok(stories);
        }

        // GET: api/Stories/genre/G/1/5
        [Route("genre/{genrename}/{start}/{end}")]
        public IActionResult GetStoriesByGenre([FromRoute] string genrename, int start, int end)
        {
            //object[]
            IQueryable<Stories> stories = from s in _context.StoryGenre
                                          where (s.Genre.GenreName.Equals(genrename) || s.Genre.Slug.Equals(genrename))
                                                && s.Story.StoryStatus == 1
                                          orderby s.Story.StoryName
                                          select s.Story;
            int total = stories.Count();
            if (total == 0)
            {

                return NoContent(); 
            }
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

            stories = stories.Skip(begin).Take(end - begin).AsQueryable();
            return Ok(stories);
        }
        // GET: api/Stories/name/N
        [Route("name/{storyslug}")]
        public IActionResult GetStoriesByName([FromRoute] string storyslug, int start, int end)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var stories =  _context.Stories.Where(s => s.Slug.Equals(storyslug));

            if (stories == null)
            {
                return NotFound();
            }
            return Ok(stories);
        }



        // PUT: api/Stories/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStories([FromRoute] int id, [FromBody] Stories stories)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != stories.StoryId)
            {
                return BadRequest();
            }

            _context.Entry(stories).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StoriesExists(id))
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

        // POST: api/Stories
        [HttpPost]
        public async Task<IActionResult> PostStories([FromBody] Stories stories)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Stories.Add(stories);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStories", new { id = stories.StoryId }, stories);
        }

        // DELETE: api/Stories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStories([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var stories = await _context.Stories.SingleOrDefaultAsync(m => m.StoryId == id);
            if (stories == null)
            {
                return NotFound();
            }

            _context.Stories.Remove(stories);
            await _context.SaveChangesAsync();

            return Ok(stories);
        }

        private bool StoriesExists(int id)
        {
            return _context.Stories.Any(e => e.StoryId == id);
        }
    }
}