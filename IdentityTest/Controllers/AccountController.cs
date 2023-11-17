using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityTest.Data;
using IdentityTest.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityTest.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        public AccountController(SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        //get
        public IActionResult Login()
        {
            return View();
        }




        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginInfo)
        {
            var result = await _signInManager.PasswordSignInAsync(loginInfo.Username,
                loginInfo.Password, loginInfo.RememberMe, false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Failed to login");
            }
            return View(loginInfo);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }



        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel userEnteredData)
        {
            if (ModelState.IsValid)
            {
                User newUser = new User();
                newUser.UserName = userEnteredData.UserName;
                newUser.FirstName = userEnteredData.FirstName;
                newUser.LastName = userEnteredData.LastName;
                newUser.Email = userEnteredData.Email;
                newUser.PhoneNumber = userEnteredData.Phone;

                var result = await _userManager.CreateAsync(newUser, userEnteredData.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(newUser, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(userEnteredData);
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }



    }


}
