using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Enquirio.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Enquirio.Controllers {

    [Route("/api/")]
    public class AuthApiController : Controller {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private const string Prefix = "Guest_";

        public AuthApiController(UserManager<IdentityUser> userManager
                , SignInManager<IdentityUser> signInManager) {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        [Consumes("application/json")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model) {
            await _signInManager.SignOutAsync();

            if (IsModelValid(model, false)) {

                var user = !string.IsNullOrEmpty(model.Email)
                    ? await _userManager.FindByEmailAsync(model.Email)
                    : await _userManager.FindByNameAsync(model.Username);

                if (user != null) {

                    var result = await _signInManager
                        .PasswordSignInAsync(user, model.Password, true, false);

                    if (result.Succeeded) {
                        return Ok();
                    }
                }

                return Unauthorized();
            }

            return BadRequest();
        }

        [AllowAnonymous]
        [HttpPost("signUp")]
        public async Task<IActionResult> SignUp([FromBody] LoginViewModel model) {
            await _signInManager.SignOutAsync();

            if (IsModelValid(model, true)) {

                var user = new IdentityUser {
                    UserName = model.Username, Email = model.Email
                };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded) {
                    await _signInManager
                        .PasswordSignInAsync(user, model.Password, true, false);
                    return Ok();
                }

                return Unauthorized(result.Errors);
            }

            return BadRequest();
        }

        [AllowAnonymous]
        [HttpPost("signUpAn")]
        public async Task<IActionResult> SignUpAnonymous() {
            await _signInManager.SignOutAsync();
            var pw = GetPw();
            var user = new IdentityUser();

            await _userManager.CreateAsync(user, pw);
            await _signInManager
                .PasswordSignInAsync(user, pw, false, false);

            return Ok();
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout() {
            await _signInManager.SignOutAsync();
            return Ok();
        }

        // Get if login model is valid, if bool all is true
        // username and email are required
        private bool IsModelValid(LoginViewModel model, bool all) {
            bool IsE(string s) => string.IsNullOrEmpty(s);

            if (IsE(model.Password)
                || all && (IsE(model.Username) || IsE(model.Email))
                || !all && IsE(model.Username) && IsE(model.Email)) {

                return false;
            }

            return true;
        }

        // Create a password for an anonymous account by adding
        // a capital letter, non alphanumeric, and number to a
        // UUID 
        private string GetPw() {
            var ranges = new Dictionary<string, int[]> {
                ["capital"] = new [] { 65, 91 }
                , ["symbol"] = new [] { 33, 48 }
                , ["numeric"] = new [] { 48, 58 }
            };

            var pass = Guid.NewGuid().ToString();
            var l = pass.Length + 1;
            var random = new Random();

            foreach (var r in ranges) {
                var point = random.Next(0, l);
                var toAdd = (char) random.Next(r.Value[0], r.Value[1]);

                pass = pass.Insert(point, toAdd.ToString());
            }

            return pass;
        }
    }
}
