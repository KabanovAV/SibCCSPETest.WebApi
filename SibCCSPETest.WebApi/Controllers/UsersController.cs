using Microsoft.AspNetCore.Mvc;
using SibCCSPETest.Data;
using SibCCSPETest.ServiceBase;

namespace SibCCSPETest.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController(IRepoServiceManager service) : ControllerBase
    {
        private readonly IRepoServiceManager _service = service;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAll()
        {
            var users = await _service.UserRepository.GetAllUserAsync();
            return Ok(users);
        }

        [HttpGet]
        public async Task<ActionResult<User>> Get([FromQuery] int id)
        {
            var user = await _service.UserRepository.GetUserAsync(u => u.Id == id);
            if (user == null)
                return NotFound(new { Message = $"Пользователь с id {id} не найден." });
            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<User>> Add([FromBody] User user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            await _service.UserRepository.AddUserAsync(user);
            return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
        }

        [HttpPut]
        public IActionResult Update([FromBody] User user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            _service.UserRepository.UpdateUser(user);
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            var user = await _service.UserRepository.GetUserAsync(u => u.Id == id);
            if (user == null)
                return NotFound(new { Message = $"Пользователь с id {id} не найден." });
            _service.UserRepository.DeleteUser(user);
            return NoContent();
        }
    }
}
