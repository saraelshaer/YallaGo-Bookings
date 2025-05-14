using YallaGo.DAL.Models;

namespace YallaGo.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public IGenericRepository<User> UsersRepo { get; }
        public IGenericRepository<Booking> BookingRepo { get; }
        public IGenericRepository<Destination> DestinationsRepo { get; }
        public IGenericRepository<Tour> ToursRepo { get; }


        public UnitOfWork(AppDbContext context)
        {
            _context = context;

            UsersRepo = new GenericRepository<User>(_context);
            BookingRepo = new GenericRepository<Booking>(_context);
            DestinationsRepo = new GenericRepository<Destination>(_context);
            ToursRepo = new GenericRepository<Tour>(_context);

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