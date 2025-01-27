﻿using SmartContactManager.Data.RepositoryInterfaces;
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

        public IEnumerable<Group> GetAllGroups(int userId)
        {
            return db.Groups.Where(grp=>grp.UserId==userId).ToList();
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

        public void DeleteGroupContact(int id)
        {
            var grpContact = db.GroupContacts.Find(id);
            if (grpContact != null)
            {
                db.GroupContacts.Remove(grpContact);
                db.SaveChanges();
            }
        }
    }
}
