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
        public static readonly IUserRepository Repository = new UserRepository();
        public static readonly IStaffRepository StaffRepository = new StaffRepository();
        public static readonly IPatientRepository PatientRepository = new PatientRepository();
        public static bool Set(NetworkCredential credential)
        {
            bool isValidUser = Repository.AuthenticateUser(credential);
            if(isValidUser)
            {
                Current = Repository.GetByUsername(credential.UserName);
            }
            return isValidUser;
        }
        public static void Set(UserModel user)
        {
                Current = Repository.GetById((Guid)user.Id);
        }
    }

}

