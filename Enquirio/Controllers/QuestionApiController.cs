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

        [HttpPut("editAnswer/{questionId}")]
        [Consumes("application/json")]
        public async Task<IActionResult> EditAnswer([FromBody] Answer answer, int questionId) {
            if (InvalidEntity(answer)) {
                return BadRequest();
            }

            if (await EntityNotFound(questionId, answer.Id)) {
                return NotFound();
            }

            _repo.Update(answer);
            await _repo.SaveAsync();

            return Ok();
        }

        private bool InvalidEntity(IPost post) =>
            string.IsNullOrEmpty(post.Title)
            || string.IsNullOrEmpty(post.Contents)
            || post.Id == 0;

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
