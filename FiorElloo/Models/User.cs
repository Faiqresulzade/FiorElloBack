using Microsoft.AspNetCore.Identity;

namespace FiorEllo.Models
{
    public class User:IdentityUser
    {
        public string FullName { get; set; }
        public Basket Basket { get; set; }
    }
}
