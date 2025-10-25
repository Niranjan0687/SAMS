using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using SmsAPI.Contract;
using SmsAPI.Data;
using SmsAPI.Models;
using System.Text.Json;

namespace SmsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly AdmissionDbContext _context;
        private readonly IMemoryCache _memorycache;
        private readonly IPersonService _personService;

        public PersonController(AdmissionDbContext context,IMemoryCache memorycache,IPersonService personservice)
        {
            _context = context;
            _memorycache = memorycache;
            _personService = personservice;
        }
        //public static string da()
        //{
        //    var x = 42;  // Magic number
        //    string y = null;
        //    try
        //    {
        //        var z = (string)("Hello from PersonController");  // Redundant cast
        //        if (true == true)  // Redundant condition
        //        {
        //            y = z;
        //        }
        //        else
        //        {
        //            y = z.ToString();  // Redundant ToString
        //        }
        //    }
        //    catch  // Empty catch block
        //    {
        //    }
        //    finally
        //    {
        //        var unusedVar = "unused";  // Unused variable
        //    }
        //    return (string)y;  // Unnecessary cast
        //}
        
        [HttpGet("GetPersonHnd")]
        
        public IActionResult GetPerson()
        {
            const string cacheKey = "ChPersonList";

            if (!_memorycache.TryGetValue(cacheKey, out List<Person> personlist))
            {
                personlist = _context.person.Where(s => s.Title != null).ToList();
               
                var cacheOption=new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)).SetSlidingExpiration(TimeSpan.FromMinutes(2));
                _memorycache.Set(cacheKey, personlist,cacheOption);

            }
            else
            {
                Console.WriteLine("Cache hit — returning data from memory...");
            }
            
           
            return Ok(personlist);
        }
        [HttpGet("ClearCache")]
        public IActionResult clearcache()
        {
            _memorycache.Remove("ChPersonList");
            return Ok("Cache cleared successfully");
        }


        [HttpGet("PersonbyId/{id}")]
   
        public IActionResult GetPersonById(int id)
        {
            var person = _context.person.Find(id);
            if (person == null)
            {
                return NotFound("Person not found");
            }
            return Ok(person);
        }
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var personlist = _context.person.ToList();
            return Ok(personlist);
        }
        [HttpGet("personorderby")]
        public IActionResult GetPersonOrderBy()
        {
            var personlist = _context.person.Where(s => s.Title != null && s.Suffix != null).OrderByDescending(s => s.BusinessEntityID).ToList();
            return Ok(personlist);
        }
        [HttpGet("gettop100person")]
        public IActionResult getTopPerson()
        {
            var personlist = _context.person.Where(s => s.Title != null).Take(10).ToList();
            return Ok(personlist);
        }
        [HttpGet("getmultipleorder")]
        public IActionResult getMultipleOrderpeople()
        {
            var personalist = _context.person.Where(s => s.Title != null).OrderBy(k => k.FirstName).ThenBy(s => s.LastName).ThenByDescending(s => s.BusinessEntityID);
            return Ok(personalist);
        }
        [HttpGet("getSpecificColumn")]
        public IActionResult getSpecificColumn()
        {
            var personalist = _context.person.Where(s => s.Title != null).Select(s => new { s.FirstName, s.LastName, s.MiddleName, s.BusinessEntityID });
            return Ok(personalist);
        }
        [HttpGet("maptoPerson")]
        public IActionResult getSpecificRow()
        {
            var personalist = _context.person.Where(s => s.Title != null).Select(s => new Person
            {
                BusinessEntityID = s.BusinessEntityID,
                FirstName = s.FirstName,
                LastName = s.LastName,
                MiddleName = s.MiddleName
            });
            return Ok(personalist);
        }
        [HttpGet("PeopleJoin")]
        public IActionResult getPeaplePhone()
        {
            var result = from s in _context.person
                         join d in _context.PersonPhones on s.BusinessEntityID equals d.BusinessEntityID
                         select new
                         {
                             s.FirstName,
                             d.PhoneNumber,
                             s.BusinessEntityID
                         };
            return Ok(result);
        }
        [HttpGet("GetAllPerson")]
        public async Task< IActionResult> GetPersonAll(int page=1,int pagesize=100)
        {
            var personlist = await _context.person.Skip((page - 1) * pagesize).Take(pagesize).ToListAsync();
            return Ok(personlist);
        }
        [HttpGet("GetAllPersonChunk")]
        public async IAsyncEnumerable<Person> GetPersonChunk()
        {
            await foreach(var person in _context.person.AsNoTracking().AsAsyncEnumerable())
            {
                yield return person;
            }
        }
        [HttpGet("GetAllPersonChunkBodyWriter")]
        public async Task GetPersonChunkBodyWriter()
        {
            Response.ContentType = "application/json";
            await using var writer = new Utf8JsonWriter(Response.BodyWriter);
            writer.WriteStartArray();
            await foreach (var person in _context.person.AsNoTracking().AsAsyncEnumerable())
            {
                JsonSerializer.Serialize(writer, person);
                await writer.FlushAsync();
            }
            writer.WriteEndArray();
            await writer.FlushAsync();
        }
    }
}
