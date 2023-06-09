using System;
using System.Collections.Generic;
using VsProject.Models;

namespace VsProject.Repositories
{
    public interface IAppointmentRepository
    {
        int Add(AppointmentModel appointmentModel);
        void Edit(AppointmentModel appointmentModel);
        void Remove(AppointmentModel appointmentModel);
        AppointmentModel? GetByIdAndPatientId(int? id, int? patientId);
        IEnumerable<AppointmentModel> GetAll();
        IEnumerable<AppointmentModel>? GetAllByPatientId(int? patientId, DateOnly? minDate = null, DateOnly? maxDate = null);
    }
}
