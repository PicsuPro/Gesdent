using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
