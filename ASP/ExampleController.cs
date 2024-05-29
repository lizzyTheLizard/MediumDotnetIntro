using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace ASP
{
    [ApiController]
    [Route("[controller]")]
    public class ExampleController(ILogger<ExampleController> logger, IOptions<ExampleOptions> options, ExampleDatabase database) : ControllerBase
    {
        private readonly ILogger<ExampleController> _logger = logger;
        private readonly ExampleDatabase _database = database;
        private readonly ExampleOptions _options = options.Value;

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ICollection<Example>>> GetAll()
        {
            var results = await _database.GetAll();
            _logger.LogDebug("Returning all results: {results}", results);
            return Ok(results);
        }

        [HttpGet]
        [Route("ByDate/{date}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ICollection<Example>>> GetForDate(DateOnly date)
        {
            var results = await _database.GetForDate(date);
            _logger.LogDebug("Returning all results for date {date}: {results}", date, results);
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
                _logger.LogWarning("No result found for {id}", id);
                return NotFound();
            }
            _logger.LogDebug("Returning result: {result}", result);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Example>> Create([FromBody] Example input)
        {
            //TODO: Is this needed?
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid input: {input}", input);
                return BadRequest(ModelState);
            }
            var existing = _database.GetForId(input.Id);
            if (existing != null)
            {
                return BadRequest($"There is alredy an item with id {input.Id}");
            }
            var result = await _database.Create(input);
            _logger.LogInformation("Added {result}", result);
            return CreatedAtAction(nameof(GetForId), result.Id, result);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Example>> Update([FromBody] Example input)
        {
            //TODO: Is this needed?
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid input: {input}", input);
                return BadRequest(ModelState);
            }
            var existing = _database.GetForId(input.Id);
            if (existing == null)
            {
                return NotFound();
            }
            var result = await _database.Update(input);
            _logger.LogInformation("Updated {existing} to {result}", existing, result);
            return Ok(result);
        }

        [HttpGet]
        [Route("options")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<ExampleOptions> GetOptions()
        {
            _logger.LogDebug("Returning all options: {options}", _options);
            return Ok(_options);
        }
    }
}
