using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MTournamentsApp.Models;

namespace MTournamentsApp.Controllers
{
    // Controller handling user account functionalities
    public class AccountController : Controller
    {
        private SignInManager<User> _signInManager;
        private UserManager<User> _userManager;

        // Constructor injecting SignInManager and UserManager dependencies
        public AccountController(SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        // GET: /Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            // Display registration form
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            // If registration data is valid
            if (ModelState.IsValid)
            {
                // Create a new user
                var user = new User { UserName = model.Username };
                var result = await _userManager.CreateAsync(user, model.Password);

                // If user creation is successful
                if (result.Succeeded)
                {
                    // Sign in the user and redirect to home page
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // If user creation fails, add errors to ModelState
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            // If ModelState is not valid, return to registration form with errors
            return View(model);
        }

        // POST: /Account/LogOut
        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            // Sign out the user and redirect to home page
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        // GET: /Account/LogIn
        [HttpGet]
        public IActionResult LogIn(string returnURL = "")
        {
            // Display login form
            var model = new LoginViewModel { ReturnUrl = returnURL };
            return View(model);
        }

        // POST: /Account/LogIn
        [HttpPost]
        public async Task<IActionResult> LogIn(LoginViewModel model)
        {
            // If login data is valid
            if (ModelState.IsValid)
            {
                // Attempt to sign in the user with provided credentials
                var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password,
                            isPersistent: model.RememberMe, lockoutOnFailure: false);

                // If login is successful
                if (result.Succeeded)
                {
                    // Redirect to the requested URL or home page
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }

            // If login fails, add error to ModelState and return to login form
            ModelState.AddModelError("", "Invalid username/password.");
            return View(model);
        }

        // GET: /Account/AccessDenied
        [HttpGet]
        public ViewResult AccessDenied()
        {
            // Display access denied page
            return View();
        }
    }
}
