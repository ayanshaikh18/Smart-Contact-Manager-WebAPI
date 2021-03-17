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
        private readonly AppDbContext _context;
        public SQLAccountRepository(AppDbContext context)
        {
            this._context = context;
        }

        public User FindUserById(int id)
        {
            return _context.Users.Where(u => u.Id == id).FirstOrDefault();
        }

        public User GetUserByUsername(string username)
        {
            return _context.Users.Where(u => u.Email == username).FirstOrDefault();
        }

        public User GetUserByUsernameAndPassword(string username, string password)
        {
            return _context.Users.Where(u => u.Email == username && u.Password == password).FirstOrDefault();
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        public User UpdateUser(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            _context.SaveChanges();
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
            _context.Users.Add(newUser);
            _context.SaveChanges();
            return newUser;
        }
    }
}
