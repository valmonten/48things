using System.ComponentModel.DataAnnotations;

namespace logreg.Models
{
    public class RegisterViewModel 
    {
        [Required]
        [MinLength(2)]
        public string fname {get;set;}
        [Required]
        [MinLength(2)]
        public string lname {get;set;}
        [Required]
        [MinLength(2)]
        public string email {get;set;}
        [Required]
        [MinLength(6)]
        public string password {get;set;}
        [Compare("password", ErrorMessage = "Passwords must match")]
        public string confirmpassword {get;set;}
    }
}