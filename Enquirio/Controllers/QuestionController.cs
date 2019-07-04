using System.Threading.Tasks;
using Enquirio.Data;
using Enquirio.Models;
using Microsoft.AspNetCore.Mvc;

namespace Enquirio.Controllers {
    public class QuestionController : Controller {

        private readonly IRepositoryEnq _repo;

        public QuestionController(IRepositoryEnq repo) => _repo = repo;

        // Show question page or not found page
        public async Task<ViewResult> ViewQuestion(string id) {
            Question question = null;
            QuestionViewModel model = new QuestionViewModel();

            if (id != null) {
                question = await _repo
                    .GetByIdAsync<Question>(id, new [] { "Answers" });
            }

            if (question == null) {
                return View("NotFound");
            }

            model.Question = question;

            return View(model);
        }

        // Create form
        public ViewResult Create() => View();
        
        // Submit question, redirect to it's page
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<RedirectToRouteResult> Create(Question question) {
            _repo.Create(question);

            await _repo.SaveAsync();

            return RedirectToRoute("question", new { id = question.Id });
        }

        // Submit answer, redirect to question page
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<RedirectToRouteResult> CreateAnswer(Answer answer) {
            _repo.Create(answer);

            await _repo.SaveAsync();

            return RedirectToRoute("question", new { id = answer.QuestionId });
        }

        // Delete question
        public async Task<RedirectToRouteResult> Delete(int id) {
            _repo.DeleteById<Question>(id);

            await _repo.SaveAsync();

            return RedirectToRoute("default");
        }
    }
}
