using System;

namespace VsProject.Models
{
    public class PersonModel
    {
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        public string? Surname { get; set; }
        public bool? Sex { get; set; }
        public string? Phone { get; set; }
        public string? PhoneAlt { get; set; }
        public string? Email { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
