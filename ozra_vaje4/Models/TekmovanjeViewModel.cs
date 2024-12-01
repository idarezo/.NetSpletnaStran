using System;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;

namespace ozra_vaje4.Models
{
    public class TekmovanjeViewModel
    {

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("ime_tekmovanja")]
        public string ime_tekmovanja { get; set; }

        [JsonPropertyName("sumOfAges")]
        public string sumOfAges { get; set; }
        [JsonPropertyName("drzava")]
        public string drzava { get; set; }
        [JsonPropertyName("leto_izvedbe")]
        public string leto_izvedbe { get; set; }
        [JsonPropertyName("averageSwimTime")]
        public string AverageSwimTime { get; set; }

        [JsonPropertyName("rezultati")]
        public List<RezultatiViewModel> rezultati { get; set; }

        [JsonPropertyName("results")]

        public List<RezultatiEnaViewModel> results { get; set; }

        public TekmovanjeViewModel()
        {
            
        }
    }
}
