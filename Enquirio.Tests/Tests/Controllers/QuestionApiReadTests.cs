using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Enquirio.Controllers;
using Enquirio.Data;
using Enquirio.Models;
using Enquirio.Tests.TestData;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Enquirio.Tests.Tests.Controllers {
    public class QuestionApiReadTests : ApiTestUtil {

        private const int PageLength = QuestionApiController.PageLength;

        [Fact]
        public async Task TestGetQuestionsPageOne() {
            // Arrange
            var repo = new Mock<IRepositoryEnq>();
            repo.Setup(r => r.GetAllAsync<Question>(
                    null, Desc(), true, 0, PageLength, null)
            ).ReturnsAsync(
                    QuestionData.TestQuestions().ToList().GetRange(0, PageLength)
            );
            
            var controller = new QuestionApiController(repo.Object);

            // Act 
            var result = await controller.GetQuestions(1);
            await controller.GetQuestions(0);
            await controller.GetQuestions(-1);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsType<List<Question>>(result.Value);
            Assert.Equal(PageLength, (result.Value as List<Question>).Count);

            repo.Verify(r => r.GetAllAsync<Question>
                    (null, Desc(), true, 0, PageLength, null), Times.Exactly(3)
            );
            repo.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task TestGetQuestionsPastPageOne() {
            // Arrange
            var repo = new Mock<IRepositoryEnq>();
            var questions = QuestionData.TestQuestions().ToList();
            var maxPage = MaxPage(questions.Count);
            var midPage = 2;
            var midSkip = midPage * PageLength - PageLength;
            var maxSkip = maxPage * PageLength - PageLength;

            SetUpPastOneRepo(repo, midSkip, maxSkip, questions);

            var controller = new QuestionApiController(repo.Object);

            // Act 
            var result = await controller.GetQuestions(midPage);
            await controller.GetQuestions(maxPage + 1000);

            // Assert
            Assert.Equal(PageLength, (result.Value as List<Question>).Count);

            repo.Verify(r => r.GetAllAsync
                    (null, Desc(), true, It.IsIn(new [] { midSkip, maxSkip }), PageLength, null)
                    , Times.Exactly(2)
           );
            repo.Verify(r => r.GetCountAsync<Question>(null), Times.Exactly(2));
            repo.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task TestGetQuestion() {
            // Arrange
            var repo = new Mock<IRepositoryEnq>();
            var question = new Question();
            var arr = new [] { "Answers" };
            int id = 1;
            int notFoundId = 999;
            var invalidId = -10;
            var zeroId = 0;

            repo.Setup(r => r.GetByIdAsync<Question>(id, arr, null))
                .ReturnsAsync(question);

            repo.Setup(r => r.GetByIdAsync<Question>(notFoundId, arr, null))
                .Returns(Task.FromResult<Question>(null));

            var controller = new QuestionApiController(repo.Object);

            // Act
            var result = await controller.GetQuestion(id);
            var notFoundResult = await controller.GetQuestion(notFoundId);
            await controller.GetQuestion(zeroId);
            await controller.GetQuestion(invalidId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsType<NotFoundResult>(notFoundResult);

            Assert.Equal(question, (result as OkObjectResult).Value);

            repo.Verify(r => r.GetByIdAsync<Question>
                (It.IsIn(new [] { id, notFoundId }), arr, null), Times.Exactly(2));
            repo.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task TestGetCount() {
            // Arrange
            var repo = new Mock<IRepositoryEnq>();
            int count = 100;
            int maxPage = MaxPage(count);

            repo.Setup(r => r.GetCountAsync<Question>(null))
                .ReturnsAsync(count);

            var controller = new QuestionApiController(repo.Object);

            // Act
            var result = await controller.GetMaxPage();

            // Assert 
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(maxPage, result.Value as int?);

            repo.Verify(r => r.GetCountAsync<Question>(null), Times.Once);
            repo.VerifyNoOtherCalls();
        }

        [Fact]
        public void HttpVerbTests() {
            Assert.True(HasAttribute(nameof(QuestionApiController.GetQuestions)
                    , typeof(HttpGetAttribute)));
            Assert.True(HasAttribute(nameof(QuestionApiController.GetMaxPage)
                    , typeof(HttpGetAttribute)));
            Assert.True(HasAttribute(nameof(QuestionApiController.GetQuestion)
                    , typeof(HttpGetAttribute)));
        }

        private Expression<Func<Question, IComparable>> Desc() 
            => It.IsAny<Expression<Func<Question, IComparable>>>();

        private int MaxPage(int items) 
            => (int) Math.Ceiling(items / (double) PageLength);

        private void SetUpPastOneRepo(Mock<IRepositoryEnq> repo, int midSkip
                , int maxSkip, List<Question> questions) {

            repo.Setup(r => r.GetAllAsync(null, Desc(), true, midSkip, PageLength, null))
                .ReturnsAsync(questions.GetRange(midSkip, PageLength));

            repo.Setup(r => r.GetAllAsync(null, Desc(), true, maxSkip, PageLength, null))
                .ReturnsAsync(new List<Question>());

            repo.Setup(r => r.GetCountAsync<Question>(null))
                .ReturnsAsync(questions.Count);
        }
    }
}
