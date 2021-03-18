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
    public class SQLContactRepository : IContactRepository
    {

        private readonly AppDbContext _context;

        public SQLContactRepository(AppDbContext _context)
        {
            this._context = _context;
        }

        public IEnumerable<Contact> GetAllContacts()
        {
            return _context.Contacts.ToList();
        }

        public Contact GetContactById(int id)
        {
            return _context.Contacts.Where(c => c.Id == id).FirstOrDefault();
        }

        public Contact CreateContact(CreateContact contact)
        {
            Contact newContact = new Contact()
            {
                Name = contact.Name,
                Email = contact.Email,
                Description = contact.Description,
                PhoneNumber = contact.PhoneNumber,
                UserId = (int)contact.UserId
            };
            _context.Contacts.Add(newContact);
            _context.SaveChanges();
            return newContact;
        }

        public Contact UpdateContact(Contact contact)
        {
            _context.Entry(contact).State = EntityState.Modified;
            _context.SaveChanges();
            return contact;
        }

        public Contact DeleteContact(Contact contact)
        {
            if (contact == null)
            {
                throw new ArgumentNullException(nameof(contact));
            }
            _context.Contacts.Remove(contact);
            _context.SaveChanges();
            return contact;
        }

        public bool DoesBelongsToUser(Contact contact, int userId)
        {
            return _context.Contacts.Where(c => c.UserId == userId).FirstOrDefault() != null ? true : false;
        }
    }
}
