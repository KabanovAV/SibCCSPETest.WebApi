using Microsoft.AspNetCore.Mvc;
using SibCCSPETest.Data;
using SibCCSPETest.ServiceBase;

namespace SibCCSPETest.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecializationController(IRepoServiceManager service) : ControllerBase
    {
        private readonly IRepoServiceManager _service = service;

        [HttpGet("All")]
        public ActionResult<IEnumerable<Specialization>> GetAll()
        {
            var specializations = _service.SpecializationRepository.GetAllSpecialization();
            return Ok(specializations);
        }

        [HttpGet("{id:int}")]
        public ActionResult<Specialization> Get(int id)
        {
            var specialization = _service.SpecializationRepository.GetSpecialization(s => s.Id == id);
            if (specialization == null)
                return NotFound(new { Message = $"Специализация с id {id} не найдена." });
            return Ok(specialization);
        }

        [HttpPost]
        public ActionResult<Specialization> Add([FromBody] Specialization specialization)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            _service.SpecializationRepository.AddSpecialization(specialization);
            return CreatedAtAction(nameof(Get), new { id = specialization.Id }, specialization);
        }

        [HttpPut]
        public IActionResult Update([FromBody] Specialization specialization)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            _service.SpecializationRepository.UpdateSpecialization(specialization);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var specialization = _service.SpecializationRepository.GetSpecialization(s => s.Id == id);
            if (specialization == null)
                return NotFound(new { Message = $"Специализация с id {id} не найдена." });
            _service.SpecializationRepository.DeleteSpecialization(specialization);
            return NoContent();
        }
    }
}
