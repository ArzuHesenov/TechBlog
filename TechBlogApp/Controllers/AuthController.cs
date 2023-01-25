﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TechBlogApp.DTOs;
using TechBlogApp.Models;

namespace TechBlogApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            var checkEmail = await _userManager.FindByEmailAsync(loginDTO.Email);
            if (checkEmail==null)
            {
            return View();
            }

            Microsoft.AspNetCore.Identity.SignInResult result=await _signInManager.PasswordSignInAsync(checkEmail, loginDTO.Password,false,false);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {

            if (!ModelState.IsValid)
            {
                return View(registerDTO);
            }

            var checkUserEmail= await _userManager.FindByEmailAsync(registerDTO.Email);
            if (checkUserEmail !=null)
            {
                return View();
            }

            User newUser = new()
            {
                UserName = registerDTO.Email,
                FirstName = registerDTO.FirstName,
                LastName = registerDTO.LastName,
                Email = registerDTO.Email,
                AboutAuthor = "/",
                PhotoUrl="/uploads/avatar.png"
            };
            var result = await _userManager.CreateAsync(newUser, registerDTO.Password);

            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Login));
            }
            return View();
        }
    }
}
