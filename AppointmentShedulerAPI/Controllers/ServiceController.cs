using AppointmentShedulerAPI.Models.Domain;
using AppointmentShedulerAPI.Models.DTO;
using AppointmentShedulerAPI.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentShedulerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly IServiceRepository serviceRepository;
        public ServiceController(IServiceRepository serviceRepository)
        {
            this.serviceRepository = serviceRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateService([FromBody] CreateServiceRequest request)
        {
            var service = new Models.Domain.Service
            {
                Name = request.Name,
                Duration = request.Duration,
                Price = request.Price
            };

            await serviceRepository.CreateService(service);

            var response = new ServiceDto
            {
                Id = service.Id,
                Name = service.Name,
                Duration = service.Duration,
                Price = service.Price
            };
            return Ok(response);

        }

        [HttpGet]
        public async Task<IActionResult> GetAllServices()
        {
            var services = await serviceRepository.GetAllAsync();
            var response = new List<ServiceDto>();

            foreach (var service in services)
            {
                response.Add(new ServiceDto
                {
                    Id = service.Id,
                    Name = service.Name,
                    Duration = service.Duration,
                    Price = service.Price
                });
            }

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

            var response = new ServiceDto
            {
                Id = exsistingService.Id,
                Name = exsistingService.Name,
                Duration = exsistingService.Duration,
                Price = exsistingService.Price
            };
            return Ok(response);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateService([FromRoute] Guid id, UpdateServiceRequestDto request)
        {
            var service = new Service
            {
                Id = id,
                Name = request.Name,
                Duration = request.Duration,
                Price = request.Price,
            };

            service = await serviceRepository.UpdateByIdAsync(service);
            if (service == null)
            {
                return NotFound();
            }
            var response = new ServiceDto
            {
                Id = service.Id,
                Name = service.Name,
                Duration = service.Duration,
                Price = service.Price,
            };
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
            var response = new ServiceDto
            {
                Id= service.Id,
                Name = service.Name,
                Duration = service.Duration,
                Price = service.Price
            };
            return Ok(response);
        }
        
    }
}
