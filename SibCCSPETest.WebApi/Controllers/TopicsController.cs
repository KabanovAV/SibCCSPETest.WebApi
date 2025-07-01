using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SibCCSPETest.Data;
using SibCCSPETest.ServiceBase;

namespace SibCCSPETest.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TopicsController(IRepoServiceManager service, IMapper mapper) : ControllerBase
    {
        private readonly IRepoServiceManager _service = service;
        private readonly IMapper _mapper = mapper;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TopicDTO>>> GetAll()
        {
            var topics = await _service.TopicRepository.GetAllTopicAsync(includeProperties: "Specialization");
            var topicDTOs = _mapper.Map<IEnumerable<TopicDTO>>(topics);
            return Ok(topicDTOs);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<TopicDTO>> Get(int id)
        {
            var topic = await _service.TopicRepository.GetTopicAsync(t => t.Id == id, "Specialization");
            if (topic == null)
                return NotFound(new { Message = $"Тема с id {id} не найдена." });
            var topicDTO = _mapper.Map<TopicDTO>(topic);
            return Ok(topicDTO);
        }

        [HttpPost]
        public async Task<ActionResult<TopicDTO>> Add(TopicCreateDTO topicCreateDTO)
        {
            if (topicCreateDTO == null)
                return BadRequest("Данные для добавления темы пустые.");
            var topic = _mapper.Map<Topic>(topicCreateDTO);
            await _service.TopicRepository.AddTopicAsync(topic, "Specialization");
            var topicDTO = _mapper.Map<TopicDTO>(topic);
            return CreatedAtAction(nameof(Get), new { id = topicDTO.Id }, topicDTO);
        }

        [HttpPut]
        public async Task<ActionResult<GroupDTO>> Update(TopicDTO topicDTO)
        {
            if (topicDTO == null)
                return BadRequest("Данные для обновления темы пустые.");
            var topic = await _service.TopicRepository.GetTopicAsync(t => t.Id == topicDTO.Id, "Specialization");
            if (topic == null)
                return NotFound(new { Message = $"Тема с id {topicDTO.Id} не найдена." });
            _mapper.Map(topicDTO, topic);
            await _service.TopicRepository.UpdateTopic(topic, "Specialization");
            topicDTO = _mapper.Map<TopicDTO>(topic);
            return Ok(topicDTO);
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
