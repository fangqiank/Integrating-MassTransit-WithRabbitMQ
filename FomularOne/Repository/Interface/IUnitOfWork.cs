using static FomularOne.Repository.DriverRepository;

namespace FomularOne.Repository.Interface
{
    public interface IUnitOfWork
    {
        IDriverRepository Drivers { get; }
        IAchievementRepository Achievements { get; }

        Task<bool> CompleteAsync();
    }
}
