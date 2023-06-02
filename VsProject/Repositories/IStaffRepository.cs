using System;
using System.Collections.Generic;

namespace VsProject.Models
{
    public interface IStaffRepository
    {
        void Add(StaffModel staffModel);
        void Edit(StaffModel staffModel);

        StaffModel? GetByUserId(Guid? id);

        IEnumerable<StaffModel> GetAll();
    }
}
