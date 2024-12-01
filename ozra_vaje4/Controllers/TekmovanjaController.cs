using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ozra_vaje4.Models;
using Newtonsoft.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using Microsoft.CodeAnalysis.CSharp;

namespace ozra_vaje4.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TekmovanjaController : Controller
    {
     
       

        public List<TekmovanjeViewModel> vsaTekmovanja = new List<TekmovanjeViewModel>();
        private readonly HttpClient _httpClient;
        private readonly ApplicationDbContext _DbContext;
        public TekmovanjaController(HttpClient httpClient, ApplicationDbContext db)
        {
            _httpClient = httpClient;
            this._DbContext = db;
        }

        

        [HttpGet("all")]
        public async Task<IEnumerable<TekmovanjeViewModel>> GetTekmovanja()
        {
            var response = await _httpClient.GetAsync("https://localhost:7179/tekmovanja");

            response.EnsureSuccessStatusCode();

            using (var responseStream = await response.Content.ReadAsStreamAsync())
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var tekmovanja = await System.Text.Json.JsonSerializer.DeserializeAsync<IEnumerable<TekmovanjeViewModel>>(responseStream, options);
                vsaTekmovanja = tekmovanja.ToList();

                foreach (var x in vsaTekmovanja)
                {
                    x.Id = Guid.NewGuid().ToString();
                }


               return tekmovanja;
            }
        }

        [HttpGet("allDrugo")]
        public async Task<IEnumerable<TekmovanjeDrugoViewModel>> GetTekmovanjaDrugo()
        {
            var response = await _httpClient.GetAsync("https://localhost:7179/tekmovanjeDrugeVrste");

            response.EnsureSuccessStatusCode();

            using (var responseStream = await response.Content.ReadAsStreamAsync())
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var tekmovanja = await System.Text.Json.JsonSerializer.DeserializeAsync<IEnumerable<TekmovanjeDrugoViewModel>>(responseStream, options);

                foreach (var tekmovanje in tekmovanja)
                {
                    if (!int.TryParse(tekmovanje.leto_izvedbe,out var value))
                    {
                        tekmovanje.leto_izvedbe = "2016";

                    }
                }
               

                return tekmovanja;
            }
        }

        [HttpGet("komentar/{ime}/{drzava}/{leto}")]
        public async Task<IActionResult> dodajKomentar(string ime, string drzava, string leto)
        {

          
            Console.WriteLine(ime);
            Console.WriteLine(drzava);
            Console.WriteLine(leto);
            

            var response = await _httpClient.GetAsync("https://localhost:7179/tekmovanja");

            response.EnsureSuccessStatusCode();

            using (var responseStream = await response.Content.ReadAsStreamAsync())
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var tekmovanja = await System.Text.Json.JsonSerializer.DeserializeAsync<IEnumerable<TekmovanjeViewModel>>(responseStream, options);
               

              

                TekmovanjeViewModel najdenoTekmovanje = tekmovanja.FirstOrDefault(x => x.ime_tekmovanja == ime && x.drzava == drzava && x.leto_izvedbe == leto);
           

                return View("~/Views/TekmovanjaIsci/PrikaziPodrobnosti.cshtml", najdenoTekmovanje);
            }
        }


        [HttpGet("TekmovanjaRezultati/{ime}/{leto}/{drzava}")]

        public async Task<object> GetTekmovanjaRezultati(string ime,string drzava,string leto)
        {
            object result = new object();
            var response = await _httpClient.GetAsync($"https://localhost:7179/tekVsa/{ime}/{leto}/{drzava}");

            response.EnsureSuccessStatusCode();

            using (var responseStream = await response.Content.ReadAsStreamAsync())
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var tekmovanja = await System.Text.Json.JsonSerializer.DeserializeAsync<TekmovanjeViewModel>(responseStream, options);
                if (tekmovanja != null)
                {
                    return tekmovanja;
                }


               
            }

            var response2 = await _httpClient.GetAsync($"https://localhost:7179/tekVsaDrugo/{ime}/{leto}/{drzava}");

            response2.EnsureSuccessStatusCode();

            using (var responseStream = await response2.Content.ReadAsStreamAsync())
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };



                var tekmovanjaDruga = await System.Text.Json.JsonSerializer.DeserializeAsync<TekmovanjeDrugoViewModel>(responseStream, options);
                result = tekmovanjaDruga;

            }

            return result;
           

        }

        [HttpGet("tekmovanjaIzracun/{ime}/{drzava}/{leto}")]
        public async Task<IActionResult> Iracuni(string ime, string drzava, string leto)
        {


            Console.WriteLine(ime);
            Console.WriteLine(drzava);
            Console.WriteLine(leto);


            var response = await _httpClient.GetAsync("https://localhost:7179/tekmovanja");

            response.EnsureSuccessStatusCode();

            using (var responseStream = await response.Content.ReadAsStreamAsync())
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var tekmovanja = await System.Text.Json.JsonSerializer.DeserializeAsync<IEnumerable<TekmovanjeViewModel>>(responseStream, options);




                TekmovanjeViewModel najdenoTekmovanje = tekmovanja.FirstOrDefault(x => x.ime_tekmovanja == ime && x.drzava == drzava && x.leto_izvedbe == leto);


                return View("~/Views/TekmovanjaIsci/PrikaziPodrobnosti.cshtml", najdenoTekmovanje);
            }


        }

        [HttpGet("komentarDrugoTekmovanje/{ime}/{drzava}/{leto}")]
        public async Task<IActionResult> dodajKomentarDrugo(string ime, string drzava, string leto)
        {



            var response = await _httpClient.GetAsync("https://localhost:7179/tekmovanjeDrugeVrste");

            response.EnsureSuccessStatusCode();

            using (var responseStream = await response.Content.ReadAsStreamAsync())
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var tekmovanja = await System.Text.Json.JsonSerializer.DeserializeAsync<IEnumerable<TekmovanjeDrugoViewModel>>(responseStream, options);




                TekmovanjeDrugoViewModel najdenoTekmovanje = tekmovanja.FirstOrDefault(x => x.ime_tekmovanja == ime && x.drzava == drzava && x.leto_izvedbe == leto);


                return View("~/Views/TekmovanjaIsci/PrikaziPodrobnostiDrugo.cshtml", najdenoTekmovanje);
            }
        }




        [HttpGet("komentarNovi/{idTekmovanja}/{komentar}")]

        public async Task<IActionResult> dodajKomentarNovi(string idTekmovanja, string komentar)
        {

            CommentViewModel komentarNovi = new CommentViewModel
            {
                TekmovanjeId = idTekmovanja,
                CommentText = komentar,
                UserId = ""
            };

            _DbContext.komentarji.Add(komentarNovi);
            _DbContext.SaveChanges();

            return Ok(komentarNovi);

        }


        [HttpGet("pridobiKomentar/{idTekmovanja}")]

        public async Task<IEnumerable<CommentViewModel>> pridobiKomentarje(string idTekmovanja)
        {

            var komentar = _DbContext.komentarji.Where(x=>x.TekmovanjeId==idTekmovanja).ToList();
            Console.WriteLine("jja");
            return komentar;

        }



        [HttpGet("specificnoEna/{id}")]
        public async Task<IEnumerable<object>> GetSpecifcnoTekmovanje(string id)
        {
            Console.WriteLine(id);
            var response = await _httpClient.GetAsync("https://localhost:7179/tekmovanja");

            response.EnsureSuccessStatusCode();

            using (var responseStream = await response.Content.ReadAsStreamAsync())
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var tekmovanja = await System.Text.Json.JsonSerializer.DeserializeAsync<IEnumerable<TekmovanjeViewModel>>(responseStream, options);

                List<TekmovanjeViewModel> tekmovanjaPoDrzavi = new List<TekmovanjeViewModel>();
                tekmovanjaPoDrzavi = tekmovanja.Where(x=>x.ime_tekmovanja==id).ToList();

                if (tekmovanjaPoDrzavi.Count() > 0)
                {
                    return tekmovanjaPoDrzavi;
                }
               

            }

            Console.WriteLine(id);
            var responseTwo = await _httpClient.GetAsync("https://localhost:7179/tekmovanjeDrugeVrste");

            response.EnsureSuccessStatusCode();

            using (var responseStream = await responseTwo.Content.ReadAsStreamAsync())
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var tekmovanjaDruga = await System.Text.Json.JsonSerializer.DeserializeAsync<IEnumerable<TekmovanjeDrugoViewModel>>(responseStream, options);

                List<TekmovanjeDrugoViewModel> tekmovanjaPoDrzavi = new List<TekmovanjeDrugoViewModel>();
                tekmovanjaPoDrzavi = tekmovanjaDruga.Where(x => x.ime_tekmovanja == id).ToList();
                List<object> tekmovanjaDrugaNova= test(tekmovanjaPoDrzavi);

                return tekmovanjaDrugaNova;
            }


        }

        [HttpGet("specificnaDrzava/{datumVnos}")]
        public async Task<IEnumerable<object>> GetSpecPoDrzavi(string datumVnos)
        {
            List<object> vsatek = new List<object>();
            var response = await _httpClient.GetAsync($"https://localhost:7179/tekmovanjeCas/{datumVnos}");

            response.EnsureSuccessStatusCode();

            using (var responseStream = await response.Content.ReadAsStreamAsync())
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var tekmovanja = await System.Text.Json.JsonSerializer.DeserializeAsync<IEnumerable<TekmovanjeViewModel>>(responseStream, options);

                vsatek.AddRange(tekmovanja);
               
            }

            var respons2 = await _httpClient.GetAsync($"https://localhost:7179/tekmovanjeCasDrugo/{datumVnos}");

            respons2.EnsureSuccessStatusCode();

            using (var responseStream = await respons2.Content.ReadAsStreamAsync())
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var tekmovanjaDruga= await System.Text.Json.JsonSerializer.DeserializeAsync<IEnumerable<TekmovanjeDrugoViewModel>>(responseStream, options);

                vsatek.AddRange(tekmovanjaDruga);

                
            }

            return vsatek;

        }

        

        [HttpGet("tekmovanjaDrz/{drzava}")]
        public async Task<IEnumerable<object>> GetSpecPoDrz(string drzava)
        {
            List<object> vsatek = new List<object>();


            var response = await _httpClient.GetAsync($"https://localhost:7179/tekmovanjaDrzava/{drzava}");

            response.EnsureSuccessStatusCode();

            using (var responseStream = await response.Content.ReadAsStreamAsync())
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var tekmovanja = await System.Text.Json.JsonSerializer.DeserializeAsync<IEnumerable<TekmovanjeViewModel>>(responseStream, options);

                vsatek.AddRange(tekmovanja);

            }


            var response2 = await _httpClient.GetAsync($"https://localhost:7179/tekmovanjaDrzavaDrugo/{drzava}");

            response2.EnsureSuccessStatusCode();

            using (var responseStream = await response2.Content.ReadAsStreamAsync())
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var tekmovanjaDruga = await System.Text.Json.JsonSerializer.DeserializeAsync<IEnumerable<TekmovanjeDrugoViewModel>>(responseStream, options);
                List<TekmovanjeDrugoViewModel> tekmovanjaDrugaList = tekmovanjaDruga.ToList();
              
                List<object> drzaveTek = test(tekmovanjaDrugaList);

                
                vsatek.AddRange(drzaveTek);
               


            }

            return vsatek;

        }


        [HttpGet("UpdateProfil")]
        public async Task<IActionResult> UpdateProfil()
        {
            return View("~/Views/Registracija/Registracija.cshtml");
        }

        [HttpGet("prviGraf/{ime}")]
        public async Task<Dictionary<string, int>> prikaziPrviGraf(string ime)
        {
            Console.WriteLine(ime);
            Dictionary<string, int> stats = new Dictionary<string, int>();
            var response = await _httpClient.GetAsync("https://localhost:7179/tekmovanja");

            response.EnsureSuccessStatusCode();

            using (var responseStream = await response.Content.ReadAsStreamAsync())
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var tekmovanja = await System.Text.Json.JsonSerializer.DeserializeAsync<IEnumerable<TekmovanjeViewModel>>(responseStream, options);
                var filtriranaTek = tekmovanja.Where(x=>x.ime_tekmovanja == ime);

                foreach (var tekmovanje in filtriranaTek)
                {
                    if (stats.ContainsKey(tekmovanje.drzava))
                    {
                        stats[tekmovanje.drzava]++;
                    }
                    else
                    {
                        stats[tekmovanje.drzava] = 1;
                    }
                }

            }

            var response2 = await _httpClient.GetAsync("https://localhost:7179/tekmovanjeDrugeVrste");

            response2.EnsureSuccessStatusCode();

            using (var responseStream = await response2.Content.ReadAsStreamAsync())
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var tekmovanja2= await System.Text.Json.JsonSerializer.DeserializeAsync<IEnumerable<TekmovanjeDrugoViewModel>>(responseStream, options);
                var filtriranaTek2 = tekmovanja2.Where(x => x.ime_tekmovanja == ime);

                foreach (var tekmovanje2 in filtriranaTek2)
                {
                    if (stats.ContainsKey(tekmovanje2.drzava))
                    {
                        stats[tekmovanje2.drzava]++;
                    }
                    else
                    {
                        stats[tekmovanje2.drzava] = 1;
                    }
                }

            }



            return stats;
        }


        
        [HttpGet("DrugiGraf/{imeEna}/{imeDva}")]
        public async Task<Dictionary<string, int>> prikaziPrviDrugiGraf(string imeEna, string imeDva)
        {
            Dictionary<string, int> stats1 = new Dictionary<string, int>();

            var response = await _httpClient.GetAsync("https://localhost:7179/tekmovanja");

            response.EnsureSuccessStatusCode();

            using (var responseStream = await response.Content.ReadAsStreamAsync())
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var tekmovanja = await System.Text.Json.JsonSerializer.DeserializeAsync<IEnumerable<TekmovanjeViewModel>>(responseStream, options);

                foreach (var tekmovanje in tekmovanja)
                {
                    if (!int.TryParse(tekmovanje.leto_izvedbe, out var value))
                    {
                        tekmovanje.leto_izvedbe = "2016";

                    }
                }


                var filtriranaTek = tekmovanja.Where(x => x.ime_tekmovanja == imeEna);

                foreach (var tekmovanje in filtriranaTek)
                {
                    if (stats1.ContainsKey(tekmovanje.leto_izvedbe))
                    {
                        stats1[tekmovanje.leto_izvedbe]++;
                    }
                    else
                    {
                        stats1[tekmovanje.leto_izvedbe] = 1;
                    }
                }


            }

            var response2 = await _httpClient.GetAsync("https://localhost:7179/tekmovanjeDrugeVrste");

            response2.EnsureSuccessStatusCode();

            using (var responseStream = await response2.Content.ReadAsStreamAsync())
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var tekmovanja2 = await System.Text.Json.JsonSerializer.DeserializeAsync<IEnumerable<TekmovanjeDrugoViewModel>>(responseStream, options);
                var filtriranaTek2 = tekmovanja2.Where(x => x.ime_tekmovanja == imeEna);

                foreach (var tekmovanje2 in filtriranaTek2)
                {
                    if (stats1.ContainsKey(tekmovanje2.leto_izvedbe))
                    {
                        stats1[tekmovanje2.leto_izvedbe]++;
                    }
                    else
                    {
                        stats1[tekmovanje2.leto_izvedbe] = 1;
                    }
                }

            }
            return  stats1;
        }


        [HttpGet("TretjiGraf/{letnica}")]
        public async Task<Dictionary<string, int>> prikaziPrviTretjiGraf(string letnica)
        {
            Dictionary<string, int> stats1 = new Dictionary<string, int>();

            var response = await _httpClient.GetAsync("https://localhost:7179/tekmovanja");

            response.EnsureSuccessStatusCode();

            using (var responseStream = await response.Content.ReadAsStreamAsync())
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var tekmovanja = await System.Text.Json.JsonSerializer.DeserializeAsync<IEnumerable<TekmovanjeViewModel>>(responseStream, options);
                var filtriranaTek = tekmovanja.Where(x => x.leto_izvedbe == letnica);

                foreach (var tekmovanje in filtriranaTek)
                {
                    if (stats1.ContainsKey(tekmovanje.ime_tekmovanja))
                    {
                        stats1[tekmovanje.ime_tekmovanja]++;
                    }
                    else
                    {
                        stats1[tekmovanje.ime_tekmovanja] = 1;
                    }
                }

            }

            var response2 = await _httpClient.GetAsync("https://localhost:7179/tekmovanjeDrugeVrste");

            response2.EnsureSuccessStatusCode();

            using (var responseStream = await response2.Content.ReadAsStreamAsync())
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var tekmovanja2 = await System.Text.Json.JsonSerializer.DeserializeAsync<IEnumerable<TekmovanjeDrugoViewModel>>(responseStream, options);
                var filtriranaTek2 = tekmovanja2.Where(x => x.leto_izvedbe == letnica);

                foreach (var tekmovanje2 in filtriranaTek2)
                {
                    if (stats1.ContainsKey(tekmovanje2.ime_tekmovanja))
                    {
                        stats1[tekmovanje2.ime_tekmovanja]++;
                    }
                    else
                    {
                        stats1[tekmovanje2.ime_tekmovanja] = 1;
                    }
                }

            }
            return stats1;

        }

        public List<object> test(List<TekmovanjeDrugoViewModel> tekmovanja)
        {
            List<object> tekmovanjaFilter = new List<object>();
            foreach (var tekmovanje in tekmovanja)
            {
                if (tekmovanje.leto_izvedbe == "")
                {
                    tekmovanje.leto_izvedbe = "2015";
                }
                else if (tekmovanje.leto_izvedbe == "Oregon" || tekmovanje.leto_izvedbe == "Florida" || tekmovanje.leto_izvedbe == "Virginia")
                {
                    tekmovanje.leto_izvedbe = "2017";
                }

                tekmovanjaFilter.Add(tekmovanje);
            }

            return tekmovanjaFilter;
        }

        [HttpGet("NajTekmeci")]
        public async Task<IEnumerable<string>> TopMaratonci()
        {
            List<string> topMaratonci = new List<string>();
            var response = await _httpClient.GetAsync("https://localhost:7179/tekmovanjaZRezultatiNormalna");
            response.EnsureSuccessStatusCode();

            using (var responseStream = await response.Content.ReadAsStreamAsync())
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var tekmovanja2 = await System.Text.Json.JsonSerializer.DeserializeAsync<IEnumerable<string>>(responseStream, options);
                topMaratonci = tekmovanja2.ToList();

            }

            return topMaratonci;

        }




    }

}
