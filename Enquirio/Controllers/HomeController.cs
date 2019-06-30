using Enquirio.Data;
using Microsoft.AspNetCore.Mvc;

namespace Enquirio.Controllers {

    public class HomeController : Controller {

        private readonly IRepositoryEnq _repository;

        public HomeController(IRepositoryEnq repository) {
            _repository = repository;
        }

        public IActionResult Index() => View();

    }
}
