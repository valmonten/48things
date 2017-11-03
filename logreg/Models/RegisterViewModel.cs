using System.ComponentModel.DataAnnotations;

namespace logreg.Models
{
    public class RegisterViewModel 
    {
        [Required, MinLength(2)]
        public string name {get;set;}
        [Required, MinLength(2)]
        public string alias {get;set;}
        [Required, MinLength(2)]
        public string email {get;set;}
        [Required, MinLength(8)]
        public string password {get;set;}
        [Compare("password", ErrorMessage = "Passwords must match")]
        public string confirmpassword {get;set;}
    }
}