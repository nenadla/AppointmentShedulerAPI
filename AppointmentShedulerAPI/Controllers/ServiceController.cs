using AppointmentShedulerAPI.Models.Domain;
using AppointmentShedulerAPI.Models.DTO;
using AppointmentShedulerAPI.Repositories.Interface;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentShedulerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly IServiceRepository serviceRepository;
        private readonly IMapper mapper;
        public ServiceController(IServiceRepository serviceRepository, IMapper mapper)
        {
            this.serviceRepository = serviceRepository;
            this.mapper = mapper;
           
    }

        [HttpPost]
        public async Task<IActionResult> CreateService([FromBody] CreateServiceRequest request)
        {
            var service = mapper.Map<Service>(request);
           
            await serviceRepository.CreateService(service);

            var response =mapper.Map<ServiceDto>(service);
            return Ok(response);

        }

        [HttpGet]
        public async Task<IActionResult> GetAllServices()
        {
            var services = await serviceRepository.GetAllAsync();
            var response = mapper.Map<List<ServiceDto>>(services);

            return Ok(response);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetServiceById([FromRoute] Guid id)
        {
            var exsistingService = await serviceRepository.GetByIdAsync(id);

            if (exsistingService == null)
            {
                return NotFound();
            }

            var response = mapper.Map<ServiceDto>(exsistingService);
            return Ok(response);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateService([FromRoute] Guid id, UpdateServiceRequestDto request)
        {
            var service = mapper.Map<Service>(request);
            service.Id = id;

            service = await serviceRepository.UpdateByIdAsync(service);
            if (service == null)
            {
                return NotFound();
            }
            var response = mapper.Map<ServiceDto>(service);
            return Ok(response);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteService([FromRoute] Guid id)
        {
          var service =  await serviceRepository.DeleteByIdAsync(id);

            if (service == null)
            {
                return NotFound();
            }
            var response = mapper.Map<ServiceDto>(service);
            return Ok(response);
        }
        
    }
}
