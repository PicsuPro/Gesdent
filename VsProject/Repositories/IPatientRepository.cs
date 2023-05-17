using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using VsProject.Models;

namespace VsProject.Repositories
{
    public interface IPatientRepository
    {
        int Add(PatientModel patientModel);
        void Edit(PatientModel patientModel);

        PatientModel? GetById(int? id);

        IEnumerable<PatientModel> GetAll();
        void Remove(PatientModel patient);
    }
}
