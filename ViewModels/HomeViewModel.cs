using Pieshop.Models;
using System.Collections.Generic;

namespace Pieshop.ViewModels
{
    public class HomeViewModel
    {
        public string Title { get; set; }
        public List<Pie> Pies { get; set; }
    }
}
