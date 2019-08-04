using Pieshop.Models;
using System.Threading.Tasks;

namespace Pieshop.Interfaces
{
    public interface IFeedbackRepository
    {
        Task<int> AddFeedback(Feedback item);
    }
}
