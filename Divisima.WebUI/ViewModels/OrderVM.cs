using Divisima.DAL.Entities;
using Divisima.WebUI.Models;

namespace Divisima.WebUI.ViewModels
{
	public class OrderVM
    {
		public List<Cart> Carts { get; set; }
		public Order Order { get; set; }
		public IEnumerable<City>Cities { get; set; }
	}
}
