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
