using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace VsProject.Models
{
    public interface IStaffRepository
    {
        void Add(StaffModel staffModel);
        void Edit(StaffModel staffModel);

        StaffModel? GetById(Guid? id);

        IEnumerable<StaffModel> GetAll();
    }
}
