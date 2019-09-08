using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Enquirio.Data;
using Enquirio.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Enquirio.Controllers {

    [Route("/api/")]
    public class QuestionApiController : Controller {
        
        private readonly IRepositoryEnq _repo;
        private readonly IHttpContextAccessor _httpContext;
        private enum IdType { ZERO, ANY, NON_ZERO };
        public const int PageLength = 15;

        public QuestionApiController(IRepositoryEnq repo
                , IHttpContextAccessor httpContext) {
            _repo = repo;
            _httpContext = httpContext;
        }

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
        private bool InvalidEntity(IPost post, IdType idType) =>
            string.IsNullOrEmpty(post.Title)
            || string.IsNullOrEmpty(post.Contents)
            || (idType == IdType.ZERO && post.Id != 0)
            || (idType == IdType.NON_ZERO && post.Id == 0)
            || (post.Id < 0);

        // Get if a post to create has an id greater than 0 
        // or empty fields
        private bool IsCreateInvalid(IPost post) 
            => InvalidEntity(post, IdType.ZERO);

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

        private string GetUserId()
            => _httpContext.HttpContext
                .User.FindFirst(ClaimTypes.NameIdentifier).Value;
    }
}
