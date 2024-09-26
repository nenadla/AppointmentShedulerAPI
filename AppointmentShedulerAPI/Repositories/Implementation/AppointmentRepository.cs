using AppointmentShedulerAPI.Data;
using AppointmentShedulerAPI.Models.Domain;
using AppointmentShedulerAPI.Repositories.Interface;
using Microsoft.AspNetCore.Http.HttpResults;

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

    }
}
