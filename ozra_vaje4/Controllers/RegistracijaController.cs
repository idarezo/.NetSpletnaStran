using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ozra_vaje4.Models;

namespace ozra_vaje4.Controllers
{
    public class RegistracijaController : Controller
    {

        private readonly ApplicationDbContext _DbContext;
        private readonly PasswordHasher<UserViewModel> _hasher;
        private readonly SignInManager<UserViewModel> _signInManager;
        private readonly UserManager<UserViewModel> _userManager;
        public RegistracijaController(ApplicationDbContext db, SignInManager<UserViewModel> signIn, UserManager<UserViewModel> userManager)
        {

            this._DbContext = db;
            this._signInManager = signIn;
            this._userManager = userManager;
            this._hasher = new PasswordHasher<UserViewModel>();

        }

        [HttpPost]
        public async Task<IActionResult> shraniVsePodatke(UserViewModel uporabnik)
        {
            if (ModelState.IsValid)
            {

                if (_userManager.FindByNameAsync(uporabnik.eNaslov).Result != null)
                {
                    ModelState.AddModelError("eNaslov", "Email already in use.");
                    return View("Registracija");
                }


                TempData["ime"] = uporabnik.ime;
                TempData["priimek"] = uporabnik.priimek;               
                TempData["eNaslov"] = uporabnik.eNaslov;
                TempData["geslo"] = uporabnik.geslo1;
                TempData["gesloPotrditev"] = uporabnik.geslo2;

                var viewModel = new UserViewModel
                {
                    UserName = uporabnik.eNaslov,
                    Email = uporabnik.eNaslov,
                    ime = TempData["ime"] as string,
                    priimek = TempData["priimek"] as string,              
                    eNaslov = TempData["eNaslov"] as string,
                    geslo1 = TempData["geslo"] as string,
                    geslo2 = TempData["gesloPotrditev"] as string
                };

                var result = await _userManager.CreateAsync(viewModel, uporabnik.geslo1);
                var result2 = await _userManager.AddToRoleAsync(viewModel, "Uporabnik");

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                if (result.Succeeded && result2.Succeeded)
                {

                    return View("~/Views/Account/LogIn.cshtml");
                }


                await _DbContext.uporabniki.AddAsync(viewModel);
                await _DbContext.SaveChangesAsync();


            }

            return View("Registracija", uporabnik);
        }


        public IActionResult Registracija()
        {


            return View();

        }
    }
}
