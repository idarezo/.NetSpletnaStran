using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ozra_vaje4.Models;
using System.Text.Json;

namespace ozra_vaje4.Controllers
{
    public class CustomeIdController : Controller
    {
        private readonly HttpClient _httpClient;
        public CustomeIdController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public IActionResult CustomeId()
        {


            return View();

        }

        [HttpPost]
        public async Task<IActionResult> shraniVsePodatkeId(CustomeIdModelView uporabnik)
        {
            if (ModelState.IsValid)
            {
                TempData["drzava"] = uporabnik.drzava;
                TempData["leto"] = uporabnik.leto_izvedbe;
                TempData["ime"] = uporabnik.ime_tekmovanje;


                var viewModel = new CustomeIdModelView
                {

                    ime_tekmovanje = TempData["ime"] as string,
                    drzava = TempData["drzava"] as string,
                    leto_izvedbe = TempData["leto"] as string,

                };


                var response = await _httpClient.GetAsync("https://localhost:7179/tekmovanja");
                using (var responseStream = await response.Content.ReadAsStreamAsync())
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    var tekmovanje = await System.Text.Json.JsonSerializer.DeserializeAsync<TekmovanjeViewModel>(responseStream, options);





                }

                return View("CustomeId");
            }
            return View("CustomeId");

        }
    }
}
