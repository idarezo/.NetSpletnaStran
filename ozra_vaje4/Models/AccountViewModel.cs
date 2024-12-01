using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ozra_vaje4.Models
{
    public class AccountViewModel : IdentityUser
    {
        [Required(ErrorMessage = "E-naslov je obvezno polje.")]
     
        public string Email { get; set; }


        [Required(ErrorMessage = "Geslo je obvezno polje.")]

        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
