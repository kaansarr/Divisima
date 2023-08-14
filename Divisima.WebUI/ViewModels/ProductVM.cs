using Divisima.DAL.Entities;

namespace Divisima.WebUI.ViewModels
{
	public class ProductVM
	{
		public Product Product { get; set; }
		public IEnumerable<Product> RelatedProducts { get; set; }
	}
}
