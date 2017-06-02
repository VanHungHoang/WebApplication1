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
    [Route("api/StoryGenres")]
    public class StoryGenresController : Controller
    {
        private readonly ProjectXdatabaseContext _context;

        public StoryGenresController(ProjectXdatabaseContext context)
        {
            _context = context;
        }

        // GET: api/StoryGenres
        [HttpGet]
        public IEnumerable<StoryGenre> GetStoryGenre()
        {
            return _context.StoryGenre;
        }

        // GET: api/StoryGenres/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStoryGenre([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var storyGenre = await _context.StoryGenre.SingleOrDefaultAsync(m => m.StoryId == id);

            if (storyGenre == null)
            {
                return NotFound();
            }

            return Ok(storyGenre);
        }

        // PUT: api/StoryGenres/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStoryGenre([FromRoute] int id, [FromBody] StoryGenre storyGenre)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != storyGenre.StoryId)
            {
                return BadRequest();
            }

            _context.Entry(storyGenre).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StoryGenreExists(id))
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

        // POST: api/StoryGenres
        [HttpPost]
        public async Task<IActionResult> PostStoryGenre([FromBody] StoryGenre storyGenre)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.StoryGenre.Add(storyGenre);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (StoryGenreExists(storyGenre.StoryId))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetStoryGenre", new { id = storyGenre.StoryId }, storyGenre);
        }

        // DELETE: api/StoryGenres/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStoryGenre([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var storyGenre = await _context.StoryGenre.SingleOrDefaultAsync(m => m.StoryId == id);
            if (storyGenre == null)
            {
                return NotFound();
            }

            _context.StoryGenre.Remove(storyGenre);
            await _context.SaveChangesAsync();

            return Ok(storyGenre);
        }

        private bool StoryGenreExists(int id)
        {
            return _context.StoryGenre.Any(e => e.StoryId == id);
        }
    }
}