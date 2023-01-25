using System.ComponentModel.DataAnnotations;

namespace TechBlogApp.DTOs
{
    public class RegisterDTO
    {
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Lastname is required")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        [Compare("Password",ErrorMessage ="Password and password repeat is not match")]
        public string PasswordRepeat { get; set; }


    }
}
