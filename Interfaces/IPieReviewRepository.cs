using Pieshop.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pieshop.Interfaces
{
    public interface IPieReviewRepository
    {
        Task<int> AddPieReview(PieReview pieReview);

        Task<IEnumerable<PieReview>> GetReviewsForPie(int pieId);

    }
}
