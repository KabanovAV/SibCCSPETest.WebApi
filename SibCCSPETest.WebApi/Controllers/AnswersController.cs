using Microsoft.AspNetCore.Mvc;
using SibCCSPETest.Data;
using SibCCSPETest.ServiceBase;

namespace SibCCSPETest.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AnswersController(IRepoServiceManager service) : ControllerBase
    {
        private readonly IRepoServiceManager _service = service;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Answer>>> GetAll()
        {
            var answers = await _service.AnswerRepository.GetAllAnswerAsync();
            return Ok(answers);
        }

        [HttpGet]
        public async Task<ActionResult<Answer>> Get([FromQuery] int id)
        {
            var answer = await _service.AnswerRepository.GetAnswerAsync(a => a.Id == id);
            if (answer == null)
                return NotFound(new { Message = $"Ответ с id {id} не найден." });
            return Ok(answer);
        }

        [HttpPost]
        public async Task<ActionResult<Answer>> Add([FromBody] Answer answer)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            await _service.AnswerRepository.AddAnswerAsync(answer);
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
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            var answer = await _service.AnswerRepository.GetAnswerAsync(a => a.Id == id);
            if (answer == null)
                return NotFound(new { Message = $"Ответ с id {id} не найден." });
            _service.AnswerRepository.DeleteAnswer(answer);
            return NoContent();
        }
    }
}
