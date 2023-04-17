using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using VsProject.Models.Repositories;

namespace VsProject.Models
{
    public class PatientModel : StaffModel
    {
        public string? Profession { get; set; }
        public string? Adresse { get; set; }
        public string? Pattern { get; set; }
        public string? PreferredDay { get; set; }
        public string? ParentName { get; set; }
        
        
    }
}
