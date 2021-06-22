using API.Filters;
using API.Models;
using API.Models.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyHouseController : ControllerBase
    {
        readonly ILogger<CompanyHouseController> _logger;
        readonly IMediator _mediator;
        public CompanyHouseController(ILogger<CompanyHouseController> logger, IMediator mediator)//, ITestableCache testableCache
        {
            _logger = logger;
            _mediator = mediator;

            //_testableCache = testableCache;
        }

        [HttpGet("{number:int}"), Produces("application/json")]
        [ServiceFilter(typeof(CacheResourceFilter))]
        public async Task<Company> GetCompany(int number)
        {
            return await _mediator.Send(new GetCompanyByNumberQuery(number));
        }

        [HttpGet("{name}/{page:int?}"), Produces("application/json")]
        [ServiceFilter(typeof(CacheResourceFilter))]
        public async Task<PaginatedList<Company>> GetCompanies(string name,int page = 1)
        {
            return await _mediator.Send(new GetCompaniesByNameQuery(name, page));
        }

        [HttpGet("{name}/{officer}"), Produces("application/json")]
        [ServiceFilter(typeof(CacheResourceFilter))]
        public async Task<PaginatedList<Company>> GetCompanies(string name, string officer)
        {
            return await _mediator.Send(new GetCompanyByNameAndOfficerQuery(name,officer));
        }

        [HttpGet("{number}/{active?}/{age?}"), Produces("application/json")]
        public async Task<IEnumerable<Officer>> GetCompanyOfficersAsync(string number, bool? active, int? age)
        {
            return await _mediator.Send(new GetCompanyOfficersQuery(number, active, age));
        }
    }
}
