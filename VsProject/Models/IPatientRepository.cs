using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace VsProject.Models
{
    public interface IPatientRepository
    {
        void Add(PatientModel patientModel);
        void Edit(PatientModel patientModel);

        PatientModel? GetById(Guid? id);

        IEnumerable<PatientModel> GetAll();
    }
}
