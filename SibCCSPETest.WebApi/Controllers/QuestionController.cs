using Microsoft.AspNetCore.Mvc;
using SibCCSPETest.Data;
using SibCCSPETest.ServiceBase;

namespace SibCCSPETest.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class QuestionController(IRepoServiceManager service) : ControllerBase
    {
        private readonly IRepoServiceManager _service = service;

        [HttpGet]
        public ActionResult<IEnumerable<Question>> GetAll()
        {
            var questions = _service.QuestionRepository.GetAllQuestion();
            return Ok(questions);
        }

        [HttpGet]
        public ActionResult<Question> Get([FromQuery] int id)
        {
            var question = _service.QuestionRepository.GetQuestion(q => q.Id == id);
            if (question == null)
                return NotFound(new { Message = $"Вопрос с id {id} не найден." });
            return Ok(question);
        }

        [HttpPost]
        public ActionResult<Question> Add([FromBody] Question question)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            _service.QuestionRepository.AddQuestion(question);
            return CreatedAtAction(nameof(Get), new { id = question.Id }, question);
        }

        [HttpPut]
        public IActionResult Update([FromBody] Question question)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            _service.QuestionRepository.UpdateQuestion(question);
            return NoContent();
        }

        [HttpDelete]
        public IActionResult Delete([FromQuery] int id)
        {
            var question = _service.QuestionRepository.GetQuestion(q => q.Id == id);
            if (question == null)
                return NotFound(new { Message = $"Вопрос с id {id} не найден." });
            _service.QuestionRepository.DeleteQuestion(question);
            return NoContent();
        }
    }
}
