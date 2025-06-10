using Microsoft.AspNetCore.Mvc;
using SibCCSPETest.Data;
using SibCCSPETest.ServiceBase;

namespace SibCCSPETest.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TopicsController(IRepoServiceManager service) : ControllerBase
    {
        private readonly IRepoServiceManager _service = service;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Topic>>> GetAll()
        {
            var topics = await _service.TopicRepository.GetAllTopicAsync();
            return Ok(topics);
        }

        [HttpGet]
        public async Task<ActionResult<Topic>> Get([FromQuery] int id)
        {
            var topic = await _service.TopicRepository.GetTopicAsync(t => t.Id == id);
            if (topic == null)
                return NotFound(new { Message = $"Тема с id {id} не найдена." });
            return Ok(topic);
        }

        [HttpPost]
        public async Task<ActionResult<Topic>> Add([FromBody] Topic topic)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            await _service.TopicRepository.AddTopicAsync(topic);
            return CreatedAtAction(nameof(Get), new { id = topic.Id }, topic);
        }

        [HttpPut]
        public IActionResult Update([FromBody] Topic topic)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            _service.TopicRepository.UpdateTopic(topic);
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            var topic = await _service.TopicRepository.GetTopicAsync(t => t.Id == id);
            if (topic == null)
                return NotFound(new { Message = $"Тема с id {id} не найдена." });
            _service.TopicRepository.DeleteTopic(topic);
            return NoContent();
        }
    }
}
