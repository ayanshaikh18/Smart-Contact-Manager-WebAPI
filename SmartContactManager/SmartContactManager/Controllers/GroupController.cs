using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartContactManager.Data.RepositoryInterfaces;
using SmartContactManager.Models;
using SmartContactManager.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartContactManager.Controllers
{
    [Route("api/groups")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IContactRepository _contactRepository;

        public GroupController(IGroupRepository groupRepository, IAccountRepository accountRepository,IContactRepository contactRepository)
        {
            this._groupRepository = groupRepository;
            this._accountRepository = accountRepository;
            this._contactRepository = contactRepository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Group>> GetAllgroups()
        {
            var groups = _groupRepository.GetAllGroups();
            return Ok(groups);
        }

        [HttpGet("{id}")]
        public ActionResult<Group> GetGroupById(int id)
        {
            var grp = _groupRepository.GetGroupById(id);
            if (grp != null)
                return Ok(grp);
            return NotFound();
        }

        [HttpPost]
        public ActionResult CreateGroup(Group group)
        {
            if (group != null && ModelState.IsValid)
            {
                var user = _accountRepository.FindUserById(group.UserId ?? 0);
                if (user == null)
                    return StatusCode(401,(new { status = 401, isSuccess = false, message = "User not found" }));

                Group grp;
                try
                {
                    grp = _groupRepository.AddGroup(group);
                }
                catch (DbUpdateException Ex)
                {
                    ModelState.AddModelError("Error", "Group already exists with the given name");
                    return BadRequest(ModelState);
                }
                return Ok();
            }
            return BadRequest(ModelState);
        }

        [HttpPut]
        public ActionResult EditGroup(Group group)
        {
            if (group != null && ModelState.IsValid)
            {
                var grp = _groupRepository.GetGroupById(group.Id);
                if (grp == null)
                    return StatusCode(401,(new { status = 401, isSuccess = false, message = "Group not found" }));

                if (grp.UserId != group.UserId)
                    return StatusCode(401,(new { status = 401, isSuccess = false, message = "Access Denied" }));

                grp.Id = group.Id;
                grp.Name = group.Name;
                grp.UserId = group.UserId;
                grp.Description = group.Description;
                try
                {
                    _groupRepository.UpdateGroup(grp);
                }
                catch (Exception Ex)
                {
                    ModelState.AddModelError("Error", "Group already exists with the given name");
                    return BadRequest(ModelState);
                }
                return Ok();
            }
            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteGroup(int id)
        {
            var grp = _groupRepository.GetGroupById(id);
            if (grp != null)
            {
                _groupRepository.DeleteGroup(grp);
                return Ok();
            }
            return BadRequest(new { Error="Group Not Found" });
        }

        [HttpPost]
        [Route("/api/groups/addGroupContacts")]
        public ActionResult AddGroupContacts(AddGroupContactsViewModel model)
        {
            if (model != null && ModelState.IsValid)
            {
                var grp = _groupRepository.GetGroupById(model.GroupId??0);

                if(grp == null)
                    return StatusCode(401, (new { status = 401, isSuccess = false, message = "Group not found" }));

                IList<GroupContact> groupContacts = new List<GroupContact>();
                foreach (var contactId in model.ContactIds)
                {
                    var groupContact = new GroupContact()
                    {
                        GroupId = model.GroupId,
                        ContactId = contactId
                    };
                    groupContacts.Add(groupContact);
                }
                _groupRepository.AddGroupContacts(groupContacts);
                return Ok();
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("/api/groups/getGroupContacts/{groupId}")]
        public ActionResult<GroupContactViewModel> GetGroupContacts(int groupId)
        {
            var groupContacts = _groupRepository.GetGroupContactsByGroupId(groupId);
            IList<GroupContactViewModel> grpContacts = new List<GroupContactViewModel>();
            foreach(var grpContact in groupContacts)
            {
                var model = new GroupContactViewModel();
                model.Contact= _contactRepository.GetContactById(grpContact.ContactId??0);
                model.Id = grpContact.Id;
                grpContacts.Add(model);
            }
            return Ok(grpContacts);
        }

        [HttpDelete]
        [Route("/api/groups/deleteGroupContact/{id}")]
        public ActionResult DeleteGroupContact(int id)
        {
            _groupRepository.DeleteGroupContact(id);
            return Ok();
        }
    }
}
