using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ozra_vaje4.Models;

namespace ozra_vaje4.Controllers
{
    public class AccountController : Controller
    {

        private readonly SignInManager<UserViewModel> _signInManager;
        private readonly UserManager<UserViewModel> _userManager;
        private readonly PasswordHasher<UserViewModel> _hasher;
        private readonly ILogger<AccountController> _logger;


        public AccountController(SignInManager<UserViewModel> signInManager, SignInManager<UserViewModel> signIn, UserManager<UserViewModel> userManager, ILogger<AccountController> logger)
        {
            this._signInManager = signIn;
            this._userManager = userManager;
            this._hasher = new PasswordHasher<UserViewModel>();
            _logger = logger;
        }

        public IActionResult Account()
        {


            return View("LogIn");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogInUser(AccountViewModel Input)
        {

            if (ModelState.IsValid)
            {
                var user = _userManager.FindByNameAsync(Input.Email).Result;
                if (user != null)
                {
                    if (_hasher.VerifyHashedPassword(user, user.PasswordHash, Input.Password) == PasswordVerificationResult.Success)
                    {

                        var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, true, lockoutOnFailure: false);

                        if (result.Succeeded)
                        {
                            var user1 = await _userManager.GetUserAsync(User);
                            if (Input.Email == "admin@afmin.com")
                            {
                                Console.WriteLine("Test");
                                return RedirectToAction("Index", "Home");
                            }
                            if (User.IsInRole("Administrator"))
                            {
                               
                                Console.WriteLine("test");
                                return RedirectToAction("Index", "Home");
                            }
                            else if (User.IsInRole("Uporabnik"))
                            {
                                Console.WriteLine("test2");

                                return RedirectToAction("Index", "Home");
                            }
                            else
                            {
                                Console.WriteLine("test22");

                               
                                return RedirectToAction("Index", "Home");

                            }



                        }

                        return View("LogIn", Input);

                    }

                    ModelState.AddModelError("enaslov", "Invalid mail or password for real");
                    return View("LogIn", Input);
                }
                ModelState.AddModelError("enaslov", "not found in database.");
                return View("LogIn", Input);
            }
            ModelState.AddModelError("enaslov", "Model state not valid.");
            foreach (var modelStateEntry in ModelState.Values)
            {
                foreach (var error in modelStateEntry.Errors)
                {
                    Console.WriteLine($"Model state error: {error.ErrorMessage}");
                }
            }
            return View("LogIn", Input);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
