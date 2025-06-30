using System.ComponentModel.DataAnnotations;

namespace Identity_User_Roles.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Email must required")]
        public string? email { get; set; }



        [Required(ErrorMessage = "password must required")]
        [Compare("Confirmpassword",ErrorMessage = "Password does not match.")]
        public string? password { get; set; }



        [Required(ErrorMessage = "password must required")]
        public string? Confirmpassword { get; set; }

    }
}
