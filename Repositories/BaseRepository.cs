using Pieshop.Models;

namespace Pieshop.Repositories
{
    public abstract class BaseRepository
    {

        protected readonly AppDbContext _appDbContext;

        public BaseRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
    }
}
