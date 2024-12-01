using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ozra_vaje4.Models
{
    public class UserViewModel : IdentityUser
    {
        [Required(ErrorMessage = "Ime je obvezno polje.")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Dovoljene so le črke.")]
        public string ime { get; set; }

        [Required(ErrorMessage = "Priimek je obvezno polje.")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Dovoljene so le črke.")]
        public string priimek { get; set; }

      

        [Required(ErrorMessage = "E-naslov je obvezno polje.")]
        public string eNaslov { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Geslo je obvezno polje.")]
       [DataType(DataType.Password)]
        public string geslo1 { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Potrditev gesla je obvezno polje.")]
        [Compare("geslo1", ErrorMessage = "Gesla se ne ujemata.")]
        [DataType(DataType.Password)]
        public string geslo2 { get; set; }
    }
}
