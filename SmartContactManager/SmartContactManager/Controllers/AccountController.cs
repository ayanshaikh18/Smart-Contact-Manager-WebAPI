using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartContactManager.Data.RepositoryInterfaces;
using SmartContactManager.Models;
using SmartContactManager.Models.ViewModels;

namespace SmartContactManager.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        public AccountController(IAccountRepository _accountRepository)
        {
            this._accountRepository = _accountRepository;
        }

        // GET: api/Account
        public ActionResult<IEnumerable<User>> GetAllUsers()
        {
            return Ok(_accountRepository.GetAllUsers());
        }


        // GET: api/Account/5
        [HttpGet("{id}")]
        public ActionResult<User> GetUserById(int id)
        {
            User user = _accountRepository.FindUserById(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }


        // PUT: api/account/
        [HttpPut("{id}")]
        public ActionResult PutUser(int id, RegisterUser user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.Id)
            {
                return BadRequest();
            }

            User updatedUser = _accountRepository.FindUserById(id);
            if (updatedUser == null)
            {
                return Ok(new { status = 401, isSuccess = false, message = "User not found" });
            }

            updatedUser.Name = user.Name;
            updatedUser.PhoneNumber = user.PhoneNumber;

            try
            {
                updatedUser = _accountRepository.UpdateUser(updatedUser);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (_accountRepository.FindUserById(id) == null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok(new { status = 200, isSuccess = true, message = "User details modified" });
        }


        // POST: api/accoun/register
        [Route("register")]
        [HttpPost]
        public IActionResult Register(RegisterUser user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User newUser;
            try
            {
                newUser = _accountRepository.CreateUser(user);
            }
            catch (DbUpdateException Ex)
            {
                string message = "User already exist with given email";
                ModelState.AddModelError("Error", message);
                return BadRequest(ModelState);
            }
            return Ok(newUser);
        }

        // POST: api/account/Login
        [Route("login")]
        [HttpPost]
        public IActionResult Login(LoginUser user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_accountRepository.GetUserByUsernameAndPassword(user.Email,user.Password) == null)
            {
                /*string message = "Invalid Credentials";
                ModelState.AddModelError("Error", message);
                return BadRequest(ModelState);*/
                return Ok(new { status = 401, isSuccess = false, message = "Invalid User Credentials" });
            }

            User loggedUser = _accountRepository.GetUserByUsername(user.Email);
            return Ok(new { status = 200, isSuccess = true, message = "User Login successfully", UserDetails = loggedUser });
        }


        // PUT: api/account/resetPassword
        [Route("resetPassword")]
        [HttpPut]
        public IActionResult ResetPassword(ResetPassword model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User user = _accountRepository.FindUserById(model.UserId);
            if (user == null)
            {
                return Ok(new { status = 401, isSuccess = false, message = "User not found" });
            }

            if(user.Password != model.OldPassword)
            {
                return Ok(new { status = 401, isSuccess = false, message = "Invalid Old Password" });
            }

            user.Password = model.Password;

            try
            {
                _accountRepository.UpdateUser(user);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (_accountRepository.FindUserById(model.UserId) == null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok(new { status = 200, isSuccess = true, message = "Password is successfully changed" });
        }
    }
}
