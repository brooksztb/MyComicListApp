using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyComicListApi.Models;

namespace MyComicListApi.Services
{
    public interface IApplicationUserRepository
    {
        bool ValidateCredentials(string Username, string Password);
        Task<string> RegisterNewUser(string Username, string Password, string Email, string Firstname, string Lastname);

    }
}
