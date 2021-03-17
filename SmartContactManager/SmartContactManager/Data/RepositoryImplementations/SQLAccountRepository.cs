using Microsoft.EntityFrameworkCore;
using SmartContactManager.Data.RepositoryInterfaces;
using SmartContactManager.Models;
using SmartContactManager.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartContactManager.Data.RepositoryImplementations
{

    public class SQLAccountRepository: IAccountRepository
    {
        private readonly AppDbContext context;
        public SQLAccountRepository(AppDbContext context)
        {
            this.context = context;
        }

        public User FindUserById(int id)
        {
            return context.Users.Where(u => u.Id == id).FirstOrDefault();
        }

        public User GetUserByUsername(string username)
        {
            return context.Users.Where(u => u.Email == username).FirstOrDefault();
        }

        public User GetUserByUsernameAndPassword(string username, string password)
        {
            return context.Users.Where(u => u.Email == username && u.Password == password).FirstOrDefault();
        }

        public IEnumerable<User> GetAllUsers()
        {
            return context.Users.ToList();
        }

        public User UpdateUser(User user)
        {
            context.Entry(user).State = EntityState.Modified;
            context.SaveChanges();
            return user;
        }

        public User CreateUser(RegisterUser user)
        {
            User newUser = new User()
            {
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                PhoneNumber = user.PhoneNumber,
            };
            context.Users.Add(newUser);
            context.SaveChanges();
            return newUser;
        }
    }
}
