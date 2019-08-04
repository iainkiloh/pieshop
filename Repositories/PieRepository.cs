using Microsoft.EntityFrameworkCore;
using Pieshop.Interfaces;
using Pieshop.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pieshop.Repositories
{
    public class PieRepository : BaseRepository, IPieRepository
    {

        public PieRepository(AppDbContext appDbContext) : base(appDbContext) { }


        public async Task<IEnumerable<Pie>> GetAllPies()
        {
            return await _appDbContext.Pies.ToListAsync();
        }

        public async Task<Pie> GetById(int id)
        {
            return await _appDbContext.Pies.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
