using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SibCCSPETest.Data;
using SibCCSPETest.ServiceBase;

namespace SibCCSPETest.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TopicsController(IRepoServiceManager service, IMapper mapper) : ControllerBase
    {
        private readonly IRepoServiceManager _service = service;
        private readonly IMapper _mapper = mapper;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TopicDTO>>> GetAll()
        {
            var topics = await _service.TopicRepository.GetAllTopicAsync();
            var topicDTOs = _mapper.Map<IEnumerable<TopicDTO>>(topics);
            return Ok(topicDTOs);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<TopicDTO>> Get(int id)
        {
            var topic = await _service.TopicRepository.GetTopicAsync(t => t.Id == id);
            if (topic == null)
                return NotFound(new { Message = $"Тема с id {id} не найдена." });
            var topicDTO = _mapper.Map<TopicDTO>(topic);
            return Ok(topicDTO);
        }

        [HttpPost]
        public async Task<ActionResult<TopicDTO>> Add([FromBody] TopicCreateDTO topicCreateDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var topic = _mapper.Map<Topic>(topicCreateDTO);
            await _service.TopicRepository.AddTopicAsync(topic);
            return CreatedAtAction(nameof(Get), new { id = topic.Id }, topic);
        }

        [HttpPut]
        public IActionResult Update([FromBody] TopicDTO topicDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var topic = _mapper.Map<Topic>(topicDTO);
            _service.TopicRepository.UpdateTopic(topic);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var topic = await _service.TopicRepository.GetTopicAsync(t => t.Id == id);
            if (topic == null)
                return NotFound(new { Message = $"Тема с id {id} не найдена." });
            _service.TopicRepository.DeleteTopic(topic);
            return NoContent();
        }
    }
}
