using Microsoft.AspNetCore.Mvc;

using System.ComponentModel.DataAnnotations;

namespace AutoPartsShop.Models.Product
{
    public class ProductIndexVM
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Product Name")]
        public string ProductName { get; set; }

        public int BrandId { get; set; }

        [Display(Name = "Brand")]
        public string BrandName { get; set; }

        public int CategoryId { get; set; }

        [Display(Name = "Category")]
        public string CategoryName { get; set; }

        [Display(Name = "Picture")]
        public string Picture { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; } = null!;

        [Display(Name = "Quantity")]
        public int Quantity { get; set; }

        [Display(Name = "Price")]
        public decimal Price { get; set; }

        [Display(Name = "Discount")]
        public decimal Discount { get; set; }

        public decimal FinalPrice => Price - (Price * Discount / 100);
    }
}