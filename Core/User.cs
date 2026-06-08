using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Core
{
    public class User
    {
        public User()
        {
        }
        public User(string nameidentifier, string name, string surname, string email, DateOnly date,
            string phone ="00", string streetAddress="", string neighborhood="", string city="", 
            string postalCode= "00000")
        {
            NameIdentifier = nameidentifier;
            Name = name;
            Surname = surname;
            Email = email;
            DateOfBirth = date;
            Phone = phone;
            Role = "Client";
            LockoutEnd =  new DateTimeOffset();
            StreetAddress = streetAddress;
            Neighborhood = neighborhood;
            City = city;
            PostalCode = postalCode;
        }

        [Key]
        public string? NameIdentifier { get; set; }

        public string? Name { get; set; }

        public string? Surname { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }       

        public DateOnly DateOfBirth { get; set; }

        public string? Role { get; set; }

        public DateTimeOffset LockoutEnd { get; set; }

        public virtual IEnumerable<OrderHeader>? OrderHeaders { get; set; } = [];

        public string? StreetAddress { get; set; }
        public string? Neighborhood { get; set; }
        public string? City { get; set; }
        public string? PostalCode { get; set; }

    }
}
