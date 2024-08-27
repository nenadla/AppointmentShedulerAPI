using AppointmentShedulerAPI.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace AppointmentShedulerAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Appointment> Appointments { get; set; }
    }
}
