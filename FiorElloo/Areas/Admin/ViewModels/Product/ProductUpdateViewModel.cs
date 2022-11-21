using FiorEllo.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FiorEllo.Areas.Admin.ViewModels.Product
{
    public class ProductUpdateViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public int Price { get; set; }
        public string? PhotoName { get; set; }
        public IFormFile? Photo { get; set; }
        public List<IFormFile>? Photos { get; set; }
        public int Quantity { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public List<SelectListItem>? Category { get; set; }
        [Required]
        public string Description { get; set; }
        public string Weight { get; set; }
        public string Dimensions { get; set; }
        public List<ProductPhoto>? ProductPhotos { get; set; }
    }
}
