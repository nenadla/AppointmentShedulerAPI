using AppointmentShedulerAPI.Models.Domain;

namespace AppointmentShedulerAPI.Repositories.Interface
{
    public interface IUserRepository
    {
        Task<User> CreateUserAsync(User user);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User?> FindByIdAsync(Guid id);
        Task<User?> DeleteUser(Guid id);
    }
}
