using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Enquirio.Data;
using Enquirio.Models;
using Microsoft.AspNetCore.Mvc;

namespace Enquirio.Controllers {

    [Route("/api/")]
    public class QuestionApiController : Controller {

        private readonly IRepositoryEnq _repo;
        public const int PageLength = 15;

        public QuestionApiController(IRepositoryEnq repo) => _repo = repo;

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
        
        [HttpPost("createAnswer")]
        [Consumes("application/json")]
        public async Task<IActionResult> CreateAnswer([FromBody] Answer answer) {
            if (InvalidEntity(answer)) {
                return BadRequest();
            }

            if (!await _repo.ExistsAsync<Question>
                    (q => q.Id == answer.QuestionId)) {
                return NotFound();
            }

            _repo.Create(answer);
            await _repo.SaveAsync();

            return Ok(answer.Id.ToString());
        }

        [HttpDelete("deleteAnswer/{id}")]
        public async Task<IActionResult> DeleteAnswer(int id) {
            return await DeleteEntity<Answer>(id);
        }

        [HttpDelete("deleteQuestion/{id}")]
        public async Task<IActionResult> DeleteQuestion(int id) {
            return await DeleteEntity<Question>(id);
        }

        [HttpPut("editQuestion")]
        [Consumes("application/json")]
        public async Task<IActionResult> EditQuestion([FromBody] Question edited) {
            return await EditPost(edited);
        }

        [HttpPut("editAnswer")]
        [Consumes("application/json")]
        public async Task<IActionResult> EditAnswer([FromBody] Answer edited) {
            return await EditPost(edited);
        }

        // Generic method to delete an entity
        private async Task<IActionResult> DeleteEntity<T>(int id) 
                where T : class, IEntity {

            if (id < 1) {
                return BadRequest();
            }

            if (!await _repo.ExistsAsync<T>(t => t.Id == id)) {
                return NotFound();
            }

            _repo.DeleteById<T>(id);
            await _repo.SaveAsync();

            return Ok();
        }

        // Generic method to edit the title and contents of a text post
        private async Task<IActionResult> EditPost<T>(T edited) 
                where T : class, IPost {

            if (InvalidEntity(edited, false)) {
                return BadRequest();
            }

            var original = await _repo.GetByIdAsync<T>(edited.Id);

            if (original == null) {
                return NotFound();
            }

            original.Title = edited.Title;
            original.Contents = edited.Contents;

            _repo.Update(original);
            await _repo.SaveAsync();

            return Ok();
        }

        // Return true if entity is invalid, zeroId allows entities with ID 0 for
        // when an entity is created without an ID and the database assigns an ID
        private bool InvalidEntity(IPost post, bool zeroId = true) =>
            string.IsNullOrEmpty(post.Title)
            || string.IsNullOrEmpty(post.Contents)
            || (!zeroId && post.Id == 0)
            || (post.Id < 0);


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
