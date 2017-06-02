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
    [Route("api/Genres")]
    public class GenresController : Controller
    {
        private readonly ProjectXdatabaseContext _context;

        public GenresController(ProjectXdatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Genres
        [HttpGet]
        public IEnumerable<Genres> GetGenres()
        {
            return _context.Genres;
        }

        // GET: api/Genres/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGenres([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var genres = await _context.Genres.SingleOrDefaultAsync(m => m.GenreId == id);

            if (genres == null)
            {
                return NotFound();
            }

            return Ok(genres);
        }

        //GET: api/Genres/name/{slug}
        [Route("name/{slug}")]
        [HttpGet("{slug}")]
        public IEnumerable<Genres> GetGenresByName([FromRoute] string slug)
        {
            IQueryable<Genres> genres = _context.Genres.Where(g => g.Slug.Equals(slug));
            return genres;
        }

        //GET: api/Genres/0/1/5
        [Route("{status}/{start}/{end}")]
        public IEnumerable<Genres> GetGenresByStatus([FromRoute] int status, int start, int end)
        {
            IQueryable<Genres> genres = from g in _context.Genres
                                        where g.GenreStatus == status
                                        orderby g.GenreName
                                        select g;
            int total = genres.Count();
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

            return genres.Skip(begin).Take(end - begin).AsQueryable();
        }


        // PUT: api/Genres/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGenres([FromRoute] int id, [FromBody] Genres genres)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != genres.GenreId)
            {
                return BadRequest();
            }

            _context.Entry(genres).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GenresExists(id))
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

        // PUT: api/Genres/change/{id}/{status}
        //[Route("change/{id}/{status}")]
        [HttpPut("change/{id}/{status}")]
        public async Task<IActionResult> PutGenresChange([FromHeader] int id, int status)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var genres = await _context.Genres.SingleOrDefaultAsync(m => m.GenreId == id);
            if (id != genres.GenreId)
            {
                return BadRequest();
            }
            genres.GenreStatus = status;
            _context.Entry(genres).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GenresExists(id))
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

        // POST: api/Genres
        [HttpPost]
        public async Task<IActionResult> PostGenres([FromBody] Genres genres)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Genres.Add(genres);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGenres", new { id = genres.GenreId }, genres);
        }

        // DELETE: api/Genres/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGenres([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var genres = await _context.Genres.SingleOrDefaultAsync(m => m.GenreId == id);
            if (genres == null)
            {
                return NotFound();
            }

            //_context.Genres.Remove(genres);
            genres.GenreStatus = -1;
            foreach (var storyGenre in genres.StoryGenre)
            {
                storyGenre.Story.StoryStatus = -1;
            }
            await _context.SaveChangesAsync();

            return Ok(genres);
        }

        private bool GenresExists(int id)
        {
            return _context.Genres.Any(e => e.GenreId == id);
        }
    }
}