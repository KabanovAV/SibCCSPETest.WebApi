using Microsoft.AspNetCore.Mvc;
using SibCCSPETest.Data;
using SibCCSPETest.ServiceBase;

namespace SibCCSPETest.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AnswerController(IRepoServiceManager service) : ControllerBase
    {
        private readonly IRepoServiceManager _service = service;

        [HttpGet]
        public ActionResult<IEnumerable<Answer>> GetAll()
        {
            var answers = _service.AnswerRepository.GetAllAnswer();
            return Ok(answers);
        }

        [HttpGet]
        public ActionResult<Answer> Get([FromQuery] int id)
        {
            var answer = _service.AnswerRepository.GetAnswer(a => a.Id == id);
            if (answer == null)
                return NotFound(new { Message = $"Ответ с id {id} не найден." });
            return Ok(answer);
        }

        [HttpPost]
        public ActionResult<Answer> Add([FromBody] Answer answer)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            _service.AnswerRepository.AddAnswer(answer);
            return CreatedAtAction(nameof(Get), new { id = answer.Id }, answer);
        }

        [HttpPut]
        public IActionResult Update([FromBody] Answer answer)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            _service.AnswerRepository.UpdateAnswer(answer);
            return NoContent();
        }

        [HttpDelete]
        public IActionResult Delete([FromQuery] int id)
        {
            var answer = _service.AnswerRepository.GetAnswer(a => a.Id == id);
            if (answer == null)
                return NotFound(new { Message = $"Ответ с id {id} не найден." });
            _service.AnswerRepository.DeleteAnswer(answer);
            return NoContent();
        }
    }
}
