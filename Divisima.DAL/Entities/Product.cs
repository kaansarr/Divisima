using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Divisima.DAL.Entities
{
    [Table("Product")]
    public class Product
    {
        public int ID { get; set; }

        [StringLength(100), Column(TypeName = "varchar(100)"), Required(ErrorMessage = "Ürün adı boş geçilemez..."), Display(Name = "Ürün Adı")]
        public string Name { get; set; }

        [StringLength(250), Column(TypeName = "varchar(250)"), Display(Name = "Ürün Açıklaması")]
        public string Description { get; set; }

        [Column(TypeName = "text"), Display(Name = "Ürün Detayı")]
        public string Detail { get; set; }

        [Column(TypeName = "decimal(18,2)"), Display(Name = "Satış Fiyatı")]
        public decimal Price { get; set; }

        [Display(Name = "Stok Miktarı")]
        public int Stock { get; set; }

        [Display(Name = "Markası")]
        public int? BrandID { get; set; }
        public Brand Brand { get; set; }

        [Display(Name = "Görüntülenme Sırası")]
        public int DisplayIndex { get; set; }

        [Display(Name = "Ürün Resimleri")]
        public ICollection<ProductPicture> ProductPictures { get; set; }

        public ICollection<ProductCategory> ProductCategories { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
