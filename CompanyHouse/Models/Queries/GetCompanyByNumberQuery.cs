using API.Exceptions;
using CompaniesHouse;
using MediatR;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace API.Models.Queries
{
    public class GetCompanyByNumberQuery : IRequest<Company>
    {
        public int CompanyNumber { get; set; }
        public GetCompanyByNumberQuery(int number) { CompanyNumber = number; }
    }

    public class GetCompanyByNumberQueryHandler : IRequestHandler<GetCompanyByNumberQuery, Company>
    {
        readonly ICompaniesHouseClient _client;

        public GetCompanyByNumberQueryHandler(ICompaniesHouseClient client) { _client = client; }

        public async Task<Company> Handle(GetCompanyByNumberQuery query, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var result = await _client.GetCompanyProfileAsync(query.CompanyNumber.ToString());

            if (result.Data == null) throw new AppException($"No Company found with id:{query.CompanyNumber}", (int)HttpStatusCode.NotFound);

            return new Company(result.Data);
        }
    }
}

   
