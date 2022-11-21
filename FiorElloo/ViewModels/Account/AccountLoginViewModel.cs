using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace FiorEllo.ViewModels.Account
{
    public class AccountLoginViewModel
    {
        [Required]
        public string UserName { get; set; }
        [Required,DataType(DataType.Password)]
        public string PassWord { get; set; }
        public string? ReturnUrl { get; set; }
    }
}
