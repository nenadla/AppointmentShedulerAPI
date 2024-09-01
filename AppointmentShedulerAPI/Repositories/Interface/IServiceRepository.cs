using AppointmentShedulerAPI.Models.Domain;

namespace AppointmentShedulerAPI.Repositories.Interface
{
    public interface IServiceRepository
    {
        Task<Service> CreateService(Service service);
        Task<IEnumerable<Service>> GetAllAsync();
        Task<Service?> GetByIdAsync(Guid id);
        Task<Service?> UpdateByIdAsync(Service service);
        Task<Service?> DeleteByIdAsync(Guid id);
    }
}
