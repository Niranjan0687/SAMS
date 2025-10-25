using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using SmsAPI.Contract;
using SmsAPI.DTOs;
using SmsAPI.Models;
using System.Text.Json;

namespace SmsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonTwoController : ControllerBase
    {
        private readonly ILogger<PersonTwoController> _logger;
        private readonly IPersonService _personService;
        private readonly IDistributedCache distributedCache;
        private const string PersonlistKey = "person:all";
        public PersonTwoController(IPersonService personservice, ILogger<PersonTwoController> logger, IDistributedCache cache)
        {
            _logger = logger;
            _personService = personservice;
            distributedCache = cache;
        }
        [HttpGet("getPersonbyId/{id}")]
        public async Task<IActionResult> GetPersonById(int id)
        {
            _logger.LogInformation("Fetching person with ID: {Id}", id);
            var person = await _personService.GetPersonById(id);
            return Ok(person);
        }
        [HttpGet("getAllPerson")]
        public async Task<IActionResult> GetAllPerson()
        {
            _logger.LogInformation("Fetching all persons");


            var persons = await _personService.GetPersonAll();

            return Ok(persons);
        }
        [HttpGet("getPersonCache")]
        public async Task<List<PersonDto>> GetPersonCacheTest()
        {
            _logger.LogInformation("Fetching all persons");
            var cached = await distributedCache.GetStringAsync(PersonlistKey);
            if (cached != null)
            {
                return JsonSerializer.Deserialize<List<PersonDto>>(cached)!;
            }
            var persons = await _personService.GetPersonAllCache();
            var option = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(6),
                SlidingExpiration = TimeSpan.FromHours(30),
            };
            var peoplePayload = JsonSerializer.Serialize(persons);
            await distributedCache.SetStringAsync(PersonlistKey, peoplePayload, option);
            return persons;
        }
        [HttpPut("UpdatePerson")]
        public async Task UpdatePersonData(Person person)
        {
            await _personService.UpdatePerson(person);
            await distributedCache.RemoveAsync(PersonlistKey);
        }

    }
}
