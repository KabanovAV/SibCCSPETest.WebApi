using Microsoft.AspNetCore.Mvc;
using SibCCSPETest.Data;
using SibCCSPETest.ServiceBase;

namespace SibCCSPETest.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GroupController(IRepoServiceManager service) : ControllerBase
    {
        private readonly IRepoServiceManager _service = service;

        [HttpGet]
        public ActionResult<IEnumerable<Group>> GetAll()
        {
            var groups = _service.GroupRepository.GetAllGroup();
            return Ok(groups);
        }

        [HttpGet]
        public ActionResult<Group> Get([FromQuery] int id)
        {
            var group = _service.GroupRepository.GetGroup(g => g.Id == id);
            if (group == null)
                return NotFound(new { Message = $"Группа с id {id} не найдена." });
            return Ok(group);
        }

        [HttpPost]
        public ActionResult<Group> Add([FromBody] Group group)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            _service.GroupRepository.AddGroup(group);
            return CreatedAtAction(nameof(Get), new { id = group.Id }, group);
        }

        [HttpPut]
        public IActionResult Update([FromBody] Group group)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            _service.GroupRepository.UpdateGroup(group);
            return NoContent();
        }

        [HttpDelete]
        public IActionResult Delete([FromQuery] int id)
        {
            var group = _service.GroupRepository.GetGroup(g => g.Id == id);
            if (group == null)
                return NotFound(new { Message = $"Группа с id {id} не найдена." });
            _service.GroupRepository.DeleteGroup(group);
            return NoContent();
        }
    }
}
