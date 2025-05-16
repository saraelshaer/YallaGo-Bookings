
using YallaGo.DAL.Models;

namespace YallaGo.DAL.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<User> UsersRepo { get; }
        IGenericRepository<Booking> BookingRepo { get; }
        IGenericRepository<Destination> DestinationRepo { get; }
        IGenericRepository<Tour> TourRepo { get; }
        
        Task<int> CompleteAsync();
    }
}
