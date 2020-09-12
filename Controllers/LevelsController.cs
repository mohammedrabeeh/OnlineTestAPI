using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineRecipeHub.Context;
using OnlineRecipeHub.Models;

namespace OnlineRecipeHub.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class LevelsController : ControllerBase
    {
        private readonly OnlineRecipeHubContext _context;

        public LevelsController(OnlineRecipeHubContext context)
        {
            _context = context;
        }

        // GET: api/Levels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Level>>> GetLevel()
        {
            return await _context.Level.ToListAsync();
        }

        // GET: api/Levels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Level>> GetLevel(int id)
        {
            var level = await _context.Level.FindAsync(id);

            if (level == null)
            {
                return NotFound();
            }

            return level;
        }

        // PUT: api/Levels/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLevel(int id, Level level)
        {
            if (id != level.LevelId)
            {
                return BadRequest();
            }

            _context.Entry(level).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LevelExists(id))
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

        // POST: api/Levels
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Level>> PostLevel(Level level)
        {
            _context.Level.Add(level);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLevel", new { id = level.LevelId }, level);
        }

        // DELETE: api/Levels/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Level>> DeleteLevel(int id)
        {
            var level = await _context.Level.FindAsync(id);
            if (level == null)
            {
                return NotFound();
            }

            _context.Level.Remove(level);
            await _context.SaveChangesAsync();

            return level;
        }

        private bool LevelExists(int id)
        {
            return _context.Level.Any(e => e.LevelId == id);
        }
    }
}
