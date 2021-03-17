using SmartContactManager.Models;
using SmartContactManager.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartContactManager.Data.RepositoryInterfaces
{
    public interface IAccountRepository
    {
        IEnumerable<User> GetAllUsers();
        User GetUserByUsernameAndPassword(string username, string password);
        User GetUserByUsername(string username);
        User FindUserById(int id);
        User UpdateUser(User user);
        User CreateUser(RegisterUser user);
    }
}
