using AppointmentShedulerAPI.Data;
using AppointmentShedulerAPI.Models.Domain;
using AppointmentShedulerAPI.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace AppointmentShedulerAPI.Repositories.Implementation
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly ApplicationDbContext dbContext;

        public ServiceRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Service> CreateService(Service service)
        {
            await dbContext.Services.AddAsync(service);
            await dbContext.SaveChangesAsync();
            return service;
        }


        public async Task<IEnumerable<Service>> GetAllAsync()
        {
           return await dbContext.Services.ToListAsync();
        }

        public async Task<Service?> GetByIdAsync(Guid id)
        {
         return  await dbContext.Services.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Service?> UpdateByIdAsync(Service service)
        {
          var exsistingService= await dbContext.Services.FirstOrDefaultAsync(x=>x.Id == service.Id);

            if(exsistingService != null)
            {
                dbContext.Entry(exsistingService).CurrentValues.SetValues(service);
                await dbContext.SaveChangesAsync();
                return service;
            }
            return null;
        }
        public async Task<Service?> DeleteByIdAsync(Guid id)
        {
          var exsistingService= await dbContext.Services.FirstOrDefaultAsync(x=> x.Id == id);
            if(exsistingService == null) { return null; }
            dbContext.Services.Remove(exsistingService);
            await dbContext.SaveChangesAsync();
            return exsistingService;
        }
    }
}
