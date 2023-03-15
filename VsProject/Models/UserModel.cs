using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using VsProject.Models.Repositories;

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

