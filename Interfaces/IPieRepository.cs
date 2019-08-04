using Pieshop.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pieshop.Interfaces
{
    public interface IPieRepository
    {
        Task<IEnumerable<Pie>> GetAllPies();

        Task<Pie> GetById(int id);
    }
}
