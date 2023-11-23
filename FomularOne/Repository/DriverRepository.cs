using FomularOne.DataService;
using FomularOne.Entities;
using FomularOne.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace FomularOne.Repository
{
    public class DriverRepository : GenericRepository<Driver>, IDriverRepository
    {
        public DriverRepository(AppDbContext context, ILogger logger) : base(context, logger)
        {

        }

        public override async Task<IEnumerable<Driver>> All()
        {
            try
            {
                return await _dbSet
                    .Where(x => x.Status == 1)
                    .AsNoTracking()
                    .AsSplitQuery()
                    .OrderBy(x => x.AddedDate)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{repo} All function error", typeof(DriverRepository));
                throw;
            }
        }

        public override async Task<bool> Delete(Guid id)
        {
            try
            {
                var result = await _dbSet.FirstOrDefaultAsync(x => x.Id == id);

                if (result == null)
                    return false;

                result.Status = 0;
                result.UpdatededDate = DateTime.UtcNow;

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{repo} Delete function error", typeof(DriverRepository));
                throw;
            }
        }

        public override async Task<bool> Update(Driver updDriver)
        {
            try
            {
                var result = await _dbSet.FirstOrDefaultAsync(x => x.Id == updDriver.Id);

                if (result == null)
                    return false;

                result.UpdatededDate = DateTime.UtcNow;
                result.DriverNumber = updDriver.DriverNumber;
                result.FirstName = updDriver.FirstName;
                result.LastName = updDriver.LastName;
                result.Dob = updDriver.Dob;

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{repo} Update function error", typeof(DriverRepository));
                throw;
            }
        }

    }
}
