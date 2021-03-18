using SmartContactManager.Data.RepositoryInterfaces;
using SmartContactManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartContactManager.Data.RepositoryImplementations
{
    public class SQLGroupRepository : IGroupRepository
    {
        private readonly AppDbContext db;
        public SQLGroupRepository(AppDbContext db)
        {
            this.db = db;
        }
        public Group AddGroup(Group group)
        {
            db.Groups.Add(group);
            db.SaveChanges();
            return group;
        }

        public void AddGroupContacts(IEnumerable<GroupContact> groupContacts)
        {
            db.GroupContacts.AddRange(groupContacts);
            db.SaveChanges();
        }

        public void DeleteGroup(Group group)
        {
            db.Groups.Remove(group);
            db.SaveChanges();
        }

        public IEnumerable<Group> GetAllGroups()
        {
            return db.Groups.ToList();
        }

        public Group GetGroupById(int id)
        {
            var group = db.Groups.Find(id);
            return group;
        }

        public IEnumerable<GroupContact> GetGroupContactsByGroupId(int grpId)
        {
            return db.GroupContacts.Where(gc => gc.GroupId == grpId).ToList();
        }

        public void UpdateGroup(Group group)
        {
            var grp = db.Groups.Attach(group);
            grp.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            db.SaveChanges();
        }
    }
}
