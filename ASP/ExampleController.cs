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
            //TODO: Is this needed?
            if (!ModelState.IsValid)
            {
                _logger.LogWarning(104, "Invalid input: {input}", input);
                return BadRequest(ModelState);
            }
            var existing = _database.GetForId(input.Id);
            if (existing != null)
            {
                return BadRequest($"There is alredy an item with id {input.Id}");
            }
            var result = await _database.Create(input);
            _logger.LogInformation(104, "Added {result}", result);
            return CreatedAtAction(nameof(GetForId), result.Id, result);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Example>> Update([FromBody] Example input)
        {
            //TODO: Is this needed?
            if (!ModelState.IsValid)
            {
                _logger.LogWarning(105, "Invalid input: {input}", input);
                return BadRequest(ModelState);
            }
            var existing = _database.GetForId(input.Id);
            if (existing == null)
            {
                return NotFound();
            }
            var result = await _database.Update(input);
            _logger.LogInformation(105, "Updated {existing} to {result}", existing, result);
            return Ok(result);
        }

        [HttpGet]
        [Route("options")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<ExampleOptions> GetOptions()
        {
            _logger.LogDebug(106, "Returning all options: {options}", _options);
            return Ok(_options);
        }

        [HttpGet]
        [Route("log")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<ExampleOptions> Log()
        {
            //Using scopes you can add additional information to the log message
            using (_logger.BeginScope(new List<KeyValuePair<string, object>> {  new("TransactionId", "value")}))
            {
                //Events can be used to associates a set of log messages with a specific event
                _logger.LogInformation(107, "This is a test log message with scope2");
            }
            return Ok();
        }

    }
}
