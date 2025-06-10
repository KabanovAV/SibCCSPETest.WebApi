using Microsoft.AspNetCore.Mvc;
using SibCCSPETest.Data;
using SibCCSPETest.ServiceBase;

namespace SibCCSPETest.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class QuestionsController(IRepoServiceManager service) : ControllerBase
    {
        private readonly IRepoServiceManager _service = service;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Question>>> GetAll()
        {
            var questions = await _service.QuestionRepository.GetAllQuestionAsync();
            return Ok(questions);
        }

        [HttpGet]
        public async Task<ActionResult<Question>> Get([FromQuery] int id)
        {
            var question = await _service.QuestionRepository.GetQuestionAsync(q => q.Id == id);
            if (question == null)
                return NotFound(new { Message = $"Вопрос с id {id} не найден." });
            return Ok(question);
        }

        [HttpPost]
        public async Task<ActionResult<Question>> Add([FromBody] Question question)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            await _service.QuestionRepository.AddQuestionAsync(question);
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
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            var question = await _service.QuestionRepository.GetQuestionAsync(q => q.Id == id);
            if (question == null)
                return NotFound(new { Message = $"Вопрос с id {id} не найден." });
            _service.QuestionRepository.DeleteQuestion(question);
            return NoContent();
        }
    }
}
