using System.Collections.ObjectModel;

namespace VsProject.Models
{
    public class PatientRecordModel
    {
        public int? PatientId { get; set; }
        public ObservableCollection<string>? Problems { get; set; }
        public string? Diagnostic { get; set; }
        public string? TreatmentPlan { get; set; }
        public string? Notes { get; set; }
    }
}
