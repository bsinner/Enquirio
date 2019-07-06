using System;
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
        public void CreateAnswerTest() {
            RunTest((answer, err, mockRepo, controller) => {
                
                // Arrange
                mockRepo.Setup(repo => repo.Create(answer));
                mockRepo.Setup(repo => repo.SaveAsync()).Callback(() => answer.Id = 50);
                mockRepo.Setup(repo => repo.ExistsAsync<Question>(e => e.Id == 1))
                    .ReturnsAsync(true);

                // Act
                OkObjectResult result = controller
                    .CreateAnswer((Answer)answer, 1)
                    .Result as OkObjectResult;

                // Assert
                Assert.Equal("50", result.Value);

                mockRepo.Verify(repo => repo.Create(answer), Times.Once);
                mockRepo.Verify(repo => repo.SaveAsync(), Times.Once);
                mockRepo.Verify(repo => repo.ExistsAsync<Question>(e => e.Id == 1), Times.Once);
            });
        }

        [Fact]
        public void CreateAnswerErrorTest() {
            RunTest((answer, errAnswer, mockRepo, controller) => {
                
                // Arrange
                mockRepo.Setup(repo => repo.ExistsAsync<Question>(e => e.Id == 999))
                    .ReturnsAsync(false);

                // Act
                var notFoundResult = controller.CreateAnswer((Answer)answer, 999);
                var badReqResult = controller.CreateAnswer((Answer)errAnswer, 999);

                // Assert
                Assert.IsType<BadRequestResult>(badReqResult);
                Assert.IsType<NotFoundResult>(notFoundResult);

                mockRepo.Verify(repo => repo.ExistsAsync<Question>(e => e.Id == 999));
            });
        }

        [Fact]
        public void EditAnswerTest() {
            RunTest((answer, err, mockRepo, controller) => {

                // Arrange
                mockRepo.Setup(repo => repo.Update(answer));
                mockRepo.Setup(repo => repo.SaveAsync());

                // Act
                Task<StatusCodeResult> result = controller.EditAnswer(answer);

                // Assert
                Assert.Equal(201, result.Result.StatusCode);
                Assert.True(HasAttribute(nameof(QuestionApiController.EditAnswer)
                    , typeof(HttpPutAttribute)));
                
                mockRepo.Verify(repo => repo.Update(answer), Times.Once);
                mockRepo.Verify(repo => repo.SaveAsync(), Times.Once);
            });
        }

        [Fact]
        public void EditAnswerErrorTest() {
        }

        [Fact]
        public void DeleteAnswerTest() {
        }

        [Fact]
        public void DeleteAnswerErrorTest() {
        }

        [Fact]
        public void EditQuestionTest() {
        }

        [Fact]
        public void EditQuestionErrorTest() {
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
        private void RunTest(Action<IPost, IPost, Mock<IRepositoryEnq>, QuestionApiController> test, bool questionTest = false) {
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
