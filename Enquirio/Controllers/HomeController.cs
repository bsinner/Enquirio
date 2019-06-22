using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Enquirio.Data;
using Enquirio.Models;
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
