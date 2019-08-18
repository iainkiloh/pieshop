using Microsoft.EntityFrameworkCore;
using Pieshop.Interfaces;
using Pieshop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pieshop.Repositories
{
    public class PieReviewRepository : IPieReviewRepository
    {
        private readonly AppDbContext _appDbContext;

        public PieReviewRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IEnumerable<PieReview>> GetReviewsForPie(int pieId)
        {
            return await _appDbContext.PieReviews.Where(p => p.Pie.Id == pieId).ToListAsync();
        }

        public async Task<int> AddPieReview(PieReview pieReview)
        {
            await _appDbContext.PieReviews.AddAsync(pieReview);
            return await _appDbContext.SaveChangesAsync();
        }


    }
}
