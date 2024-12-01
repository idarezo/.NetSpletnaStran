namespace ozra_vaje4.Models
{
    public class TekmovanjeDrugoViewModel
    {
        public string Id { get; set; }
        public string ime_tekmovanja { get; set; }

        public string drzava { get; set; }
        public string leto_izvedbe { get; set; }
        public string averageSwimTime { get; set; }
        public List<RezultatiDrugoViewModel> results { get; set; }
        public List<RezultatiDrugoViewModel> rezultati { get; set; }

        public TekmovanjeDrugoViewModel()
        {

        }
    }
}
