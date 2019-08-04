using Pieshop.Interfaces;
using Pieshop.Models;
using System.Threading.Tasks;

namespace Pieshop.Repositories
{
    public class FeedbackRepository : BaseRepository, IFeedbackRepository
    {

        public FeedbackRepository(AppDbContext appDbContext) : base(appDbContext) { }

        public async Task<int> AddFeedback(Feedback item)
        {
            await _appDbContext.Feedbacks.AddAsync(item);
            return await _appDbContext.SaveChangesAsync();
        }
    }
}
