using System;

namespace VsProject.Models
{
    public class AppointmentModel
    {
        public string? Subject { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
    }
}
