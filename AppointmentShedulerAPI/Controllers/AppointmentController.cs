using AppointmentShedulerAPI.Data;
using AppointmentShedulerAPI.Models.Domain;
using AppointmentShedulerAPI.Models.DTO;
using AppointmentShedulerAPI.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentShedulerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentRepository appointmentRepository;

        public AppointmentController(IAppointmentRepository appointmentRepository)
        {
            this.appointmentRepository=appointmentRepository;
        }
        [HttpPost]

        public async Task<IActionResult> CreateAppointment(CreateAppointmentRequestDto request)
        {
            var appointment = new Appointment
            {
                Id = Guid.NewGuid(),  
                StartTime = request.StartTime,
                EndTime = request.StartTime.AddMinutes(request.Duration),  
                Worker = request.Worker,
                UserId = request.UserId,
                ServiceId = request.ServiceId
            };

            await appointmentRepository.CreateAppointmentAsync(appointment);
            
            var response = new AppointmentDto
                {
                    // Polja iz Appointment modela
                    AppointmentId = appointment.Id,
                    StartTime = appointment.StartTime,
                    EndTime = appointment.EndTime,
                    Worker = appointment.Worker,

                    // Polja iz User modela (preko navigacionog svojstva User)
                    UserId = appointment.UserId,
                    Username = appointment.User?.Username,  // Proveri da li User nije null
                    Phone = appointment.User?.Phone,
                    Email = appointment.User?.Email,
                    Counter = appointment.User?.Counter ?? 0,  // Ako je User null, postavi na 0
                    Edited = appointment.User?.Edited ?? 0,
                    Cancelled = appointment.User?.Cancelled ?? 0,

                    // Polja iz Service modela (preko navigacionog svojstva Service)
                    ServiceId = appointment.ServiceId,
                    ServiceName = appointment.Service?.Name,  // Proveri da li Service nije null
                    Duration = request.Duration,
                    Price = appointment.Service?.Price ?? 0
                };

            return Ok(response);

        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var exsistingAppointment = await appointmentRepository.FindById(id);

            if(exsistingAppointment == null)
            {
                return NotFound();
            }

            var response = new AppointmentDto
            {
                AppointmentId= exsistingAppointment.Id,
                StartTime= exsistingAppointment.StartTime,
                EndTime = exsistingAppointment.EndTime,
                Worker = exsistingAppointment.Worker,

                UserId = exsistingAppointment.UserId,
                Username = exsistingAppointment.User?.Username,
                Phone = exsistingAppointment.User?.Phone,
                Counter = exsistingAppointment.User?.Counter ?? 0,  // Ako je User null, postavi na 0
                Edited = exsistingAppointment.User?.Edited ?? 0,
                Cancelled = exsistingAppointment.User?.Cancelled ?? 0,

                ServiceId = exsistingAppointment.ServiceId,
                ServiceName = exsistingAppointment.Service?.Name,
                Duration = exsistingAppointment.Service?.Duration ?? 0,
                Price = exsistingAppointment.Service?.Price ?? 0
            };

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAppointments()
        {
            var appointments = await appointmentRepository.GetAllAppointmentsAsync();
            var response = new List<AppointmentDto>();

            foreach(var appointment in appointments)
            {
                response.Add(new AppointmentDto
                {
                    AppointmentId = appointment.Id,
                    StartTime = appointment.StartTime,
                    EndTime = appointment.EndTime,
                    Worker = appointment.Worker,

                   
                    UserId = appointment.UserId,
                    Username = appointment.User?.Username,  
                    Phone = appointment.User?.Phone,
                    Email = appointment.User?.Email,
                    Counter = appointment.User?.Counter ?? 0,  
                    Edited = appointment.User?.Edited ?? 0,
                    Cancelled = appointment.User?.Cancelled ?? 0,

                    
                    ServiceId = appointment.ServiceId,
                    ServiceName = appointment.Service?.Name,  
                    Duration = appointment.Service?.Duration ?? 0,
                    Price = appointment.Service?.Price ?? 0
                });
            }
            return Ok(response);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteAppointment([FromRoute] Guid id)
        {
            var exsistingAppointment = await appointmentRepository.DeleteAppointment(id);
            if (exsistingAppointment == null) { return NotFound(); }
            var response = new AppointmentDto
            {

                AppointmentId = exsistingAppointment.Id,
                StartTime = exsistingAppointment.StartTime,
                EndTime = exsistingAppointment.EndTime,
                Worker = exsistingAppointment.Worker,

                UserId = exsistingAppointment.UserId,
                Username = exsistingAppointment.User?.Username,
                Phone = exsistingAppointment.User?.Phone,
                Counter = exsistingAppointment.User?.Counter ?? 0,  
                Edited = exsistingAppointment.User?.Edited ?? 0,
                Cancelled = exsistingAppointment.User?.Cancelled ?? 0,

                ServiceId = exsistingAppointment.ServiceId,
                ServiceName = exsistingAppointment.Service?.Name,
                Duration = exsistingAppointment.Service?.Duration ?? 0,
                Price = exsistingAppointment.Service?.Price ?? 0
            };
            return Ok(response);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateAppointmentById([FromRoute] Guid id, UpdateAppointmentRequestDto request)
        {
            var appointment = new Appointment
            {
                Id = id,
                StartTime= request.StartTime,
                EndTime = request.StartTime.AddMinutes(request.Duration),
                Worker = request.Worker,
                UserId = request.UserId,
                ServiceId = request.ServiceId
            };
            
            appointment = await appointmentRepository.UpdateAppointmentByIdAsync(appointment);

            if(appointment == null) { return NotFound(); }

            var response = new AppointmentDto
            {
                AppointmentId = appointment.Id,
                StartTime = appointment.StartTime,
                EndTime = appointment.EndTime,
                Worker = appointment.Worker,

                UserId = appointment.UserId,
                Username = appointment.User?.Username,
                Phone = appointment.User?.Phone,
                Counter = appointment.User?.Counter ?? 0,  // Ako je User null, postavi na 0
                Edited = appointment.User?.Edited ?? 0,
                Cancelled = appointment.User?.Cancelled ?? 0,

                ServiceId = appointment.ServiceId,
                ServiceName = appointment.Service?.Name,
                Duration = appointment.Service?.Duration ?? 0,
                Price = appointment.Service?.Price ?? 0
            };

            return Ok(response);

        }

       

        }
}
