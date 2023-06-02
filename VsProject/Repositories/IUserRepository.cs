using System;
using System.Collections.Generic;
using System.Net;

namespace VsProject.Models
{
    public interface IUserRepository
    {
        bool AuthenticateUser(NetworkCredential credential);
        void Add(UserModel userModel);
        void Edit(UserModel userModel);
        void Remove(UserModel userModel);

        UserModel? GetById(Guid? id);
        UserModel? GetByUsername(string? username);

        IEnumerable<UserModel> GetAll();


    }
}
