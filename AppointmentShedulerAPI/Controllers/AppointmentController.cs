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
                Username=request.Username,
                Email=request.Email,
                Phone=request.Phone,
                StartTime=request.StartTime,
                EndTime=request.EndTime,
                Service=request.Service,
                Worker=request.Worker,
                Counter=1,
                Cancelled=0
            };

           await appointmentRepository.CreateAsync(appointment);

            var response = new AppointmentDto
            {
                Id=appointment.Id,
                Username=appointment.Username,
                Email=appointment.Email,
                Phone=appointment.Phone,
                StartTime=appointment.StartTime,
                EndTime=appointment.EndTime,
                Service=appointment.Service,
                Worker=appointment.Worker,
                Counter = (int)appointment.Counter,
                Cancelled = (int)appointment.Cancelled

            };

            return Ok(response);

        }
       
    }
}
