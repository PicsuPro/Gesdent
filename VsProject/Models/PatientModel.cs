using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using VsProject.Models.Repositories;

namespace VsProject.Models
{
    public class PatientModel
    {
        public int? Id { get; set; }
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        public string? Surname { get; set; }
        public bool? Sex { get; set; }
        public string? Phone { get; set; }
        public string? PhoneAlt { get; set; }
        public string? Email { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Profession { get; set; }
        public string? Adress { get; set; }
        public string? Motive { get; set; }
        public string? OrientedBy { get; set; }
        public string? PreferredDay { get; set; }
        public string? ParentName { get; set; }
        
        
    }
}
