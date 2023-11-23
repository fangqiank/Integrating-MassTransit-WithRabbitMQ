using FomularOne.DataService;
using FomularOne.Entities;
using FomularOne.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace FomularOne.Repository
{
    public class AchievementRepository : GenericRepository<Achievement>, IAchievementRepository
    {
        public AchievementRepository(AppDbContext context, ILogger logger) : base(context, logger)
        {
        }

        public async Task<Achievement?> GetDriverAchievementAsync(Guid driverId)
        {
            try
            {
                return await _dbSet.FirstOrDefaultAsync(x => x.DriverId == driverId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{repo} GetDriverAchievement function error", typeof(AchievementRepository));
                throw;
            }
        }

        public override async Task<IEnumerable<Achievement>> All()
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
                _logger.LogError(ex, "{repo} All function error", typeof(AchievementRepository));
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
                _logger.LogError(ex, "{repo} Delete function error", typeof(AchievementRepository));
                throw;
            }
        }

        public override async Task<bool> Update(Achievement updAchievement)
        {
            try
            {
                var result = await _dbSet.FirstOrDefaultAsync(x => x.Id == updAchievement.Id);

                if (result == null)
                    return false;

                result.UpdatededDate = DateTime.UtcNow;
                result.FastestLap = updAchievement.FastestLap;
                result.PolePosition = updAchievement.PolePosition;
                result.WorldChampionship = updAchievement.WorldChampionship;
                result.RaceWins = updAchievement.RaceWins;

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{repo} Update function error", typeof(AchievementRepository));
                throw;
            }
        }
    }
}
