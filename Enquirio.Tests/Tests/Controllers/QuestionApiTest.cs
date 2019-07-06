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

                // Act
                Task<string> id = controller.CreateAnswer((Answer)answer, 99);

                // Assert
                Assert.Equal("50", id.Result);
                Assert.True(HasAttribute(nameof(QuestionApiController.CreateAnswer)
                    , typeof(HttpPostAttribute)));

                mockRepo.Verify(repo => repo.Create(answer));
                mockRepo.Verify(repo => repo.SaveAsync());
            });
        }

        [Fact]
        public void CreateAnswerErrorTest() {
            RunTest((answer, err, mockRepo, controller) => {
                // Arrange
                // Act
                // Assert
            });
        }

        [Fact]
        public void EditAnswerTest() {
//            RunTest((answer, err, mockRepo, controller) => {
//
//                // Arrange
//                mockRepo.Setup(repo => repo.Update(answer));
//                mockRepo.Setup(repo => repo.SaveAsync());
//
//                // Act
//                Task<StatusCodeResult> result = controller.EditAnswer(answer);
//
//                // Assert
//                Assert.Equal(201, result.Result.StatusCode);
//                Assert.True(HasAttribute(nameof(QuestionApiController.EditAnswer)
//                    , typeof(HttpPutAttribute)));
//                
//                mockRepo.Verify(repo => repo.Update(answer), Times.Once);
//                mockRepo.Verify(repo => repo.SaveAsync(), Times.Once);
//            });
        }

        [Fact]
        public void DeleteAnswerTest() {
        }

        [Fact]
        public void EditQuestionTest() {
        }

        // Run test, pass valid entity, invalid entity, repository, and controller
        private void RunTest(Action<IPost, IPost, Mock<IRepositoryEnq>, QuestionApiController> test, bool questionTest = false) {
            IPost entity = null;
            IPost invalidEntity = null;

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
