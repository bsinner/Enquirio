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

            if (isValid(model)) {

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
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("signUpAn")]
        public async Task<IActionResult> SignUpAnonymous() {
            return Ok();
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout() {
            await _signInManager.SignOutAsync();
            return Ok();
        }

        private bool isValid(LoginViewModel model) 
            => !string.IsNullOrEmpty(model.Password) 
               && (!string.IsNullOrEmpty(model.Username) 
                   || !string.IsNullOrEmpty(model.Email));

    }
}
