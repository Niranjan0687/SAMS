using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmsAPI.Contract;

namespace SmsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonV2Controller : ControllerBase
    {
        private readonly ILogger<PersonTwoController> _logger;
        private readonly IPersonService _personService;
    }
}
