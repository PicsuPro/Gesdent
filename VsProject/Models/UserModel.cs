using System;
using System.Net;
using VsProject.Repositories;

namespace VsProject.Models
{
    public class UserModel
    {
        public Guid? Id { get; set; }
        public string? UserName { get; set; }
        public string? Hash { get; set; }
        public string? Email { get; set; }

    }

    public static class UserPrincipal
    {
        public static UserModel? Current { get; private set; }
        public static readonly IUserRepository UserRepository = new UserRepository();
        public static readonly IStaffRepository StaffRepository = new StaffRepository();
        public static readonly IPatientRepository PatientRepository = new PatientRepository();
        public static readonly IPatientRecordRepository PatientRecordRepository = new PatientRecordRepository();
        public static readonly IToothRepository ToothRepository = new ToothRepository();
        public static readonly IAppointmentRepository AppointmentRepository = new AppointmentRepository();
        public static readonly IMedicationRepository MedicationRepository = new MedicationRepository();
        public static bool SetUser(NetworkCredential credential)
        {
            bool isValidUser = UserRepository.AuthenticateUser(credential);
            if (isValidUser)
            {
                Current = UserRepository.GetByUsername(credential.UserName);
            }
            return isValidUser;
        }
        public static void SetUser(UserModel user)
        {
            Current = UserRepository.GetById((Guid)user.Id);
        }
    }

}

