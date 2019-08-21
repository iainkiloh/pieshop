using Microsoft.EntityFrameworkCore;
using Pieshop.Interfaces;
using Pieshop.Models;
using System.Collections.Generic;
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

        public async Task<IEnumerable<Pie>> GetPiesOfTheWeek()
        {
            //implement categories of pies later
                // return _appDbContext.Pies.Include(c => c.Category).Where(p => p.IsPieOfTheWeek);
                return await _appDbContext.Pies.ToListAsync();  
        }


        public async Task<Pie> GetById(int id)
        {
            //get pies with al reviews
            //TODO - limit reviews 
            return await _appDbContext.Pies.Include(inc => inc.PieReviews).FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
