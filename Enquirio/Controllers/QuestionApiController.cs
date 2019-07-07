using System.Threading.Tasks;
using Enquirio.Data;
using Enquirio.Models;
using Microsoft.AspNetCore.Mvc;

namespace Enquirio.Controllers {

    [Route("/api/")]
    public class QuestionApiController : Controller {

        private readonly IRepositoryEnq _repo;

        public QuestionApiController(IRepositoryEnq repo) => _repo = repo;

        [HttpPost("createAnswer/{questionId}")]
        [Consumes("application/json")]
        public async Task<IActionResult> CreateAnswer([FromBody] Answer answer, int questionId) {
            if (InvalidEntity(answer)) {
                return BadRequest();
            }

            if (await EntityNotFound(questionId)) {
                return NotFound();
            }

            answer.QuestionId = questionId;

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
            if (await EntityNotFound(aId : answerId)) {
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
        private async Task<bool> EntityNotFound(int? qId = null, int? aId = null) {
            bool errResult = true;

            if (qId != null) {
                errResult = !await _repo.ExistsAsync<Question>(q => q.Id == qId);

                if (errResult) return errResult;
            }

            if (aId != null) {
                return !await _repo.ExistsAsync<Answer>(a => a.Id == aId);
            }

            return errResult;
        }
            
            
    }
}
