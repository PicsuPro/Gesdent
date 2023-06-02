using System;

namespace VsProject.Models
{
    public class AppointmentModel
    {
        public string? Subject { get; set; }
        public DateTime StartDateTime { get; set; }
        public TimeSpan Duration { get; set; }
    }
}
