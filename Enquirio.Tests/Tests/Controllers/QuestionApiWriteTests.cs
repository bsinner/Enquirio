using System;
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
    public class QuestionApiWriteTests : ApiTestUtil {
        
        [Fact]
        public async Task CreateAnswerTest() {
            // Arrange
            var repo = new Mock<IRepositoryEnq>();
            var answer = QuestionData.TestAnswer;

            repo.Setup(r => r.SaveAsync()).Callback(() => answer.Id = 50);
            repo.Setup(r => r.ExistsAsync<Question>
            (It.IsAny<Expression<Func<Question, bool>>>()))
                .ReturnsAsync(true);

            var controller = new QuestionApiController(repo.Object);

            // Act
            var result = await controller.CreateAnswer(answer);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal("50", (result as OkObjectResult).Value);
                
            repo.Verify(r => r.Create(answer), Times.Once);
            repo.Verify(r => r.SaveAsync(), Times.Once);
            repo.Verify(r => r.ExistsAsync<Question>
                (It.IsAny<Expression<Func<Question, bool>>>()), Times.Once);
            repo.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task CreateAnswerErrorTest() {
            // Arrange
            var repo = new Mock<IRepositoryEnq>();
            var answer = QuestionData.TestAnswer;
            var errAnswer = QuestionData.InvalidTestAnswer;

            repo.Setup(r => r.ExistsAsync<Question>(q => q.Id == answer.Id))
                .ReturnsAsync(false);

            var controller = new QuestionApiController(repo.Object);
            // Act
            var notFoundResult = await controller.CreateAnswer(answer);
            var badReqResult = await controller.CreateAnswer(errAnswer);

            // Assert
            Assert.IsType<BadRequestResult>(badReqResult);
            Assert.IsType<NotFoundResult>(notFoundResult);

            repo.Verify(r => r.ExistsAsync<Question>
                (It.IsAny<Expression<Func<Question, bool>>>()), Times.Once);
            repo.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task EditAnswerTest() {
            // Arrange
            var repo = new Mock<IRepositoryEnq>();
            var answer = QuestionData.TestAnswer;

            repo.Setup(r => r.GetByIdAsync<Answer>(answer.Id, null, null))
                .ReturnsAsync(answer);

            var controller = new QuestionApiController(repo.Object);

            // Act
            var result = await controller.EditAnswer(answer);
            
            // Assert
            Assert.IsType<OkResult>(result);
            repo.Verify(r => r.Update(answer), Times.Once);
            repo.Verify(r => r.SaveAsync(), Times.Once);
            repo.Verify(r => r.GetByIdAsync<Answer>(answer.Id, null, null));
            repo.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task EditAnswerErrorTest() {
            // Arrange
            var repo = new Mock<IRepositoryEnq>();
            var answer = QuestionData.TestAnswer;
            var errAnswer = QuestionData.InvalidTestAnswer;

            repo.Setup(r => r.GetByIdAsync<Answer>(answer.Id, null, null))
                .Returns(Task.FromResult<Answer>(null));

            var controller = new QuestionApiController(repo.Object);

            // Act
            var notFoundResult = await controller.EditAnswer(answer);
            var badReqResult = await controller.EditAnswer(errAnswer);

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
            Assert.IsType<BadRequestResult>(badReqResult);
            Assert.Equal(1, repo.Invocations.Count);

            repo.Verify(r => r.GetByIdAsync<Answer>(answer.Id, null, null), Times.Once);
            repo.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task DeleteAnswerTest() {
            // Arrange
            var repo = new Mock<IRepositoryEnq>();

            repo.Setup(r => r.ExistsAsync<Answer>
                    (It.IsAny<Expression<Func<Answer, bool>>>())).ReturnsAsync(true);

            var controller = new QuestionApiController(repo.Object);

            // Act
            var result = await controller.DeleteAnswer(10);

            // Assert
            Assert.IsType<OkResult>(result);

            repo.Verify(r => r.ExistsAsync<Answer>
                (It.IsAny<Expression<Func<Answer, bool>>>()), Times.Once);
            repo.Verify(r => r.DeleteById<Answer>(10), Times.Once);
            repo.Verify(r => r.SaveAsync(), Times.Once);
            repo.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task DeleteAnswerErrorTest() {
            // Arrange
            var repo = new Mock<IRepositoryEnq>();

            repo.Setup(r => r.ExistsAsync<Answer>
                    (It.IsAny<Expression<Func<Answer, bool>>>())).ReturnsAsync(false);

            var controller = new QuestionApiController(repo.Object);

            // Act
            var result = await controller.DeleteAnswer(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);

            repo.Verify(r => r.ExistsAsync<Answer>
                (It.IsAny<Expression<Func<Answer, bool>>>()), Times.Once);
            repo.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task EditQuestionTest() {
            // Arrange
            var repo = new Mock<IRepositoryEnq>();
            var question = QuestionData.TestQuestion;

            repo.Setup(r => r.GetByIdAsync<Question>(question.Id, null, null))
                .ReturnsAsync(question);

            var controller = new QuestionApiController(repo.Object);
            
            // Act
            var result = await controller.EditQuestion(question);

            // Assert
            Assert.IsType<OkResult>(result);
            repo.Verify(r => r.GetByIdAsync<Question>(question.Id, null, null)
                , Times.Once);
            repo.Verify(r => r.Update(question), Times.Once);
            repo.Verify(r => r.SaveAsync(), Times.Once);
            repo.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task EditQuestionErrorTest() {
            // Arrange
            var repo = new Mock<IRepositoryEnq>();
            var question = QuestionData.TestQuestion;
            var errQuestion = QuestionData.InvalidTestQuestion;

            repo.Setup(r => r.GetByIdAsync<Question>(question.Id, null, null))
                .Returns(Task.FromResult<Question>(null));
            
            var controller = new QuestionApiController(repo.Object);

            // Act
            var notFoundResult = await controller.EditQuestion(question);
            var badReqResult = await controller.EditQuestion(errQuestion);

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
            Assert.IsType<BadRequestResult>(badReqResult);

            repo.Verify(r => r.GetByIdAsync<Question>(question.Id, null, null)
                , Times.Once);
            repo.VerifyNoOtherCalls();
        }

        [Fact]
        public void HttpVerbTests() {
            Assert.True(HasAttribute(nameof(QuestionApiController.CreateAnswer)
                , typeof(HttpPostAttribute)));

            Assert.True(HasAttribute(nameof(QuestionApiController.DeleteAnswer)
                , typeof(HttpDeleteAttribute)));

            Assert.True(HasAttribute(nameof(QuestionApiController.EditAnswer)
                , typeof(HttpPutAttribute)));

            Assert.True(HasAttribute(nameof(QuestionApiController.EditQuestion)
                , typeof(HttpPutAttribute)));
        }
    }
}
