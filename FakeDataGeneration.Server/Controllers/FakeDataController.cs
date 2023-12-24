using FakeDataGeneration.Server.Models;
using FakeDataGeneration.Server.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FakeDataGeneration.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FakeDataController : ControllerBase
    {
        private readonly IFakeDataService _fakeDataService;

        public FakeDataController(IFakeDataService fakeDataService)
        {
            _fakeDataService = fakeDataService;
        }

        [HttpGet]
        public IActionResult GetData(
            [FromQuery] Region region = Region.Russia,
            [FromQuery] int seed = 0,
            [FromQuery] int pageNumber = 0,
            [FromQuery] double errorsAmount = 0,
            [FromQuery] int perPage = 10)
        {
            var fakeDataRequest = new FakeDataRequest
            {
                Region = region,
                Seed = seed,
                ErrorsAmount = errorsAmount,
                PageNumber = pageNumber,
                PerPage = perPage
            };

            return Ok(_fakeDataService.GetFakeUsers(fakeDataRequest));
        }
    }
}
