using System.Threading.Tasks;
using Enquirio.Data;
using Enquirio.Models;
using Microsoft.AspNetCore.Mvc;

namespace Enquirio.Controllers {
    public class QuestionController : Controller {

        private readonly IRepositoryEnq _repo;

        public QuestionController(IRepositoryEnq repo) => _repo = repo;

        public async Task<ViewResult> ViewQuestion(string id) {
            Question question = null;

            if (id != null) {
                question = await _repo
                    .GetByIdAsync<Question>(id, navPropCollections : new [] { "Answers" });
            }

            return View(question);
        }

        public ViewResult Create() => View();
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Question question) {
            _repo.Create(question);

            await _repo.SaveAsync();

            return RedirectToRoute("question", new { id = question.Id });
        }

    }
}
