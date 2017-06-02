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
    [Route("api/UserWatches")]
    public class UserWatchesController : Controller
    {
        private readonly ProjectXdatabaseContext _context;

        public UserWatchesController(ProjectXdatabaseContext context)
        {
            _context = context;
        }

        // GET: api/UserWatches
        [HttpGet]
        public IEnumerable<UserWatch> GetUserWatch()
        {
            return _context.UserWatch;
        }

        // GET: api/UserWatches/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserWatch([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userWatch = await _context.UserWatch.SingleOrDefaultAsync(m => m.UserId == id);

            if (userWatch == null)
            {
                return NotFound();
            }

            return Ok(userWatch);
        }

        // PUT: api/UserWatches/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserWatch([FromRoute] string id, [FromBody] UserWatch userWatch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != userWatch.UserId)
            {
                return BadRequest();
            }

            _context.Entry(userWatch).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserWatchExists(id))
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

        // POST: api/UserWatches
        [HttpPost]
        public async Task<IActionResult> PostUserWatch([FromBody] UserWatch userWatch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.UserWatch.Add(userWatch);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UserWatchExists(userWatch.UserId))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetUserWatch", new { id = userWatch.UserId }, userWatch);
        }

        // DELETE: api/UserWatches/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserWatch([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userWatch = await _context.UserWatch.SingleOrDefaultAsync(m => m.UserId == id);
            if (userWatch == null)
            {
                return NotFound();
            }

            _context.UserWatch.Remove(userWatch);
            await _context.SaveChangesAsync();

            return Ok(userWatch);
        }

        private bool UserWatchExists(string id)
        {
            return _context.UserWatch.Any(e => e.UserId == id);
        }
    }
}