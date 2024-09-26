using AppointmentShedulerAPI.Models.Domain;

namespace AppointmentShedulerAPI.Repositories.Interface
{
    public interface IAppointmentRepository
    {
        Task<Appointment> CreateAppointmentAsync(Appointment appointment);
    }
}
