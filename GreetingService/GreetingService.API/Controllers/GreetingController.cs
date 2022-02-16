using Microsoft.AspNetCore.Mvc;
using GreetingService.Core.Entities;
using GreetingService.Infrastructure;
using GreetingService.API.Authentication;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GreetingService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[BasicAuth]
    public class GreetingController : ControllerBase
    {
        private readonly IGreetingRepository _greetingRepository;

        public GreetingController(IGreetingRepository greetingRepository)
        {
            _greetingRepository = greetingRepository;
        }

        // GET: api/<GreetingController>
        [HttpGet]
        [BasicAuth]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Greeting>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAsync()
        {
            try
            {
                var responseResult = await _greetingRepository.GetAsync();
                return Ok(responseResult);
            }
            catch
            {
                return NotFound();
            }
        }

        // GET api/<GreetingController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Greeting))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            try
            {
                var responseResult = await _greetingRepository.GetAsync(id);
                return Ok(responseResult);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            return NotFound();
        }

        // POST api/<GreetingController>
        [HttpPost]
        public async Task PostAsync([FromBody] Greeting greeting)
        {
            await _greetingRepository.CreateAsync(greeting);
            Console.WriteLine(greeting.Message);
        }

        // PUT api/<GreetingController>/5
        [HttpPut]
        public async Task PutAsync([FromBody] Greeting greeting)
        {
            await _greetingRepository.UpdateAsync(greeting);
            Console.WriteLine(greeting.Message);
        }

        
    }
}
