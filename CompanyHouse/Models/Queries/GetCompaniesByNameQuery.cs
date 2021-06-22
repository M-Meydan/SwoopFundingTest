using API.Exceptions;
using CompaniesHouse;
using CompaniesHouse.Request;
using MediatR;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace API.Models.Queries
{
    public class GetCompaniesByNameQuery : IRequest<PaginatedList<Company>>
    {
        public string CompanyName { get; set; }
        public int? PageNumber { get; internal set; }

        public GetCompaniesByNameQuery(string name, int? page) { CompanyName = name; PageNumber = page; }
    }

    public class GetCompanyByNameQueryHandler : IRequestHandler<GetCompaniesByNameQuery, PaginatedList<Company>>//IRequestHandler<GetWeatherForecastsQuery, IEnumerable<WeatherForecast>>
    {
        readonly int _numberOfResults;
        readonly ICompaniesHouseClient _client;
        
        public GetCompanyByNameQueryHandler(ICompaniesHouseClient client, IConfiguration config) { _client = client; _numberOfResults = config.GetValue<int>("NumberOfResults"); }

        public async Task<PaginatedList<Company>> Handle(GetCompaniesByNameQuery query, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var nextItemIndex = _numberOfResults * (query.PageNumber==0?1: query.PageNumber-1);

            var searchRequest = new SearchRequest() { ItemsPerPage = _numberOfResults, Query = query.CompanyName, StartIndex = nextItemIndex };
            var result = await _client.SearchCompanyAsync(searchRequest);
            
            if (result.Data == null || result.Data.Companies.Length==0) throw new AppException($"No Company found with name:{query.CompanyName}", (int)HttpStatusCode.NotFound);

            var companyList = result.Data.Companies.Select(x => new Company(x));
            return new PaginatedList<Company>(companyList.ToList(), result.Data.TotalResults.Value, result.Data.PageNumber.Value, result.Data.ItemsPerPage.Value);
        }
    }
}

   
