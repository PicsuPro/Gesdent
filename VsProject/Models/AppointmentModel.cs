using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VsProject.Models
{
    public class AppointmentModel
    {
        public string? Subject { get; set; }
        public DateTime StartDateTime { get; set; }
        public TimeSpan Duration { get; set; }
    }
}
