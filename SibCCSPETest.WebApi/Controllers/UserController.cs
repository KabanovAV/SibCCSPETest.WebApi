using Microsoft.AspNetCore.Mvc;
using SibCCSPETest.Data;
using SibCCSPETest.ServiceBase;

namespace SibCCSPETest.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController(IRepoServiceManager service) : ControllerBase
    {
        private readonly IRepoServiceManager _service = service;

        [HttpGet]
        public ActionResult<IEnumerable<User>> GetAll()
        {
            var users = _service.UserRepository.GetAllUser();
            return Ok(users);
        }

        [HttpGet]
        public ActionResult<User> Get([FromQuery] int id)
        {
            var user = _service.UserRepository.GetUser(u => u.Id == id);
            if (user == null)
                return NotFound(new { Message = $"Пользователь с id {id} не найден." });
            return Ok(user);
        }

        [HttpPost]
        public ActionResult<User> Add([FromBody] User user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            _service.UserRepository.AddUser(user);
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
        public IActionResult Delete([FromQuery] int id)
        {
            var user = _service.UserRepository.GetUser(u => u.Id == id);
            if (user == null)
                return NotFound(new { Message = $"Пользователь с id {id} не найден." });
            _service.UserRepository.DeleteUser(user);
            return NoContent();
        }
    }
}
