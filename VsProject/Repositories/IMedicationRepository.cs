using System.Collections.Generic;
using VsProject.Models;

namespace VsProject.Repositories
{
    public interface IMedicationRepository
    {
        int Add(MedicationModel medicationModel);
        void Edit(MedicationModel medicationModel);

        MedicationModel? GetById(int? id);

        IEnumerable<MedicationModel> GetAll();
        void Remove(MedicationModel medicationModel);
    }
}
