using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SibCCSPETest.Data;
using SibCCSPETest.ServiceBase;

namespace SibCCSPETest.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class QuestionsController(IRepoServiceManager service, IMapper mapper) : ControllerBase
    {
        private readonly IRepoServiceManager _service = service;
        private readonly IMapper _mapper = mapper;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuestionDTO>>> GetAll()
        {
            var questions = await _service.QuestionRepository.GetAllQuestionAsync();
            var questionDTOs = _mapper.Map<IEnumerable<QuestionDTO>>(questions);
            return Ok(questionDTOs);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<QuestionDTO>> Get(int id)
        {
            var question = await _service.QuestionRepository.GetQuestionAsync(q => q.Id == id);
            if (question == null)
                return NotFound(new { Message = $"Вопрос с id {id} не найден." });
            var questionDTO = _mapper.Map<QuestionDTO>(question);
            return Ok(questionDTO);
        }

        [HttpPost]
        public async Task<ActionResult<QuestionDTO>> Add([FromBody] QuestionCreateDTO questionCreateDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var question = _mapper.Map<Question>(questionCreateDTO);
            await _service.QuestionRepository.AddQuestionAsync(question);
            return CreatedAtAction(nameof(Get), new { id = question.Id }, question);
        }

        [HttpPut]
        public IActionResult Update([FromBody] QuestionDTO questionDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var question = _mapper.Map<Question>(questionDTO);
            _service.QuestionRepository.UpdateQuestion(question);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var question = await _service.QuestionRepository.GetQuestionAsync(q => q.Id == id);
            if (question == null)
                return NotFound(new { Message = $"Вопрос с id {id} не найден." });
            _service.QuestionRepository.DeleteQuestion(question);
            return NoContent();
        }
    }
}
