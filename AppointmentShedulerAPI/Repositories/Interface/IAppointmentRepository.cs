using AppointmentShedulerAPI.Models.Domain;

namespace AppointmentShedulerAPI.Repositories.Interface
{
    public interface IAppointmentRepository
    {
        Task<Appointment> CreateAppointmentAsync(Appointment appointment);
        Task<Appointment?> FindById(Guid id);
        Task<IEnumerable<Appointment>> GetAllAppointmentsAsync();
        Task<Appointment?> DeleteAppointment(Guid id);
        Task<Appointment?> UpdateAppointmentByIdAsync(Appointment appointment);
        Task<List<Appointment>> GetAppointmentsInRangeAsync(DateTime start, DateTime end);
    }
}
