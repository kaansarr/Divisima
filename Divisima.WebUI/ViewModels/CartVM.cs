using Divisima.DAL.Entities;
using Divisima.WebUI.Models;

namespace Divisima.WebUI.ViewModels
{
	public class CartVM
	{
		public List<Cart> Carts { get; set; }
		public IEnumerable<Product> Products { get; set; }
	}
}
