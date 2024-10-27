using AppointmentShedulerAPI.Data;
using AppointmentShedulerAPI.Models.Domain;
using AppointmentShedulerAPI.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

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
            bool userExists = await dbContext.Users
                                .AnyAsync(u => u.Username == user.Username || u.Email == user.Email);
            if (userExists)
            {
               
                throw new InvalidOperationException("Korisnik sa ovim korisničkim imenom ili e-mailom već postoji.");
            }

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

        public async Task<User?> UpdateByIdAsync(User user)
        {
            bool userExists = await dbContext.Users
                                .AnyAsync(u => u.Username == user.Username || u.Email == user.Email);
            if (userExists)
            {

                throw new InvalidOperationException("Korisnik sa ovim korisničkim imenom ili e-mailom već postoji.");
            }
            var exsistingUser =await dbContext.Users.FirstOrDefaultAsync(x=>x.Id == user.Id);

            if (exsistingUser != null) 
            {
                dbContext.Entry(exsistingUser).CurrentValues.SetValues(user);
                await dbContext.SaveChangesAsync();
                return user;
            }
            return null;
        }
    }
}
