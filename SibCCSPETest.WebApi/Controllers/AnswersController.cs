using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SibCCSPETest.Data;
using SibCCSPETest.ServiceBase;

namespace SibCCSPETest.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnswersController(IRepoServiceManager service, IMapper mapper) : ControllerBase
    {
        private readonly IRepoServiceManager _service = service;
        private readonly IMapper _mapper = mapper;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AnswerDTO>>> GetAll()
        {
            var answers = await _service.AnswerRepository.GetAllAnswerAsync(includeProperties: "Question");
            var answerDTOs = _mapper.Map<IEnumerable<AnswerDTO>>(answers);
            return Ok(answerDTOs);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<AnswerDTO>> Get(int id)
        {
            var answer = await _service.AnswerRepository.GetAnswerAsync(a => a.Id == id, "Question");
            if (answer == null)
                return NotFound(new { Message = $"Ответ с id {id} не найден." });
            var answerDTO = _mapper.Map<AnswerDTO>(answer);
            return Ok(answerDTO);
        }

        [HttpPost]
        public async Task<ActionResult<AnswerDTO>> Add(AnswerCreateDTO answerCreateDTO)
        {
            if (answerCreateDTO == null)
                return BadRequest("Данные для добавления ответа пустые.");
            var answer = _mapper.Map<Answer>(answerCreateDTO);
            await _service.AnswerRepository.AddAnswerAsync(answer, "Question");
            var answerDTO = _mapper.Map<AnswerDTO>(answer);
            return CreatedAtAction(nameof(Get), new { id = answerDTO.Id }, answerDTO);
        }

        [HttpPost("AddRange")]
        public async Task<ActionResult<List<AnswerDTO>>> Add(List<AnswerCreateDTO> answerCreateDTOs)
        {
            if (answerCreateDTOs == null)
                return BadRequest("Данные для добавления ответов пустые.");
            var answers = _mapper.Map<List<Answer>>(answerCreateDTOs);
            await _service.AnswerRepository.AddRangeAnswerAsync(answers, "Question");
            var answerDTOs = _mapper.Map<List<AnswerDTO>>(answers);
            return CreatedAtAction(nameof(GetAll), answerDTOs);
        }

        [HttpPut]
        public async Task<ActionResult<GroupDTO>> Update(AnswerDTO answerDTO)
        {
            if (answerDTO == null)
                return BadRequest("Данные для обновления ответа пустые.");
            var answer = await _service.AnswerRepository.GetAnswerAsync(a => a.Id == answerDTO.Id, "Question");
            if (answer == null)
                return NotFound(new { Message = $"Вопрос с id {answerDTO.Id} не найден." });
            _mapper.Map(answerDTO, answer);
            await _service.AnswerRepository.UpdateAnswer(answer, "Question");
            answerDTO = _mapper.Map<AnswerDTO>(answer);
            return Ok(answerDTO);
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
