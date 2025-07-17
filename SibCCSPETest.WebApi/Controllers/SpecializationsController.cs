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
        public async Task<ActionResult<SpecializationDTO>> Add(SpecializationCreateDTO specializationCreateDTO)
        {
            if (specializationCreateDTO == null)
                return BadRequest("Данные для добавления специализации пустые.");
            var specialization = _mapper.Map<Specialization>(specializationCreateDTO);
            await _service.SpecializationRepository.AddSpecializationAsync(specialization);
            var specializationDTO = _mapper.Map<SpecializationDTO>(specialization);
            return CreatedAtAction(nameof(Get), new { id = specializationDTO.Id }, specializationDTO);
        }

        [HttpPut]
        public async Task<ActionResult<SpecializationDTO>> Update(SpecializationDTO specializationDTO)
        {
            if (specializationDTO == null)
                return BadRequest("Данные для обновления специализации пустые.");
            var specialization = await _service.SpecializationRepository.GetSpecializationAsync(s => s.Id == specializationDTO.Id);
            if (specialization == null)
                return NotFound(new { Message = $"Специализация с id {specializationDTO.Id} не найдена." });
            _mapper.Map(specializationDTO, specialization);
            await _service.SpecializationRepository.UpdateSpecialization(specialization);
            specializationDTO = _mapper.Map<SpecializationDTO>(specialization);
            return Ok(specializationDTO);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var specialization = await _service.SpecializationRepository.GetSpecializationAsync(s => s.Id == id, "Groups, Topics");
            if (specialization == null)
                return NotFound(new { Message = $"Специализация с id {id} не найдена." });
            var result = _service.SpecializationRepository.DeleteSpecialization(specialization);
            return Ok(result);
        }
    }
}
