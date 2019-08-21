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

            var maxPage = (int) Math.Floor(
                await _repo.GetCountAsync<Question>() / (double) PageLength);

            return Ok(await GetPage(p > maxPage ? maxPage : p));
        }

        [HttpPost("createAnswer")]
        [Consumes("application/json")]
        public async Task<IActionResult> CreateAnswer([FromBody] Answer answer) {
            if (InvalidEntity(answer)) {
                return BadRequest();
            }

            if (await EntityNotFound(qId : answer.QuestionId)) {
                return NotFound();
            }

            _repo.Create(answer);
            await _repo.SaveAsync();

            return Ok(answer.Id.ToString());
        }

        [HttpPut("editAnswer")]
        [Consumes("application/json")]
        public async Task<IActionResult> EditAnswer([FromBody] Answer edited) {
            if (InvalidEntity(edited, false)) {
                return BadRequest();
            }

            var answer = await _repo.GetByIdAsync<Answer>(edited.Id);

            if (answer == null) {
                return NotFound();
            }

            answer.Title = edited.Title;
            answer.Contents = edited.Contents;

            _repo.Update(answer);
            await _repo.SaveAsync();

            return Ok();
        }

        [HttpDelete("deleteAnswer/{answerId}")]
        public async Task<IActionResult> DeleteAnswer(int answerId) {
            if (await EntityNotFound(answerId)) {
                return NotFound();
            }

            _repo.DeleteById<Answer>(answerId);
            await _repo.SaveAsync();

            return Ok();
        }

        [HttpPut("editQuestion")]
        [Consumes("application/json")]
        public async Task<IActionResult> EditQuestion([FromBody] Question edited) {
            if (InvalidEntity(edited, false)) {
                return BadRequest();
            }

            var question = await _repo.GetByIdAsync<Question>(edited.Id);

            if (question == null) {
                return NotFound();
            }

            question.Contents = edited.Contents;
            question.Title = edited.Title;

            _repo.Update(question);
            await _repo.SaveAsync();

            return Ok();
        }

        // Return true if entity is invalid, zeroId allows entities with ID 0
        private bool InvalidEntity(IPost post, bool zeroId = true) =>
            string.IsNullOrEmpty(post.Title)
            || string.IsNullOrEmpty(post.Contents)
            || (!zeroId && post.Id == 0);

        // Search db for entity, return true if it isn't found
        private async Task<bool> EntityNotFound(int? aId = null, int? qId = null) {
            bool notFound = true;

            if (aId != null) {
                notFound = !await _repo.ExistsAsync<Answer>(a => a.Id == aId);
            }

            if (qId != null) {
                notFound = !await _repo.ExistsAsync<Question>(q => q.Id == qId);
            }

            return notFound;
        }

        // Get one page of questions
        private async Task<List<Question>> GetPage(int page) {
            return await _repo.GetAllAsync<Question>(
                orderBy: q => q.Created, sortDesc: true
                , take: PageLength, skip: PageLength * page - PageLength
            );
        }
    }
}
