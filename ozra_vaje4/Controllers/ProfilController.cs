using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ozra_vaje4.Models;

namespace ozra_vaje4.Controllers
{
    public class ProfilController : Controller
    {
        private readonly ApplicationDbContext _DbContext;
        private readonly PasswordHasher<UserViewModel> _hasher;
        private readonly SignInManager<UserViewModel> _signInManager;
        private readonly UserManager<UserViewModel> _userManager;
        public ProfilController(ApplicationDbContext db, SignInManager<UserViewModel> signIn, UserManager<UserViewModel> userManager)
        {

            this._DbContext = db;
            this._signInManager = signIn;
            this._userManager = userManager;
            this._hasher = new PasswordHasher<UserViewModel>();

        }
        public async Task<IActionResult> Profil()
        {
            var user = await _userManager.GetUserAsync(User);

            return View(user);
        }

        public async Task<IActionResult> Show()
        {
            var user = await _userManager.GetUserAsync(User);

            return View("~/Views/Profil/RegistracijaSpremeniProfil.cshtml", user);

        }

        [HttpPost]
        public async Task<IActionResult> posodobiPodatke(UserViewModel uporabnik, string id)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user != null)
                {

                    user.ime = uporabnik.ime;
                    user.priimek = uporabnik.priimek;
                    user.eNaslov = uporabnik.eNaslov;
                    user.geslo1 = uporabnik.geslo1;
                    user.geslo2 = uporabnik.geslo2;
                    var result = await _userManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                        await _DbContext.SaveChangesAsync();
                        return RedirectToAction("Profil");
                    }
                    else
                    {

                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "User not found.");
                }
            }
            return View("RegistracijaSpremeniProfil", uporabnik);
        }


    }
}
