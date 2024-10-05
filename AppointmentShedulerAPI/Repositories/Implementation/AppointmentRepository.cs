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
            // Proveri da li User postoji u bazi
            var user = await dbContext.Users.FindAsync(appointment.UserId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            user.Counter++;

            // Proveri da li Service postoji u bazi
            var service = await dbContext.Services.FindAsync(appointment.ServiceId);
            if (service == null)
            {
                throw new Exception("Service not found");
            }

            appointment.User = user;
            appointment.Service = service;

            // Kreiraj novi termin
            await dbContext.Appointments.AddAsync(appointment);
            await dbContext.SaveChangesAsync();  // Čuvanje promena uključujući ažuriranje User.Counter

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

          
            existingAppointment.StartTime = appointment.StartTime;
            existingAppointment.EndTime = appointment.EndTime;
            existingAppointment.Worker = appointment.Worker;

            

            if (existingAppointment.User != null)
            {
                
                existingAppointment.User.Username = appointment.User?.Username ?? existingAppointment.User.Username;
                existingAppointment.User.Phone = appointment.User?.Phone ?? existingAppointment.User.Phone;
                existingAppointment.User.Email = appointment.User?.Email ?? existingAppointment.User.Email;

            }


            if (appointment.Service != null && existingAppointment.Service != null)
            {
                existingAppointment.Service.Name = appointment.Service.Name ?? existingAppointment.Service.Name;
                existingAppointment.Service.Duration = appointment.Service.Duration != 0 ? appointment.Service.Duration : existingAppointment.Service.Duration;
                existingAppointment.Service.Price = appointment.Service.Price != 0 ? appointment.Service.Price : existingAppointment.Service.Price;

               
                
            }

            
            await dbContext.SaveChangesAsync();
            return existingAppointment; // Vrati ažurirani appointment
        }

    }
}
