using AppointmentShedulerAPI.Data;
using AppointmentShedulerAPI.Models.Domain;
using AppointmentShedulerAPI.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace AppointmentShedulerAPI.Repositories.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext dbContext;

        public UserRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<User> CreateUserAsync(User user)
        {
           await dbContext.Users.AddAsync(user);
           await dbContext.SaveChangesAsync();
            return user;
        }

        public async Task<User?> DeleteUser(Guid id)
        {
            var exsistingUser = await dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (exsistingUser == null) { return null; }
            dbContext.Users.Remove(exsistingUser);
            await dbContext.SaveChangesAsync();
            return exsistingUser;
        }

        public async Task<User?> FindByIdAsync(Guid id)
        {
            return await dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await dbContext.Users.ToListAsync();
        }
    }
}
