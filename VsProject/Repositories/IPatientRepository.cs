using System.Collections.Generic;
using VsProject.Models;

namespace VsProject.Repositories
{
    public interface IPatientRepository
    {
        int Add(PatientModel patientModel);
        void Edit(PatientModel patientModel);

        PatientModel? GetById(int? id);

        IEnumerable<PatientModel> GetAll();
    }
}
