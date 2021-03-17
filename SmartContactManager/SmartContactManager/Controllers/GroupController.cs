using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartContactManager.Data.RepositoryInterfaces;
using SmartContactManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartContactManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly IGroupRepository groupRepository;

        public GroupController(IGroupRepository groupRepository)
        {
            this.groupRepository = groupRepository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Group>> GetAllgroups()
        {
            var groups = groupRepository.GetAllGroups();
            return Ok(groups);
        }

        [HttpGet("{id}")]
        public ActionResult<Group> GetGroupById(int id)
        {
            var grp = groupRepository.GetGroupById(id);
            if (grp != null)
                return Ok(grp);
            return NotFound();
        }

        [HttpPost]
        public ActionResult CreateGroup(Group group)
        {
            if (ModelState.IsValid)
            {
                groupRepository.AddGroup(group);
                return Ok();
            }
            return BadRequest();
        }
    }
}
