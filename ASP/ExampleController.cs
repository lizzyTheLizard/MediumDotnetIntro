using Microsoft.AspNetCore.Mvc;

namespace ASP
{
    [ApiController]
    [Route("[controller]")]
    public class ExampleController(ILogger<ExampleController> logger, ExampleDatabase database) : ControllerBase
    {
        private readonly ILogger<ExampleController> _logger = logger;
        private readonly ExampleDatabase _database = database;

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ICollection<Example>>> GetAll()
        {
            var results = await _database.GetAll();
            _logger.LogDebug(101, "Returning all results: {results}", results);
            return Ok(results);
        }

        [HttpGet]
        [Route("ByDate/{date}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ICollection<Example>>> GetForDate(DateOnly date)
        {
            var results = await _database.GetForDate(date);
            _logger.LogDebug(102, "Returning all results for date {date}: {results}", date, results);
            return Ok(results);
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Example>> GetForId(Guid id)
        {
            var result = await _database.GetForId(id);
            if (result == null)
            {
                _logger.LogWarning(103, "No result found for {id}", id);
                return NotFound();
            }
            _logger.LogDebug(103, "Returning result: {result}", result);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Example>> Create([FromBody] Example input)
        {
            var existing = _database.GetForId(input.Id);
            if (existing != null)
            {
                var result = await _database.Update(input);
                _logger.LogInformation(104, "Updated {existing} to {result}", existing, result);
                return Ok(result);
            }
            else
            {
                var result = await _database.Create(input);
                _logger.LogInformation(104, "Added {result}", result);
                return CreatedAtAction(nameof(GetForId), result.Id, result);
            }
        }

        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Example>> Update(Guid id, [FromBody] Example input)
        {
            if(id != input.Id)
            {
                _logger.LogWarning(105, "ID {id} in path but {id2} in body", id, input.Id);
                return BadRequest("Invalid ID in Body");
            }
            var existing = await _database.GetForId(id);
            if (existing == null)
            {
                return NotFound();
            }
            var result = await _database.Update(input);
            _logger.LogInformation(105, "Updated {existing} to {result}", existing, result);
            return Ok(result);
        }

        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<Example>> Delete(Guid id)
        {
            var existing = await _database.GetForId(id);
            if (existing == null)
            {
                return NotFound();
            }
            await _database.Delete(id);
            _logger.LogInformation(106, "Deleted {existing}", existing);
            return NoContent();
        }
    }
}
