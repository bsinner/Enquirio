using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using Enquirio.Controllers;
using Enquirio.Data;
using Enquirio.Models;
using Enquirio.Tests.TestData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Enquirio.Tests.Tests.Controllers {
    public class QuestionApiWriteTests : ApiTestUtil {
        
        [Fact]
        public async Task CreateQuestionTest() {
            // Arrange
            var repo = new Mock<IRepositoryEnq>();
            var context = GetMockContext();

            var question = QuestionData.TestQuestionMinProps;
            var controller 
                = new WriteApiController(repo.Object, context.Object);

            // Act
            var result = await controller.CreateQuestion(question);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            
            repo.Verify(r => r.Create(question), Times.Once);
            repo.Verify(r => r.SaveAsync(), Times.Once);
            VerifyMockContext(context, 1);
            repo.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task CreateQuestionErrorTest() {
            // Arrange
            var repo = new Mock<IRepositoryEnq>();
            var question = QuestionData.CreateTestInvalidQuestion;
            var context = GetMockContext();

            var controller 
                = new WriteApiController(repo.Object, context.Object);

            // Act
            var result = await controller.CreateQuestion(question);

            // Assert
            Assert.IsType<BadRequestResult>(result);
            VerifyMockContext(context, 0);
            repo.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task CreateAnswerTest() {
            // Arrange
            var repo = new Mock<IRepositoryEnq>();
            var answer = QuestionData.TestAnswerMinProps;
            var context = GetMockContext();

            repo.Setup(r => r.ExistsAsync<Question>
                (It.IsAny<Expression<Func<Question, bool>>>()))
                        .ReturnsAsync(true);

            var controller 
                = new WriteApiController(repo.Object, context.Object);

            // Act
            var result = await controller.CreateAnswer(answer);

            // Assert
            Assert.IsType<OkObjectResult>(result);

            repo.Verify(r => r.Create(answer), Times.Once);
            repo.Verify(r => r.SaveAsync(), Times.Once);
            repo.Verify(r => r.ExistsAsync<Question>
                (It.IsAny<Expression<Func<Question, bool>>>()), Times.Once);
            VerifyMockContext(context, 1);
            repo.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task CreateAnswerErrorTest() {
            // Arrange
            var repo = new Mock<IRepositoryEnq>();
            var answer = QuestionData.TestAnswerMinProps;
            var errAnswer = QuestionData.InvalidTestAnswer;
            var context = GetMockContext();

            repo.Setup(r => r.ExistsAsync<Question>(q => q.Id == answer.Id))
                .ReturnsAsync(false);

            var controller 
                = new WriteApiController(repo.Object, context.Object);

            // Act
            var notFoundResult = await controller.CreateAnswer(answer);
            var badReqResult = await controller.CreateAnswer(errAnswer);

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
            Assert.IsType<BadRequestResult>(badReqResult);

            repo.Verify(r => r.ExistsAsync<Question>
                (It.IsAny<Expression<Func<Question, bool>>>()), Times.Once);
            VerifyMockContext(context, 0);
            repo.VerifyNoOtherCalls();
        }


        [Fact]
        public async Task EditQuestionTest() {
            // Arrange
            var repo = new Mock<IRepositoryEnq>();
            var question = QuestionData.TestQuestion;

            repo.Setup(r => r.GetByIdAsync<Question>(question.Id, null, null))
                .ReturnsAsync(question);

            var controller = new WrApiControllerWithContext(repo.Object);

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
            var errQuestion = QuestionData.EditTestInvalidTestQuestion;

            repo.Setup(r => r.GetByIdAsync<Question>(question.Id, null, null))
                .Returns(Task.FromResult<Question>(null));

            var controller = new WrApiControllerWithContext(repo.Object);

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
        public async Task EditAnswerTest() {
            // Arrange
            var repo = new Mock<IRepositoryEnq>();
            var answer = QuestionData.TestAnswer;

            repo.Setup(r => r.GetByIdAsync<Answer>(answer.Id, null, null))
                .ReturnsAsync(answer);

            var controller = new WrApiControllerWithContext(repo.Object);

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

            var controller = new WrApiControllerWithContext(repo.Object);

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
        public async Task DeleteQuestionTest() {
            // Arrange
            var repo = new Mock<IRepositoryEnq>();
            var id = 1;

            repo.Setup(r => r.ExistsAsync<Question>
                    (It.IsAny<Expression<Func<Question, bool>>>())).ReturnsAsync(true);

            var controller = new WrApiControllerWithContext(repo.Object);

            // Act
            var result = await controller.DeleteQuestion(id);

            // Assert
            Assert.IsType<OkResult>(result);

            repo.Verify(r => r.ExistsAsync<Question>
                    (It.IsAny<Expression<Func<Question, bool>>>()), Times.Once);
            repo.Verify(r => r.DeleteById<Question>(id), Times.Once);
            repo.Verify(r => r.SaveAsync(), Times.Once);
            repo.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task DeleteQuestionErrorTest() {
            // Arrange
            var repo = new Mock<IRepositoryEnq>();
            var notFoundId = 8000;
            var badId1 = 0;
            var badId2 = -10;

            repo.Setup(r => r.ExistsAsync<Question>
                    (It.IsAny<Expression<Func<Question, bool>>>())).ReturnsAsync(false);
            
            var controller = new WrApiControllerWithContext(repo.Object);

            // Act
            var notFoundResult = await controller.DeleteQuestion(notFoundId);
            var badReqResult = await controller.DeleteQuestion(badId1);
            await controller.DeleteQuestion(badId2);

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
            Assert.IsType<BadRequestResult>(badReqResult);

            repo.Verify(r => r.ExistsAsync<Question>
                    (It.IsAny<Expression<Func<Question, bool>>>()), Times.Once);
            repo.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task DeleteAnswerTest() {
            // Arrange
            var repo = new Mock<IRepositoryEnq>();
            var id = 10;

            repo.Setup(r => r.ExistsAsync<Answer>
                (It.IsAny<Expression<Func<Answer, bool>>>())).ReturnsAsync(true);

            var controller = new WrApiControllerWithContext(repo.Object);

            // Act
            var result = await controller.DeleteAnswer(id);

            // Assert
            Assert.IsType<OkResult>(result);

            repo.Verify(r => r.ExistsAsync<Answer>
                (It.IsAny<Expression<Func<Answer, bool>>>()), Times.Once);
            repo.Verify(r => r.DeleteById<Answer>(id), Times.Once);
            repo.Verify(r => r.SaveAsync(), Times.Once);
            repo.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task DeleteAnswerErrorTest() {
            // Arrange
            var repo = new Mock<IRepositoryEnq>();
            var notFoundId = 7000;
            var badId1 = 0;
            var badId2 = -100;

            repo.Setup(r => r.ExistsAsync<Answer>
                (It.IsAny<Expression<Func<Answer, bool>>>())).ReturnsAsync(false);

            var controller = new WrApiControllerWithContext(repo.Object);

            // Act
            var notFoundResult = await controller.DeleteAnswer(notFoundId);
            var badReqResult = await controller.DeleteAnswer(badId1);
            await controller.DeleteAnswer(badId2);

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
            Assert.IsType<BadRequestResult>(badReqResult);

            repo.Verify(r => r.ExistsAsync<Answer>
                (It.IsAny<Expression<Func<Answer, bool>>>()), Times.Once);
            repo.VerifyNoOtherCalls();
        }

        [Fact]
        public void HttpVerbTests() {
            var t = typeof(WriteApiController);
            var post = typeof(HttpPostAttribute);
            var put = typeof(HttpPutAttribute);
            var delete = typeof(HttpDeleteAttribute);

            var methods = new Dictionary<string, Type> {
                [ nameof(WriteApiController.CreateAnswer) ] = post
                , [ nameof(WriteApiController.DeleteAnswer) ] = delete
                , [ nameof(WriteApiController.EditAnswer) ] = put
                , [ nameof(WriteApiController.EditQuestion) ] = put
                , [ nameof(WriteApiController.DeleteQuestion) ] = delete
                , [ nameof(WriteApiController.CreateQuestion) ] = post
            };

            Assert.All(methods, kvp => Assert.True(HasAttribute(kvp.Key, kvp.Value, t)));
        }

        private Mock<IHttpContextAccessor> GetMockContext() {
            var cxt = new Mock<IHttpContextAccessor>();
            var fakeGuid = "".PadLeft(36, '.');

            cxt.Setup(c => c.HttpContext.User
                    .FindFirst(ClaimTypes.NameIdentifier))
                .Returns(new Claim(ClaimTypes.NameIdentifier, fakeGuid));

            return cxt;
        }

        // Times represents times IHttpContextAccessor got the current user's ID
        private void VerifyMockContext(Mock<IHttpContextAccessor> context, int times) {
            
            context.Verify(c => c.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)
                , Times.Exactly(times));
            context.VerifyNoOtherCalls();
        }
    }
}
