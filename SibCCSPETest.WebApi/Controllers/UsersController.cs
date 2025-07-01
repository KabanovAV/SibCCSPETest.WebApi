using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SibCCSPETest.Data;
using SibCCSPETest.ServiceBase;

namespace SibCCSPETest.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IRepoServiceManager service, IMapper mapper) : ControllerBase
    {
        private readonly IRepoServiceManager _service = service;
        private readonly IMapper _mapper = mapper;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAll()
        {
            var users = await _service.UserRepository.GetAllUserAsync();
            var userDTOs = _mapper.Map<IEnumerable<UserDTO>>(users);
            return Ok(userDTOs);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<UserDTO>> Get(int id)
        {
            var user = await _service.UserRepository.GetUserAsync(u => u.Id == id);
            if (user == null)
                return NotFound(new { Message = $"Пользователь с id {id} не найден." });
            var userDTO = _mapper.Map<UserDTO>(user);
            return Ok(userDTO);
        }

        [HttpPost]
        public async Task<ActionResult<UserDTO>> Add(UserCreateDTO userCreateDTO)
        {
            if (userCreateDTO == null)
                return BadRequest("Данные для добавления пользователя пустые.");
            var user = _mapper.Map<User>(userCreateDTO);
            await _service.UserRepository.AddUserAsync(user);
            var userDTO = _mapper.Map<UserDTO>(user);
            return CreatedAtAction(nameof(Get), new { id = userDTO.Id }, userDTO);
        }

        [HttpPut]
        public async Task<ActionResult<GroupDTO>> Update(UserDTO userDTO)
        {
            if (userDTO == null)
                return BadRequest("Данные для обновления пользователя пустые.");
            var user = await _service.UserRepository.GetUserAsync(u => u.Id == userDTO.Id);
            if (user == null)
                return NotFound(new { Message = $"Пользователь с id {userDTO.Id} не найден." });
            _mapper.Map(userDTO, user);
            await _service.UserRepository.UpdateUser(user);
            userDTO = _mapper.Map<UserDTO>(user);
            return Ok(userDTO);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _service.UserRepository.GetUserAsync(u => u.Id == id);
            if (user == null)
                return NotFound(new { Message = $"Пользователь с id {id} не найден." });
            _service.UserRepository.DeleteUser(user);
            return NoContent();
        }
    }
}
