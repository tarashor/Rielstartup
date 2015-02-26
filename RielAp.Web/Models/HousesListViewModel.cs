using RielAp.Domain.Models;
using RielAp.Web.Models.Filter;

namespace RielAp.Web.Models
{
    public class HousesListViewModel:HousesViewModel
    {
        public PageInfo Page { get; set; }
    }
}