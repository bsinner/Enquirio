using System.Threading.Tasks;
using Enquirio.Data;
using Enquirio.Models;
using Microsoft.AspNetCore.Mvc;

namespace Enquirio.Controllers {

    public class HomeController : Controller {

        private readonly IRepositoryEnq _repo;

        public HomeController(IRepositoryEnq repo) {
            _repo = repo;
        }

        public async Task<IActionResult> Index() {
            var questions = await _repo.GetAllAsync<Question>();

            return View(questions);
        }

    }
}
