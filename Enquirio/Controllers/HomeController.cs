using System.Threading.Tasks;
using Enquirio.Data;
using Enquirio.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace Enquirio.Controllers {

    public class HomeController : Controller {

        private readonly IWebHostEnvironment _env;

        public HomeController(IWebHostEnvironment env) => _env = env;

        public IActionResult Index() {
            if (!_env.IsDevelopment()) {
                return File("~/devenv.html", "text/html");
            }

            return File("~/index.html", "text/html");
        }

    }
}
