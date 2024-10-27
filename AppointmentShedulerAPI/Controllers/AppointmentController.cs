using AppointmentShedulerAPI.Data;
using AppointmentShedulerAPI.Models.Domain;
using AppointmentShedulerAPI.Models.DTO;
using AppointmentShedulerAPI.Repositories.Interface;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

namespace AppointmentShedulerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentRepository appointmentRepository;
        private readonly IMapper mapper;

        public AppointmentController(IAppointmentRepository appointmentRepository, IMapper mapper)
        {
            this.appointmentRepository = appointmentRepository;
            this.mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAppointment(CreateAppointmentRequestDto request)
        {
            var startTime = request.StartTime;
            var today = DateTime.Today; 
            var maxDate = today.AddDays(20); 

            
            if (startTime.Date <= today || startTime.Date > maxDate)
            {
                return BadRequest("Zakazivanje je moguće za današnji dan i narednih 20 dana.");
            }

            if (startTime.DayOfWeek == DayOfWeek.Sunday)
            {
                return BadRequest("Zakazivanje nije moguće u nedelju.");
            }

            if (startTime.DayOfWeek >= DayOfWeek.Monday && startTime.DayOfWeek <= DayOfWeek.Friday)
            {
                if (startTime.TimeOfDay <= new TimeSpan(10, 0, 0) || startTime.TimeOfDay > new TimeSpan(19, 40, 0))
                {
                    return BadRequest("Vreme početka usluge mora biti između 10:00h i 19:40h radnim danima.");
                }
            }

            if (startTime.DayOfWeek == DayOfWeek.Saturday)
            {
                if (startTime.TimeOfDay <= new TimeSpan(9, 0, 0) || startTime.TimeOfDay > new TimeSpan(15, 40, 0))
                {
                    return BadRequest("Vreme početka usluge mora biti između 09:00h i 15:30h subotom.");
                }
            }
            var appointment = mapper.Map<Appointment>(request);

            try
            {
                await appointmentRepository.CreateAppointmentAsync(appointment);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); 
            }

            var response = mapper.Map<AppointmentDto>(appointment);

            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var existingAppointment = await appointmentRepository.FindById(id);

            if (existingAppointment == null)
            {
                return NotFound();
            }

            var response = mapper.Map<AppointmentDto>(existingAppointment);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAppointments()
        {
            var appointments = await appointmentRepository.GetAllAppointmentsAsync();
            var response = mapper.Map<List<AppointmentDto>>(appointments);

            return Ok(response);
        }

        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> DeleteAppointment([FromRoute] Guid id)
        {
            var existingAppointment = await appointmentRepository.DeleteAppointment(id);

            if (existingAppointment == null)
            {
                return NotFound();
            }

            var response = mapper.Map<AppointmentDto>(existingAppointment);
            return Ok(response);
        }

        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> UpdateAppointmentById([FromRoute] Guid id, UpdateAppointmentRequestDto request)
        {
            var startTime = request.StartTime;
            var today = DateTime.Today;
            var maxDate = today.AddDays(20);


            if (startTime.Date <= today || startTime.Date > maxDate)
            {
                return BadRequest("Zakazivanje je moguće za današnji dan i narednih 20 dana.");
            }

            if (startTime.DayOfWeek == DayOfWeek.Sunday)
            {
                return BadRequest("Zakazivanje nije moguće u nedelju.");
            }

            if (startTime.DayOfWeek >= DayOfWeek.Monday && startTime.DayOfWeek <= DayOfWeek.Friday)
            {
                if (startTime.TimeOfDay <= new TimeSpan(10, 0, 0) || startTime.TimeOfDay > new TimeSpan(19, 40, 0))
                {
                    return BadRequest("Vreme početka usluge mora biti između 10:00h i 19:40h radnim danima.");
                }
            }

            if (startTime.DayOfWeek == DayOfWeek.Saturday)
            {
                if (startTime.TimeOfDay <= new TimeSpan(9, 0, 0) || startTime.TimeOfDay > new TimeSpan(15, 40, 0))
                {
                    return BadRequest("Vreme početka usluge mora biti između 09:00h i 15:30h subotom.");
                }
            }
            var appointment = mapper.Map<Appointment>(request);
            appointment.Id = id;

            try
            {
                appointment = await appointmentRepository.UpdateAppointmentByIdAsync(appointment);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); 
            }

            if (appointment == null)
            {
                return NotFound();
            }

            var response = mapper.Map<AppointmentDto>(appointment);
            return Ok(response);
        }
    }
}
