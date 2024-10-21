using Microsoft.AspNetCore.Mvc;
using StarshipProject.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Cors;

namespace StarshipProject.Controllers
{
    [EnableCors("AllowAllOrigins")]
    [Route("api/[controller]")]
    [ApiController]
    public class StarshipsController : ControllerBase
    {
        private readonly StarshipContext _context;

        public StarshipsController(StarshipContext context)
        {
            _context = context;
        }

        #region create
        [HttpPost]
        public async Task<ActionResult<Starship>> CreateStarship(Starship starship)
        {
            // Check if the starship already exists in the DB
            Starship existingStarship = await _context.Starships
                .FirstOrDefaultAsync(s => s.Name == starship.Name && s.Model == starship.Model);

            if (existingStarship != null)
            {
                return Conflict("Starship already exists in the database.");
            }

            //add new starship to the DB
            _context.Starships.Add(starship);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetStarshipById), new {id = starship.Id}, starship);
        }

        #endregion
    
        #region Read

        //retrieves starships
        [HttpGet]
        public ActionResult<IEnumerable<Starship>> GetStarships()
        {
            return _context.Starships.ToList();
        }

        //retrieves starship by a given ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Starship>> GetStarshipById(int id)
        {
            Starship starship = await _context.Starships.FindAsync(id);

            if (starship == null)
            {
                return NotFound();
            }

            return starship;
        }

        //retrieves a random starship
        [HttpGet("random")]
        public async Task<ActionResult<Starship>> GetRandomStarship()
        {
            int count = await _context.Starships.CountAsync();
            Random random = new Random();
            int randomIndex = random.Next(0,count);

            Starship randomStarship = await _context.Starships.Skip(randomIndex).FirstOrDefaultAsync();

            if(randomStarship == null)
            {
                return NotFound();
            }

            return randomStarship;
        }
        
        //retrieves starship by passed in name, model, or manufacturer
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Starship>>> GetStarshipByFilters([FromQuery] string? name, [FromQuery] string? model, [FromQuery] string? manufacturer)
        {
            IQueryable<Starship> query = _context.Starships;

            //checks if parameter is not null
            //if not null, the parameter is added to the query
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(s => s.Name.Contains(name));
            }

            //multiple parameters will have it where the query will combine them with an AND
            if (!string.IsNullOrEmpty(model))
            {
                query = query.Where(s => s.Model.Contains(model));
            }

            if (!string.IsNullOrEmpty(manufacturer))
            {
                query = query.Where(s => s.Manufacturer.Contains(manufacturer));
            }

            return await query.ToListAsync();
        }


        #endregion

        #region Update

        //updates a starship by id
        [HttpPut("{id}")]
        public async Task<ActionResult<Starship>> UpdateStarship(int id, Starship starship)
        {

            //checks to see if the id and the starship match. If not don't update
            if (id != starship.Id)
            {
                return BadRequest();
            }
            //mark the starship state as modified and then save changes. This forces the DB to update the record with the new starship
            _context.Entry(starship).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                //could not find the starship by id
                if (!StarshipExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(starship); // Return the updated starship
        }

        #endregion

        #region Delete

        //deletes the displayed starship
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStarship(int id)
        {
            Starship starship = await _context.Starships.FindAsync(id);
            
            //return error if no starship is found
            if(starship == null)
            {
                return NotFound();
            }

            //remove the starship and save the changes
            _context.Starships.Remove(starship);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        #endregion


        #region support methods

        //checks to see if the starship exists in the DB by looking at the id
        private bool StarshipExists(int id)
        {
            return _context.Starships.Any(e => e.Id == id);
        }

        #endregion
    
    }
}
