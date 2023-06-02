using System.Collections.Generic;
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
