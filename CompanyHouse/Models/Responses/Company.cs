using CompaniesHouse.Response;
using CompaniesHouse.Response.CompanyProfile;
using System;

namespace API.Models
{
    public class Company
    {

        public Company(CompanyProfile companyProfile)
        {
            RegistrationNumber = companyProfile.CompanyNumber;
            Name = companyProfile.CompanyName;
            CompanyAge = (companyProfile.DateOfCreation.HasValue? (DateTime.Now.Year - companyProfile.DateOfCreation.Value.Year) : 0);
            Address = new OfficeAddress(companyProfile.RegisteredOfficeAddress);
        }

        public Company(CompaniesHouse.Response.Search.CompanySearch.Company company)
        {
            RegistrationNumber = company.CompanyNumber;
            Name = company.Title;
            CompanyAge = (company.DateOfCreation.HasValue ? (DateTime.Now.Year - company.DateOfCreation.Value.Year) : 0);
            Address = new OfficeAddress(company.Address);
        }

        public string RegistrationNumber { get; set; }
        public string Name { get; set; }
        public int CompanyAge { get; set; }
        public OfficeAddress Address { get; set; }
    }

    public class OfficeAddress
    {
        public OfficeAddress(Address officeAddress)
        {
            PostalCode = officeAddress.PostalCode;
            Country = officeAddress.Country;
            Locality = officeAddress.Locality;
            Region = officeAddress.Region;
            AddressLine1 = officeAddress.AddressLine1;
            AddressLine2 = officeAddress.AddressLine2;
        }

        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Locality { get; set; }
        public string Region { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
    }
}
