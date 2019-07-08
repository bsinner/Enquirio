using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Castle.Core.Internal;
using Enquirio.Controllers;
using Enquirio.Data;
using Enquirio.Models;
using Enquirio.Tests.TestData;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Enquirio.Tests.Tests.Controllers {
    public class QuestionApiTest {
        
        [Fact]
        public async Task CreateAnswerTest() {
            // Arrange
            var mockRepo = new Mock<IRepositoryEnq>();
            var answer = QuestionData.TestAnswer;

            mockRepo.Setup(repo => repo.SaveAsync()).Callback(() => answer.Id = 50);
            mockRepo.Setup(repo => repo.ExistsAsync<Question>
            (It.IsAny<Expression<Func<Question, bool>>>()))
                .ReturnsAsync(true);

            var controller = new QuestionApiController(mockRepo.Object);

            // Act
            var result = await controller.CreateAnswer(answer);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal("50", (result as OkObjectResult).Value);
                
            mockRepo.Verify(repo => repo.Create(answer), Times.Once);
            mockRepo.Verify(repo => repo.SaveAsync(), Times.Once);
            mockRepo.Verify(repo => repo.ExistsAsync<Question>
                (It.IsAny<Expression<Func<Question, bool>>>()), Times.Once);
            mockRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public void CreateAnswerErrorTest() {
            RunTest(async (answer, errAnswer, mockRepo, controller) => {
                
                // Arrange
                mockRepo.Setup(repo => repo.ExistsAsync<Question>(q => q.Id == answer.Id))
                    .ReturnsAsync(false);

                // Act
                var notFoundResult = await controller.CreateAnswer((Answer)answer);
                var badReqResult = await controller.CreateAnswer((Answer)errAnswer);

                // Assert
                Assert.IsType<BadRequestResult>(badReqResult);
                Assert.IsType<NotFoundResult>(notFoundResult);

                mockRepo.Verify(repo => repo.ExistsAsync<Question>
                    (It.IsAny<Expression<Func<Question, bool>>>()), Times.Once);
                mockRepo.VerifyNoOtherCalls();
            });
        }

        [Fact]
        public async Task EditAnswerTest() {
            // Arrange
            var mockRepo = new Mock<IRepositoryEnq>();
            var answer = QuestionData.TestAnswer;

            mockRepo.Setup(repo => repo.GetByIdAsync<Answer>(answer.Id, null, null))
                .ReturnsAsync(answer);

            var controller = new QuestionApiController(mockRepo.Object);

            // Act
            var result = await controller.EditAnswer(answer);
            
            // Assert
            Assert.IsType<OkResult>(result);
            mockRepo.Verify(repo => repo.Update(answer), Times.Once);
            mockRepo.Verify(repo => repo.SaveAsync(), Times.Once);
            mockRepo.Verify(repo => repo.GetByIdAsync<Answer>(answer.Id, null, null));
            mockRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task EditAnswerErrorTest() {
            // Arrange
            var mockRepo = new Mock<IRepositoryEnq>();
            var answer = QuestionData.TestAnswer;
            var errAnswer = QuestionData.InvalidTestAnswer;

            mockRepo.Setup(repo => repo.GetByIdAsync<Answer>(answer.Id, null, null))
                .Returns(Task.FromResult<Answer>(null));

            var controller = new QuestionApiController(mockRepo.Object);

            // Act
            var notFoundResult = await controller.EditAnswer(answer);
            var badReqResult = await controller.EditAnswer(errAnswer);

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
            Assert.IsType<BadRequestResult>(badReqResult);
            Assert.Equal(1, mockRepo.Invocations.Count);

            mockRepo.Verify(repo => repo.GetByIdAsync<Answer>(answer.Id, null, null), Times.Once);
            mockRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public void DeleteAnswerTest() {
            RunTest(async (answer, err, mockRepo, controller) => {
                
                // Arrange
                mockRepo.Setup(repo => repo.ExistsAsync<Answer>(a => a.Id == 10))
                    .ReturnsAsync(true);

                // Act
                var result = await controller.DeleteAnswer(10);

                // Assert
                Assert.IsType<OkResult>(result);

                mockRepo.Verify(repo => repo.ExistsAsync<Answer>
                    (a => a.Id == 10), Times.Once);
                mockRepo.Verify(repo => repo.DeleteById<Answer>(10), Times.Once);
                mockRepo.Verify(repo => repo.SaveAsync(), Times.Once);
                mockRepo.VerifyNoOtherCalls();
            });
        }

        [Fact]
        public void DeleteAnswerErrorTest() {
            RunTest(async (answer, errAnswer, mockRepo, controller) => {

                // Arrange
                mockRepo.Setup(repo => repo.ExistsAsync<Answer>(a => a.Id == 999))
                    .ReturnsAsync(false);

                // Act
                var result = await controller.DeleteAnswer(999);

                // Assert
                Assert.IsType<NotFoundResult>(result);

                mockRepo.Verify(repo => repo.ExistsAsync<Answer>
                    (a => a.Id == 999), Times.Once);
                mockRepo.VerifyNoOtherCalls();
            });
        }

        [Fact]
        public async Task EditQuestionTest() {
            // Arrange
            var mockRepo = new Mock<IRepositoryEnq>();
            var question = QuestionData.TestQuestion;

            mockRepo.Setup(repo => repo.GetByIdAsync<Question>(question.Id, null, null))
                .ReturnsAsync(question);

            var controller = new QuestionApiController(mockRepo.Object);
            
            // Act
            var result = await controller.EditQuestion(question);

            // Assert
            Assert.IsType<OkResult>(result);
            mockRepo.Verify(repo => repo.GetByIdAsync<Question>(question.Id, null, null)
                , Times.Once);
            mockRepo.Verify(repo => repo.Update(question), Times.Once);
            mockRepo.Verify(repo => repo.SaveAsync(), Times.Once);
            mockRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task EditQuestionErrorTest() {
            // Arrange
            var mockRepo = new Mock<IRepositoryEnq>();
            var question = QuestionData.TestQuestion;
            var errQuestion = QuestionData.InvalidTestQuestion;

            mockRepo.Setup(repo => repo.GetByIdAsync<Question>(question.Id, null, null))
                .Returns(Task.FromResult<Question>(null));
            
            var controller = new QuestionApiController(mockRepo.Object);

            // Act
            var notFoundResult = await controller.EditQuestion(question);
            var badReqResult = await controller.EditQuestion(errQuestion);

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
            Assert.IsType<BadRequestResult>(badReqResult);

            mockRepo.Verify(repo => repo.GetByIdAsync<Question>(question.Id, null, null)
                , Times.Once);
            mockRepo.VerifyNoOtherCalls();
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

        // Run test, pass valid entity, invalid entity, repository, and controller
        private void RunTest(Func<IPost, IPost, Mock<IRepositoryEnq>, QuestionApiController, Task> test
            , bool questionTest = false) {

            IPost entity;
            IPost invalidEntity;

            if (!questionTest) {
                entity = QuestionData.TestAnswer;
                invalidEntity = QuestionData.InvalidTestAnswer;
            } else {
                entity = QuestionData.TestQuestion;
                invalidEntity = QuestionData.InvalidTestQuestion;
            }

            var repo = new Mock<IRepositoryEnq>();
            var controller = new QuestionApiController(repo.Object);

            test.Invoke(entity, invalidEntity, repo, controller);
        }

        private bool HasAttribute(String method, Type attribute) {
            return !typeof(QuestionApiController)
                .GetMethod(method)
                .GetCustomAttributes(attribute)
                .IsNullOrEmpty();
        }

    }
}
