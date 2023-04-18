﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VsProject.Models
{
    public class StaffModel
    {
        public Guid? UserId { get; set; }
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        public bool? Sex { get; set; }
        public string? Phone { get; set; }
        public string? PhoneAlt { get; set; }
        public string? Email { get; set; }
        public DateTime? BirthDate { get; set; }

    }
}
