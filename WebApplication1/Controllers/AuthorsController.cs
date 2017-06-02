using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using System.Data.SqlClient;

namespace WebApplication1.Controllers
{
    [Produces("application/json")]
    [Route("api/Authors")]
    public class AuthorsController : Controller
    {
        private readonly ProjectXdatabaseContext _context;

        public AuthorsController(ProjectXdatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Authors
        [HttpGet]
        public IEnumerable<Authors> GetAuthors()
        {
            //return _context.Authors.Where(a => a.AuthorStatus == 1);
            return _context.Authors;
        }

        // GET: api/Authors/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAuthors([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var authors = await _context.Authors.SingleOrDefaultAsync(m => m.AuthorId == id);
            
            //Get Authors by storeproceduce
            //var author = new SqlParameter("@id", id);
            //var authors = await _context.AuthorVMs.FromSql("execute dbo.spGetAuthorById @id", author).SingleOrDefaultAsync();

            if (authors == null)
            {
                return NotFound();
            }

            return Ok(authors);
        }

        // GET: api/Authors/name/{slug}        
        [Route("name/{slug}")]
        [HttpGet("{slug}")]
        public IEnumerable<Authors> GetAuthorsByName([FromRoute] string slug)
        {
            IQueryable<Authors> authors = _context.Authors.Where(a => a.Slug.Equals(slug));

            return authors;
        }

        // GET: api/Authors/0/1/5
        [Route("{status}/{start}/{end}")]
        public IEnumerable<Authors> GetAuthorsByStatus([FromRoute] int status, int start, int end)
        {
            IQueryable<Authors> authors = from a in _context.Authors
                                          where a.AuthorStatus == status
                                          orderby a.AuthorName
                                          select a;
            int total = authors.Count();

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

            return authors.Skip(begin).Take(end - begin).AsQueryable();
        }

        // PUT: api/Authors/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAuthors([FromRoute] int id, [FromBody] Authors authors)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != authors.AuthorId)
            {
                return BadRequest();
            }

            _context.Entry(authors).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuthorsExists(id))
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

        // PUT: api/Authors/change/5/1
        [Route("change/{id}/{status}")]
        [HttpPut("{id}/{status}")]
        public async Task<IActionResult> PutAuthorChange(int id, int status)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var authors = await _context.Authors.SingleOrDefaultAsync(m => m.AuthorId == id);
            if (id != authors.AuthorId)
            {
                return BadRequest();
            }
            authors.AuthorStatus = status;
            _context.Entry(authors).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuthorsExists(id))
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


        // POST: api/Authors
        [HttpPost]
        public async Task<IActionResult> PostAuthors([FromBody] Authors authors)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Authors.Add(authors);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAuthors", new { id = authors.AuthorId }, authors);
        }

        // DELETE: api/Authors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthors([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var authors = await _context.Authors.SingleOrDefaultAsync(m => m.AuthorId == id);
            if (authors == null)
            {
                return NotFound();
            }

            //_context.Authors.Remove(authors);
            authors.AuthorStatus = -1;
            foreach (var story in authors.Stories)
            {
                story.StoryStatus = -1;
            }
            await _context.SaveChangesAsync();

            return Ok(authors);
        }

        private bool AuthorsExists(int id)
        {
            return _context.Authors.Any(e => e.AuthorId == id);
        }
    }
}