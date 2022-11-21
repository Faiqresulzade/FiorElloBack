using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace FiorEllo.Areas.Admin.ViewModels.Product
{
    public class ProductIndexViewModel
    {
        public List<Models.Product> Products { get; set; }
        #region Filter
        public string? Title { get; set; }
        public double? MinPrice { get; set; }
        public double? MaxPrice { get; set; }
        public List<SelectListItem> Categories { get; set; }
        [Display(Name ="Category")]
        public int? CategoryId { get; set; }
        public int? MinQuantity { get; set; }
        public int? MaxQuantity { get; set; }
        [Display(Name ="Create at Start")]
        public DateTime? CreateAtStart { get; set; }
        [Display(Name = "Create at End")]
        public DateTime? CreateAtEnd { get; set; }
        #endregion
    }
}
