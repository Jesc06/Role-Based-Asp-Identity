using System.ComponentModel.DataAnnotations;

namespace Identity_User_Roles.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="Email must required")]
        public string? email { get; set; }


        [Required(ErrorMessage = "password must required")]
        public string? password { get; set; }

    }
}
