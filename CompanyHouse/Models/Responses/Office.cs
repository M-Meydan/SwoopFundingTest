using System;

namespace API.Models
{
    public class Officer
    {
        public Officer(CompaniesHouse.Response.Officers.Officer officer) {
            Name = officer.Name;
            DateOfBirth = $"{officer.DateOfBirth.Month}/{officer.DateOfBirth.Year}";
            Age = (DateTime.Now.Year - DateTime.Parse(DateOfBirth).Year);
            Role = officer.OfficerRole.ToString();
        }
        public string Name { get; set; }
        public string DateOfBirth { get; set; }
        public int Age{ get; set; }
        public string Role { get; set; }
    }
}
