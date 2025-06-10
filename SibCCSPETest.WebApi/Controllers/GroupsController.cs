using Microsoft.AspNetCore.Mvc;
using SibCCSPETest.Data;
using SibCCSPETest.ServiceBase;

namespace SibCCSPETest.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GroupsController(IRepoServiceManager service) : ControllerBase
    {
        private readonly IRepoServiceManager _service = service;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Group>>> GetAll()
        {
            var groups = await _service.GroupRepository.GetAllGroupAsync();
            return Ok(groups);
        }

        [HttpGet]
        public async Task<ActionResult<Group>> Get([FromQuery] int id)
        {
            var group = await _service.GroupRepository.GetGroupAsync(g => g.Id == id);
            if (group == null)
                return NotFound(new { Message = $"Группа с id {id} не найдена." });
            return Ok(group);
        }

        [HttpPost]
        public async Task<ActionResult<Group>> Add([FromBody] Group group)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            await _service.GroupRepository.AddGroupAsync(group);
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
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            var group = await _service.GroupRepository.GetGroupAsync(g => g.Id == id);
            if (group == null)
                return NotFound(new { Message = $"Группа с id {id} не найдена." });
            _service.GroupRepository.DeleteGroup(group);
            return NoContent();
        }
    }
}
