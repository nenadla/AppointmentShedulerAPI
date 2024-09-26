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
                    Duration = appointment.Service?.Duration ?? 0,
                    Price = appointment.Service?.Price ?? 0
                };

            return Ok(response);

        }
       
    }
}
