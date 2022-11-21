using FiorEllo.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations.Schema;

namespace FiorEllo.Areas.Admin.ViewModels.Product
{
    public class ProductCreateViewModel
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public string? PhotoName { get; set; }
        [NotMapped]
        public IFormFile? Photo { get; set; }
        [NotMapped]
        public List<IFormFile>? Photos { get; set; }
        public int Quantity { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public List<SelectListItem>? Categories { get; set; }
        [Required]
        public string? Description { get; set; }
        public string Weight { get; set; }
        public string Dimensions { get; set; }
        public List<ProductPhoto>? ProductPhotos { get; set; }
    }
}
