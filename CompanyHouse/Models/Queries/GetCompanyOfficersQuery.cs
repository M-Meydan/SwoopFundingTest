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
    public class GetCompanyOfficersQuery : IRequest<List<Officer>>
    {
        public string CompanyNumber { get; set; }
        public bool? Active { get;  set; }
        public int? Age { get;  set; }

        public GetCompanyOfficersQuery(string number, bool? active, int? age) { CompanyNumber = number; Age = age; }
    }

    public class GetCompanyOfficersQueryHandler : IRequestHandler<GetCompanyOfficersQuery, List<Officer>>
    {
        readonly int _numberOfResults;
        readonly ICompaniesHouseClient _client;

        public GetCompanyOfficersQueryHandler(ICompaniesHouseClient client, IConfiguration config) { _client = client; _numberOfResults = config.GetValue<int>("NumberOfResults"); }

        public async Task<List<Officer>> Handle(GetCompanyOfficersQuery query, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            var result = await _client.GetOfficersAsync(query.CompanyNumber, 1 , _numberOfResults);

            if (result.Data == null || result.Data.Items.Length == 0) throw new AppException($"No Officer found with name:{query.CompanyNumber}", (int)HttpStatusCode.NotFound);

            var companyList = result.Data.Items.Select(x => new Officer(x));
            return companyList.ToList();
        }
    }
}

   
