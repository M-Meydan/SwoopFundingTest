using API.Exceptions;
using CompaniesHouse;
using CompaniesHouse.Request;
using MediatR;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace API.Models.Queries
{
    public class GetCompanyByNameAndOfficerQuery : IRequest<PaginatedList<Company>>
    {
        public string CompanyName { get; set; }
        public string Officer { get; set; }
        public int? StartIndex { get; internal set; }

        public GetCompanyByNameAndOfficerQuery(string name,string officer) { CompanyName = name; Officer = officer; }
    }

    public class GetCompanyByNameAndOfficerQueryHandler : IRequestHandler<GetCompanyByNameAndOfficerQuery, PaginatedList<Company>>
    {
        readonly int _numberOfResults;
        readonly ICompaniesHouseClient _client;

        public GetCompanyByNameAndOfficerQueryHandler(ICompaniesHouseClient client, IConfiguration config) { _client = client; _numberOfResults = config.GetValue<int>("NumberOfResults"); }

        public async Task<PaginatedList<Company>> Handle(GetCompanyByNameAndOfficerQuery query, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var searchRequest = new SearchRequest() { ItemsPerPage = _numberOfResults, Query = query.CompanyName, StartIndex = query.StartIndex };
            var result = await _client.SearchCompanyAsync(searchRequest);

            if (result.Data == null || result.Data.Companies.Length == 0) throw new AppException($"No Company found with name:{query.CompanyName}", (int)HttpStatusCode.NotFound);

            var companyList = result.Data.Companies.Select(x => new Company(x));
            return new PaginatedList<Company>(companyList.ToList(), result.Data.TotalResults.Value, result.Data.PageNumber.Value, result.Data.ItemsPerPage.Value);
        }
    }
}

   
