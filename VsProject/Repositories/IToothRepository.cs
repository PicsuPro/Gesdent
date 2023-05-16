using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VsProject.Models;

namespace VsProject.Repositories
{
    public interface IToothRepository
    {
        void AddAll(IEnumerable<ToothModel> teethList, int? patientRecordId);
        void EditAll(IEnumerable<ToothModel> teethList, int? patientRecordId);

        ToothModel? GetByNumber(int? number, int patientRecordId);

        IEnumerable<ToothModel> GetAll(int patientRecordId);
        IEnumerable<IEnumerable<ToothModel>> GetAllFromHistory(int patientRecordId);

    }
}
