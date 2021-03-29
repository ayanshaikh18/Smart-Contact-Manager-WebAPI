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
    [Route("api/contacts")]
    [ApiController]
    public class ContactsController : ControllerBase
    {

        private readonly IContactRepository _contactRepository;
        private readonly IAccountRepository _accountRepository;
        public ContactsController(IContactRepository _contactRepository, IAccountRepository _accountRepository)
        {
            this._accountRepository = _accountRepository;
            this._contactRepository = _contactRepository;
        }



        // GET: api/Contact
        public ActionResult<IEnumerable<Contact>> GetAllContacts()
        {
            IEnumerable<Contact> contacts = _contactRepository.GetAllContacts();
            foreach (var item in contacts)
            {
                item.User = _accountRepository.FindUserById(item.UserId);
            }
            return Ok(contacts);
        }



        // GET: api/Contact/5
        [HttpGet("{id}")]
        public ActionResult<Contact> GetContactById(int id)
        {
            Contact contact = _contactRepository.GetContactById(id);
            contact.User = _accountRepository.FindUserById(contact.UserId);
            if (contact == null)
            {
                return NotFound(new { message = "Contact not found" });
            }

            return Ok(contact);
        }



        // POST: api/accoun/register
        [HttpPost]
        public IActionResult CreateContact(CreateContact contact)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User user = _accountRepository.FindUserById((int)contact.UserId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            Contact newContact;
            try
            {
                newContact = _contactRepository.CreateContact(contact);
            }
            catch (DbUpdateException Ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Contact already exits");
            }

            return Ok(newContact);
        }



        // PUT: api/Contact/
        [HttpPut("{id}")]
        public ActionResult PutContact(int id, UpdateContact contact)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != contact.Id)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Invalid request");
            }

            Contact updatedContact = _contactRepository.GetContactById(id);
            if (updatedContact == null)
            {
                return NotFound("Contact not found");
            }
            User user = _accountRepository.FindUserById((int)contact.UserId);
            bool doesBelongsToUser = _contactRepository.DoesBelongsToUser(updatedContact, (int)contact.UserId);
            if (user == null || !doesBelongsToUser)
            {
                return Unauthorized("Access Denied");
            }

            updatedContact.Name = contact.Name;
            updatedContact.Email = contact.Email;
            updatedContact.Description = contact.Description;
            updatedContact.PhoneNumber = contact.PhoneNumber;

            try
            {
                updatedContact = _contactRepository.UpdateContact(updatedContact);
            }
            catch (Exception ex)
            {
                if (_contactRepository.GetContactById(id) == null)
                {
                    return NotFound("Contact not found");
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Contact already exist");
                }
            }
            return Ok(updatedContact);
        }



        // DELETE: api/contacts/5
        [HttpDelete("{id}")]
        public ActionResult DeleteContact(int id)
        {
            Contact contact = _contactRepository.GetContactById(id);
            if (contact == null)
            {
                return NotFound("Contact not found");
            }
            contact = _contactRepository.DeleteContact(contact);
            return Ok(contact);
        }

    }
}