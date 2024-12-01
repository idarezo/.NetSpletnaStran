using Microsoft.AspNetCore.Mvc;
using ozra_vaje4.Models;
using System.Text.Json;

namespace ozra_vaje4.Controllers
{
    public class TekmovanjaIsciController : Controller
    {
        public List<TekmovanjeViewModel> zacetnetekme = new List<TekmovanjeViewModel>();

        private readonly HttpClient _httpClient;
        public TekmovanjaIsciController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public IActionResult TekmovanjaIsci()
        {

            return View(zacetnetekme);

        }

        public IActionResult TekmovanjaFunkcionalnosti()
        {

            return View("~/Views/TekmovanjaIsci/TekmovanjaFunkcionalnosti.cshtml");

        }

        
        public async Task<IActionResult> PrikaziPodrobnosti(int id)
        {
            Console.WriteLine(id);
            string idString = id.ToString();
            var response = await _httpClient.GetAsync($"https://localhost:7179/tekmovanjeID/{idString}");
            using (var responseStream = await response.Content.ReadAsStreamAsync())
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var tekmovanje = await System.Text.Json.JsonSerializer.DeserializeAsync<TekmovanjeViewModel>(responseStream, options);

                
                return View("~/Views/TekmovanjaIsci/PrikaziPodrobnosti.cshtml", tekmovanje);
            }
            

        }



    }
}
