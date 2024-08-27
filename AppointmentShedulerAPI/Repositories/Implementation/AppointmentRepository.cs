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
        public async Task<Appointment> CreateAsync(Appointment appointment)
        {
            await dbContext.Appointments.AddAsync(appointment);
            await dbContext.SaveChangesAsync();

            return appointment;
        }
    }
}
