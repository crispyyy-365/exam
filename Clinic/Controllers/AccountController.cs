using Clinic.Models;
using Clinic.Utilities.Enums;
using Clinic.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVm registerVm)
        {
            if (!ModelState.IsValid)
            {
                return View(registerVm);
            }
            AppUser user = new AppUser()
            {
                UserName = registerVm.Username,
                Email = registerVm.Email,
            };
            IdentityResult resut = await _userManager.CreateAsync(user, registerVm.Password);
            if (!resut.Succeeded)
            {
                foreach (IdentityError error in resut.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                    return View(registerVm);
                }
            }
            await _userManager.AddToRoleAsync(user, UserRole.Member.ToString());
            await _signInManager.SignInAsync(user, false);
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVm loginVm, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(loginVm);
            }
            AppUser? appUser = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == loginVm.UsernameOrEmail || x.UserName == loginVm.UsernameOrEmail);
            if (appUser == null)
            {
                ModelState.AddModelError(string.Empty, "Email, Username or password is incorrect");
                return View(loginVm);
            }
            var result = await _signInManager.PasswordSignInAsync(appUser, loginVm.Password, true, true);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Email, Username or password is incorrect");
                return View(loginVm);
            }
            if (result.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "Account is locked, try again later");
                return View(loginVm);
            }
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
        public async Task<IActionResult> CreateRoles()
        {
            foreach(UserRole role in Enum.GetValues(typeof(UserRole)))
            {
                if(! await _roleManager.RoleExistsAsync(role.ToString()))
                {
                    await _roleManager.CreateAsync(new IdentityRole { Name = role.ToString() });
                }
            }
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
