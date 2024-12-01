using System.ComponentModel.DataAnnotations;

namespace ozra_vaje4.Models
{
    public class CustomeIdModelView
    {
        [Required]
        public string ime_tekmovanje { get; set; }

        [Required]
        public string drzava { get; set; }

        [Required]
        public string leto_izvedbe { get; set; }

       

    }
}
