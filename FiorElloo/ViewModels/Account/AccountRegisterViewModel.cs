using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;

namespace FiorEllo.ViewModels.Account
{
    public class AccountRegisterViewModel
    {
        [Microsoft.Build.Framework.Required, MaxLength(50)]
        public string FullName { get; set; }
        [Microsoft.Build.Framework.Required, MaxLength(50), DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Microsoft.Build.Framework.Required, MaxLength(50), DataType(DataType.Password)]
        public string PassWord { get; set; }
        [Microsoft.Build.Framework.Required, MaxLength(50), DataType(DataType.Password), Display(Name = "Confirm PassWord"), Compare(nameof(PassWord))]
        public string ConfirmPassWord { get; set; }
        [Microsoft.Build.Framework.Required, MaxLength(50)]
        public string UserName { get; set; }
    }
}
