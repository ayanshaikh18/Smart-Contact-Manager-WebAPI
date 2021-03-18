using SmartContactManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartContactManager.Data.RepositoryInterfaces
{
    public interface IGroupRepository
    {
        Group AddGroup(Group group);
        Group GetGroupById(int id);
        IEnumerable<Group> GetAllGroups();
        void UpdateGroup(Group group);
        void DeleteGroup(Group group);

        void AddGroupContacts(IEnumerable<GroupContact> groupContacts);
        IEnumerable<GroupContact> GetGroupContactsByGroupId(int grpId);
    }
}
