using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SibCCSPETest.Data;
using SibCCSPETest.ServiceBase;

namespace SibCCSPETest.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupsController(IRepoServiceManager service, IMapper mapper) : ControllerBase
    {
        private readonly IRepoServiceManager _service = service;
        private readonly IMapper _mapper = mapper;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GroupDTO>>> GetAll()
        {
            var groups = await _service.GroupRepository.GetAllGroupAsync(includeProperties: "Specialization");
            var groupDTOs = _mapper.Map<IEnumerable<GroupDTO>>(groups);
            return Ok(groupDTOs);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<GroupDTO>> Get(int id)
        {
            var group = await _service.GroupRepository.GetGroupAsync(g => g.Id == id);
            if (group == null)
                return NotFound(new { Message = $"Группа с id {id} не найдена." });
            var groupDTO = _mapper.Map<GroupDTO>(group);
            return Ok(groupDTO);
        }

        [HttpPost]
        public async Task<ActionResult<GroupDTO>> Add([FromBody] GroupCreateDTO groupCreateDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var group = _mapper.Map<Group>(groupCreateDTO);
            await _service.GroupRepository.AddGroupAsync(group);
            return CreatedAtAction(nameof(Get), new { id = group.Id }, group);
        }

        [HttpPut]
        public IActionResult Update([FromBody] GroupDTO groupDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var group = _mapper.Map<Group>(groupDTO);
            _service.GroupRepository.UpdateGroup(group);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var group = await _service.GroupRepository.GetGroupAsync(g => g.Id == id);
            if (group == null)
                return NotFound(new { Message = $"Группа с id {id} не найдена." });
            _service.GroupRepository.DeleteGroup(group);
            return NoContent();
        }
    }
}
