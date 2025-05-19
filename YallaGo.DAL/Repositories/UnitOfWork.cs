using YallaGo.DAL.Models;

namespace YallaGo.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public IGenericRepository<User> UsersRepo { get; }
        public IGenericRepository<Booking> BookingRepo { get; }
        public IGenericRepository<Destination> DestinationRepo { get; }
        public IGenericRepository<Tour> TourRepo { get; }


        public UnitOfWork(AppDbContext context)
        {
            _context = context;

            UsersRepo = new GenericRepository<User>(_context);
            BookingRepo = new GenericRepository<Booking>(_context);
            DestinationRepo = new GenericRepository<Destination>(_context);
            TourRepo = new GenericRepository<Tour>(_context);


        }


        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }


        public void Dispose()
        {
            _context.Dispose();
        }
    }
}