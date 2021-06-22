using NUnit.Framework;
using FakeItEasy;
using System.Threading.Tasks;
using CompaniesHouse;
using API.Models.Queries;
using System.Threading;
using CompaniesHouse.Response.CompanyProfile;
using API.Exceptions;
using CompaniesHouse.Request;
using CompaniesHouse.Response.Search.CompanySearch;
using CompaniesHouse.Response;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System;

namespace CampanyHouseAPI.Test
{
    public class GetCompaniesByNameQueryTest
    {
        [SetUp]
        public void Setup()
        {
        }

        //[Test]
        //public async Task Company_returned_by_valid_number()
        //{
        //    var query = new GetCompaniesByNameQuery("Swoop",1);
        //    var searchRequest = new SearchRequest() { ItemsPerPage = 5, Query = query.CompanyName, StartIndex = 1 };

        //    var fakeClient = A.Fake<ICompaniesHouseClient>();
        //    //var fakeConfig = A.Fake<IConfiguration>();
        //    var companies = new Company[] {
        //                        new Company {Title = "SWOOP FINANCE LIMITED", CompanyNumber = "11163382", Address= new Address() },
        //                        new Company {Title = "SWOOP FINANCE LIMITED", CompanyNumber = "11163382", Address= new Address() },
        //                        new Company {Title = "SWOOP FINANCE LIMITED", CompanyNumber = "11163382", Address= new Address()}
        //    };

        //    var companySearch = new CompanySearch() { Companies = companies, ItemsPerPage = 5, StartIndex = 1, TotalResults = 10 , ETag="abc", Kind="abc", PageNumber=1};

        //    var config = new Dictionary<string, string>{ {"NumberOfResults", "5"}};

        //    var fakeConfig = new ConfigurationBuilder()
        //        .AddInMemoryCollection(config)
        //        .Build();

        //    //A.CallTo(() => fakeConfig.GetValue<int>("NumberOfResults")).Returns(5);
        //    //A.CallTo(()=> fakeClient.SearchCompanyAsync(searchRequest, default(CancellationToken)))
        //    //    .Returns(new CompaniesHouseClientResponse<CompanySearch>(companySearch));

        //    //act
        //    var actualResult = await new GetCompanyByNameQueryHandler(fakeClient, fakeConfig).Handle(query, default(CancellationToken));

        //    //assert
        //    //A.CallTo(() => fakeClient.GetCompanyProfileAsync(A<string>._, default(CancellationToken))).MustHaveHappenedOnceExactly();
        //    //Assert.AreEqual(query.CompanyNumber.ToString(), actualResult.RegistrationNumber);
        //}

        [Test]
        public void AppException_thrown_when_company_number_not_found()
        {
            var query = new GetCompaniesByNameQuery("Swoop", 1);
            var searchRequest = new SearchRequest() { ItemsPerPage = 5, Query = query.CompanyName, StartIndex = 1 };
            
            var fakeClient = A.Fake<ICompaniesHouseClient>();
            var config = new Dictionary<string, string> { { "NumberOfResults", "5" } };

            var fakeConfig = new ConfigurationBuilder()
                .AddInMemoryCollection(config)
                .Build();

            //assert
            Assert.ThrowsAsync<NullReferenceException>(async () => await new GetCompanyByNameQueryHandler(fakeClient, fakeConfig).Handle(query, default(CancellationToken)));
        }
    }
}