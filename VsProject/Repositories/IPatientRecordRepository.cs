using System.Collections.Generic;
using VsProject.Models;

namespace VsProject.Repositories
{
    public interface IPatientRecordRepository
    {
        void Add(PatientRecordModel patientRecordModel);
        void Edit(PatientRecordModel patientRecordModel);

        PatientRecordModel? GetByPatientId(int? patientId);

        IEnumerable<PatientRecordModel> GetAllFromHistory(int? patientId);
    }
}
