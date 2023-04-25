using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using VsProject.Repositories;

namespace VsProject.Models
{
    public class PatientModel : PersonModel
    {
        public int? Id { get; set; }
        public string? Profession { get; set; }
        public string? Adress { get; set; }
        public string? Motive { get; set; }
        public string? OrientedBy { get; set; }
        public string? PreferredDay { get; set; }
        public string? ParentName { get; set; }
        
        
    }
}
