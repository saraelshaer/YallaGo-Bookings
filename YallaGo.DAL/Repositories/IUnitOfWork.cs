
using YallaGo.DAL.Models;

namespace YallaGo.DAL.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<User> UsersRepo { get; }
        IGenericRepository<Booking> BookingRepo { get; }
        IGenericRepository<Destination> DestinationsRepo { get; }
        IGenericRepository<Tour> ToursRepo { get; }
        
        Task<int> CompleteAsync();
    }
}
