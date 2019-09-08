using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Enquirio.Data;
using Enquirio.Models;
using Microsoft.AspNetCore.Mvc;

namespace Enquirio.Controllers {

    [Route("/api/")]
    public class ReadonlyApiController : Controller {

        public const int PageLength = 15;
        private readonly IRepositoryEnq _repo;

        public ReadonlyApiController(IRepositoryEnq repo) {
            _repo = repo;
        }

        // Get a page of questions
        [HttpGet("questions")]
        [Produces("application/json")]
        public async Task<OkObjectResult> GetQuestions(int p) {
            if (p <= 1) {
                // Pass 1 instead of p in case the page is 0 or negative
                return Ok(await GetPage(1));
            }

            var maxPage = await MaxPage();

            return Ok(await GetPage(p > maxPage ? maxPage : p));
        }

        [HttpGet("question/{id}")]
        [Produces("application/json")]
        public async Task<IActionResult> GetQuestion(int id) {
            if (id >= 1) {
                var question = await _repo
                    .GetByIdAsync<Question>(id, new[] {"Answers"});

                if (question != null) {
                    return Ok(question);
                }
            }

            return NotFound();
        }

        [HttpGet("qMaxPage")]
        public async Task<OkObjectResult> GetMaxPage() 
            => Ok(await MaxPage());

        // Get one page of questions
        private async Task<List<Question>> GetPage(int page) {
            return await _repo.GetAllAsync<Question>(
                orderBy: q => q.Created, sortDesc: true
                , skip: PageLength * page - PageLength, take: PageLength);
        }

        // Get number of last page of questions
        private async Task<int> MaxPage() => (int) Math.Ceiling(
            await _repo.GetCountAsync<Question>() / (double) PageLength
        );
    }
}
