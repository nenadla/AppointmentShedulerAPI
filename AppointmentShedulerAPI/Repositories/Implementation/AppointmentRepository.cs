using AppointmentShedulerAPI.Data;
using AppointmentShedulerAPI.Models.Domain;
using AppointmentShedulerAPI.Repositories.Interface;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace AppointmentShedulerAPI.Repositories.Implementation
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly ApplicationDbContext dbContext;

        public AppointmentRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Appointment> CreateAppointmentAsync(Appointment appointment)
        {
            
            var user = await dbContext.Users.FindAsync(appointment.UserId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            user.Counter++;

            
            var service = await dbContext.Services.FindAsync(appointment.ServiceId);
            if (service == null)
            {
                throw new Exception("Service not found");
            }

            appointment.User = user;
            appointment.Service = service;

            DateTime today = DateTime.Today;
            DateTime maxDate = today.AddDays(21);
            var existingAppointments = await GetAppointmentsInRangeAsync(today, maxDate);

            bool isOverlapping = existingAppointments.Any(a =>
               (appointment.StartTime < a.EndTime && appointment.EndTime > a.StartTime) || 
               (appointment.EndTime > a.StartTime && appointment.EndTime < a.EndTime) || 
               (appointment.StartTime <= a.StartTime && appointment.EndTime >= a.EndTime) || 
               (appointment.StartTime >= a.StartTime && appointment.EndTime <= a.EndTime));

            if (isOverlapping)
            {
                throw new Exception("Termin se preklapa sa postojećim terminima.");
            }


            await dbContext.Appointments.AddAsync(appointment);
            await dbContext.SaveChangesAsync();  

            return appointment;
        }

        public async Task<Appointment?> DeleteAppointment(Guid id)
        {
           var exsistingAppointment = await dbContext.Appointments
                    .Include(a => a.User)
                    .Include(a => a.Service)
                    .FirstOrDefaultAsync(a => a.Id == id);
            if (exsistingAppointment == null)
            {
                return null;
            }

            if (exsistingAppointment.User != null)
            {
                exsistingAppointment.User.Cancelled += 1;
            }

            dbContext.Appointments.Remove(exsistingAppointment);
            await dbContext.SaveChangesAsync();
            return exsistingAppointment;
        }

        public async Task<Appointment?> FindById(Guid id)
        {
             return await dbContext.Appointments
                    .Include(a => a.User)     
                    .Include(a => a.Service)  
                    .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Appointment>> GetAllAppointmentsAsync()
        {
            return await dbContext.Appointments
                        .Include(a => a.User)
                        .Include(a => a.Service).ToListAsync();

        }

        public async Task<List<Appointment>> GetAppointmentsInRangeAsync(DateTime start, DateTime end)
        {
            return await dbContext.Appointments
                .Where(a => a.StartTime >= start && a.StartTime < end) 
                .ToListAsync();
        }


        public async Task<Appointment?> UpdateAppointmentByIdAsync(Appointment appointment)
        {
           
            var existingAppointment = await dbContext.Appointments
                .Include(a => a.User)
                .Include(a => a.Service)
                .FirstOrDefaultAsync(a => a.Id == appointment.Id);

            if (existingAppointment == null)
            {
                return null; 
            }

            DateTime today = DateTime.Today;
            DateTime maxDate = today.AddDays(21);
            var existingAppointments = await GetAppointmentsInRangeAsync(today, maxDate);

            bool isOverlapping = existingAppointments.Any(a =>
               (appointment.StartTime < a.EndTime && appointment.EndTime > a.StartTime) ||
               (appointment.EndTime > a.StartTime && appointment.EndTime < a.EndTime) ||
               (appointment.StartTime <= a.StartTime && appointment.EndTime >= a.EndTime) ||
               (appointment.StartTime >= a.StartTime && appointment.EndTime <= a.EndTime));

            if (isOverlapping)
            {
                throw new Exception("Termin se preklapa sa postojećim terminima.");
            }


            existingAppointment.StartTime = appointment.StartTime;
            existingAppointment.EndTime = appointment.EndTime;
            existingAppointment.Worker = appointment.Worker;

            
            existingAppointment.UserId = appointment.UserId;
            existingAppointment.ServiceId = appointment.ServiceId;

            var exsistingUser = await dbContext.Users.FirstOrDefaultAsync(x => x.Id == appointment.UserId);
            if (exsistingUser != null) 
            {
                exsistingUser.Edited++;
            }
            await dbContext.SaveChangesAsync();
            return existingAppointment; 
        }

    }
}
