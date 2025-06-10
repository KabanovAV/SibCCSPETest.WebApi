using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SibCCSPETest.Data;
using SibCCSPETest.ServiceBase;

namespace SibCCSPETest.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecializationsController(IRepoServiceManager service, IMapper mapper) : ControllerBase
    {
        private readonly IRepoServiceManager _service = service;
        private readonly IMapper _mapper = mapper;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SpecializationDTO>>> GetAll()
        {
            var specializations = await _service.SpecializationRepository.GetAllSpecializationAsync();
            var specializationDTOs = _mapper.Map<IEnumerable<SpecializationDTO>>(specializations);
            return Ok(specializationDTOs);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<SpecializationDTO>> Get(int id)
        {
            var specialization = await _service.SpecializationRepository.GetSpecializationAsync(s => s.Id == id);
            if (specialization == null)
                return NotFound(new { Message = $"Специализация с id {id} не найдена." });
            var specializationDTO = _mapper.Map<SpecializationDTO>(specialization);
            return Ok(specializationDTO);
        }

        [HttpPost]
        public async Task<ActionResult<SpecializationDTO>> Add([FromBody] SpecializationCreateDTO specializationCreateDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var specialization = _mapper.Map<Specialization>(specializationCreateDTO);
            await _service.SpecializationRepository.AddSpecializationAsync(specialization);
            return CreatedAtAction(nameof(Get), new { id = specialization.Id }, specialization);
        }

        [HttpPut]
        public IActionResult Update([FromBody] SpecializationDTO specializationDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var specialization = _mapper.Map<Specialization>(specializationDTO);
            _service.SpecializationRepository.UpdateSpecialization(specialization);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var specialization = await _service.SpecializationRepository.GetSpecializationAsync(s => s.Id == id);
            if (specialization == null)
                return NotFound(new { Message = $"Специализация с id {id} не найдена." });
            _service.SpecializationRepository.DeleteSpecialization(specialization);
            return NoContent();
        }
    }
}
