using FiorEllo.Models;

namespace FiorEllo.ViewModels.Blog
{
    public class BlogIndexViewModel
    {
        public List<BlogMasonry> BlogMasonries { get; set; }
        public int Page { get; set; } = 1;
        public int Take { get; set; } = 2;
        public int PageCount { get; set; }
    }
}
