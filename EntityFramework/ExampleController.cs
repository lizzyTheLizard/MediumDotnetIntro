using EntityFramework;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EntityFramework;

[ApiController]
[Route("[controller]")]
public class ExampleController(ILogger<ExampleController> logger, ExampleDbContext dbContext) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<ICollection<Example>>> GetAll()
    {
        var query = from example in dbContext.Examples select example;
        //This eager loads the SubExamples
        var results = await query.Include(x => x.SubExamples).ToListAsync();
        logger.LogDebug(101, "Returning all results: {results}", results);
        return results;
    }

    [HttpGet("ByDate/{date}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<ICollection<Example>>> GetForDate(DateOnly date)
    {
        var query = from example in dbContext.Examples where example.Date == date select example;
        var results = await query.Include(x => x.SubExamples).ToListAsync();
        logger.LogDebug(102, "Returning all results for date {date}: {results}", date, results);
        return results;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Example>> GetForId(Guid id)
    {
        var query = from example in dbContext.Examples where example.Id == id select example;
        var existing = await query.SingleOrDefaultAsync();
        if (existing == null)
        {
            logger.LogWarning(103, "No result found for {id}", id);
            return NotFound();
        }
        //This implicitely loads the SubExamples as we did not do eager loading in the query
        dbContext.Entry(existing).Collection(e => e.SubExamples!).Load();
        logger.LogDebug(103, "Returning result: {result}", existing);
        return existing;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<Example>> Create([FromBody] Example input)
    {
        var query = from example in dbContext.Examples where example.Id == input.Id select example;
        //No loading of the SubExamples as we do not need them => SubExamples will be null
        var existing = await query.SingleOrDefaultAsync();
        if (existing == null)
        {
            //This will insert the entity to the DB. You cannot "Add" an existing entity.
            dbContext.Examples.Add(input);
            await dbContext.SaveChangesAsync();
            logger.LogInformation(104, "Created {input}", input);
            return CreatedAtAction(nameof(GetForId), new { id = input.Id }, input);
        }
        //Need to manually check the concurency here as SQLite cannot update itself (like Timestamp).
        if (existing.Timestamp != input.Timestamp)
        {
            logger.LogWarning(104, "Conflict for {input} with {existing}", input, existing);
            return Conflict("Example has been changed while editing");
        }
        //We cannot use "Update" here as this entity is alreay in the current context. So we have to edit the existing entity
        //Just overwrite all values and set timestamp manually
        dbContext.Entry(existing).CurrentValues.SetValues(input);
        existing.Timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
        //This will save all changes in one transaction
        await dbContext.SaveChangesAsync();
        logger.LogInformation(104, "Updated {id} to {existing}", input.Id, existing);
        return CreatedAtAction(nameof(GetForId), new{ id = input.Id }, existing);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<Example>> Update(Guid id, [FromBody] Example input)
    {
        if (id != input.Id)
        {
            logger.LogWarning(105, "ID {id} in path but {id2} in body", id, input.Id);
            return BadRequest("Invalid ID in Body");
        }
        var query = from example in dbContext.Examples where example.Id == id select example;
        var existing = await query.SingleOrDefaultAsync();
        if (existing == null)
        {
            logger.LogWarning(105, "No result found for {id}", id);
            return NotFound();
        }
        if (existing.Timestamp != input.Timestamp)
        {
            logger.LogWarning(105, "Conflict for {input} with {existing}", input, existing);
            return Conflict("Example has been changed while editing");
        }
        dbContext.Entry(existing).CurrentValues.SetValues(input);
        existing.Timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
        await dbContext.SaveChangesAsync();
        logger.LogInformation(105, "Updated {id} to {result}", existing.Id, existing);
        return existing;
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<Example>> Delete(Guid id)
    {
        var query = from example in dbContext.Examples where example.Id == id select example;
        var existing = await query.SingleOrDefaultAsync();
        if (existing == null)
        {
            logger.LogInformation(106, "Cannot delete {id} as it does not exist", id);
            return NoContent();
        }
        dbContext.Examples.Remove(existing);
        await dbContext.SaveChangesAsync();
        logger.LogInformation(106, "Deleted {existing}", existing);
        return NoContent();
    }


    private async Task<ActionResult<UserConfiguration>> GetFavorite(ExampleDbContext dbContext, ILogger<Program> logger)
    {
        var query = from f in dbContext.UserConfiguration where f.Name == UserConfigurationName.Favorite select f;
        var favorite = await query.SingleOrDefaultAsync();
        if (favorite == null)
        {
            logger.LogInformation(107, "Cannot get favorite as none is set");
            return NotFound();
        }
        logger.LogInformation(107, "Return favorite {favorite}", favorite);
        dbContext.Entry(favorite.Example).Collection(e => e.SubExamples!).Load();
        return favorite;
    }

    private async Task<ActionResult<UserConfiguration>> SetFavorite([FromBody] Guid exampleGuid, ExampleDbContext dbContext, ILogger<Program> logger)
    {
        var query = from example in dbContext.Examples where example.Id == exampleGuid select example;
        var existing = await query.SingleOrDefaultAsync();
        if (existing == null)
        {
            logger.LogInformation(108, "Cannot set favorite for {exampleGuid} as it does not exist", exampleGuid);
            return NotFound();
        }
        var toBeAdded = new UserConfiguration { Name = UserConfigurationName.Favorite, Example = existing };
        var favorite = dbContext.UserConfiguration.Update(toBeAdded);
        await dbContext.SaveChangesAsync();
        //Load explicitely the SubExamples to send them as result
        dbContext.Entry(favorite.Entity.Example).Collection(e => e.SubExamples!).Load();
        logger.LogInformation(108, "Set {existing} as favorites", existing);
        return CreatedAtRoute("GetFavorite", new { }, favorite.Entity);
    }
}
