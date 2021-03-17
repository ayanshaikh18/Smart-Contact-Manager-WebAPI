using SmartContactManager.Models;
using SmartContactManager.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartContactManager.Data.RepositoryInterfaces
{
    public interface IContactRepository
    {
        IEnumerable<Contact> GetAllContacts();
/*        IEnumerable<Contact> GetAllContactsByUserId(int id);*/
        Contact GetContactById(int id);
        Contact UpdateContact(Contact contact);
        Contact CreateContact(CreateContact contact);
        Contact DeleteContact(Contact contact);
        bool DoesBelongsToUser(Contact contact, int userId);
    }
}
