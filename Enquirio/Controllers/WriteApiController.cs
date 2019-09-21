using System.Security.Claims;
using System.Threading.Tasks;
using Enquirio.Data;
using Enquirio.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Enquirio.Controllers {

    [Authorize]
    [Route("/api/")]
    public class WriteApiController : Controller {
        
        private readonly IRepositoryEnq _repo;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IAuthorizationService _authService;
        private enum IdType { ZERO, ANY, NON_ZERO };

        public WriteApiController(IRepositoryEnq repo
                , IHttpContextAccessor httpContext
                , IAuthorizationService authService) {
            _repo = repo;
            _httpContext = httpContext;
            _authService = authService;
        }

        [HttpPost("createAnswer")]
        [Consumes("application/json")]
        public async Task<IActionResult> CreateAnswer([FromBody] Answer answer) {
            if (IsCreateInvalid(answer)) {
                return BadRequest();
            }

            if (!await _repo.ExistsAsync<Question>
                    (q => q.Id == answer.QuestionId)) {
                return NotFound();
            }

            return await CreatePost(answer);
        }

        [HttpPost("createQuestion")]
        [Consumes("application/json")]
        public async Task<IActionResult> CreateQuestion([FromBody] Question question) {
            if (IsCreateInvalid(question)) {
                return BadRequest();
            }

            return await CreatePost(question);
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

//            var serviceResult = await _authService.AuthorizeAsync(
//                _httpContext.HttpContext.User
//                , await _repo.GetByIdAsync<T>(id)
//                , "AuthorOnly");

            _repo.DeleteById<T>(id);
            await _repo.SaveAsync();

            return Ok();
        }

        // Generic method to edit the title and contents of a text post
        private async Task<IActionResult> EditPost<T>(T edited) 
                where T : class, IPost {

            if (InvalidEntity(edited, IdType.NON_ZERO)) {
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

        // Generic create IPost
        private async Task<IActionResult> CreatePost<T>(T textPost) 
            where T : class, IPost {

            textPost.UserId = GetUserId();
            _repo.Create(textPost);
            
            await _repo.SaveAsync();
            return Ok(textPost);
        }

        // Return true if entity is invalid, IdType specifies if id
        // should be zero, greater than 0, or any non negative number
        private bool InvalidEntity(IPost post, IdType idType) 
            => string.IsNullOrEmpty(post.Title)
            || string.IsNullOrEmpty(post.Contents)
            || (idType == IdType.ZERO && post.Id != 0)
            || (idType == IdType.NON_ZERO && post.Id == 0)
            || (post.Id < 0);

        // Get if a post to create has an id greater than 0 
        // or empty fields
        private bool IsCreateInvalid(IPost post) 
            => InvalidEntity(post, IdType.ZERO);

        private string GetUserId()
            => _httpContext.HttpContext
                .User.FindFirst(ClaimTypes.NameIdentifier).Value;
    }
}
