using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VsProject.Models
{
    public class StaffModel : PersonModel
    {
        public Guid? UserId { get; set; }

    }
}
