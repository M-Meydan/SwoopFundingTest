using API.Exceptions;
using API.Models.Queries;
using CompaniesHouse;
using CompaniesHouse.Response.CompanyProfile;
using FakeItEasy;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;

namespace CampanyHouseAPI.Test
{
    public class GetCompaniesByNumberQueryTest
    { 

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task Company_returned_by_valid_number()
        {
            var query = new GetCompanyByNumberQuery(11163382);

            var fakeClient = A.Fake<ICompaniesHouseClient>();
            A.CallTo(()=> fakeClient.GetCompanyProfileAsync(query.CompanyNumber.ToString(), default(CancellationToken)))
                .Returns(new CompaniesHouseClientResponse<CompanyProfile>(new CompanyProfile
                {
                    CompanyName = "SWOOP FINANCE LIMITED",
                    CompanyNumber = "11163382",
                     RegisteredOfficeAddress = new CompaniesHouse.Response.Address()
                }));


            //act
            var actualResult = await new GetCompanyByNumberQueryHandler(fakeClient).Handle(query, default(CancellationToken));

            //assert
            A.CallTo(() => fakeClient.GetCompanyProfileAsync(A<string>._, default(CancellationToken))).MustHaveHappenedOnceExactly();
            Assert.AreEqual(query.CompanyNumber.ToString(), actualResult.RegistrationNumber);
        }

        [Test]
        public void AppException_thrown_when_company_number_not_found()
        {
            var query = new GetCompanyByNumberQuery(00000);

            var fakeClient = A.Fake<ICompaniesHouseClient>();
            A.CallTo(() => fakeClient.GetCompanyProfileAsync(query.CompanyNumber.ToString(), default(CancellationToken)))
                .ThrowsAsync(new AppException($"No Company found"));

            //assert
            Assert.ThrowsAsync<AppException>(async () => await new GetCompanyByNumberQueryHandler(fakeClient).Handle(query, default(CancellationToken)));
        }
    }
}