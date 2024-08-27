using AppointmentShedulerAPI.Models.Domain;

namespace AppointmentShedulerAPI.Repositories.Interface
{
    public interface IAppointmentRepository
    {
        Task<Appointment> CreateAsync(Appointment appointment);
    }
}
