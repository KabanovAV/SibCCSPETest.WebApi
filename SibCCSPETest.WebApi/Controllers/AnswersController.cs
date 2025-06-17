using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SibCCSPETest.Data;
using SibCCSPETest.ServiceBase;

namespace SibCCSPETest.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AnswersController(IRepoServiceManager service, IMapper mapper) : ControllerBase
    {
        private readonly IRepoServiceManager _service = service;
        private readonly IMapper _mapper = mapper;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AnswerDTO>>> GetAll()
        {
            var answers = await _service.AnswerRepository.GetAllAnswerAsync();
            var answerDTOs = _mapper.Map<IEnumerable<AnswerDTO>>(answers);
            return Ok(answerDTOs);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<AnswerDTO>> Get(int id)
        {
            var answer = await _service.AnswerRepository.GetAnswerAsync(a => a.Id == id);
            if (answer == null)
                return NotFound(new { Message = $"Ответ с id {id} не найден." });
            var answerDTO = _mapper.Map<AnswerDTO>(answer);
            return Ok(answerDTO);
        }

        [HttpPost]
        public async Task<ActionResult<AnswerDTO>> Add([FromBody] AnswerCreateDTO answerCreateDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var answer = _mapper.Map<Answer>(answerCreateDTO);
            await _service.AnswerRepository.AddAnswerAsync(answer);
            return CreatedAtAction(nameof(Get), new { id = answer.Id }, answer);
        }

        [HttpPut]
        public IActionResult Update([FromBody] AnswerDTO answerDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var answer = _mapper.Map<Answer>(answerDTO);
            _service.AnswerRepository.UpdateAnswer(answer);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var answer = await _service.AnswerRepository.GetAnswerAsync(a => a.Id == id);
            if (answer == null)
                return NotFound(new { Message = $"Ответ с id {id} не найден." });
            _service.AnswerRepository.DeleteAnswer(answer);
            return NoContent();
        }
    }
}
