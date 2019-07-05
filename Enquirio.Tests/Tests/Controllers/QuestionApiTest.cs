using System;
using System.Reflection;
using System.Threading.Tasks;
using Castle.Core.Internal;
using Enquirio.Controllers;
using Enquirio.Data;
using Enquirio.Tests.TestData;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Enquirio.Tests.Tests.Controllers {
    public class QuestionApiTest {
        
        [Fact]
        public void CreateAnswerTest() {
            // Arrange
            var mockRepo = new Mock<IRepositoryEnq>();
            var answer = QuestionData.TestAnswer;

            mockRepo.Setup(repo => repo.Create(answer));
            mockRepo.Setup(repo => repo.SaveAsync()).Callback(() => answer.Id = 1);

            var controller = new QuestionApiController(mockRepo.Object);

            // Act
            Task<string> id = controller.CreateAnswer(answer, 99);

            // Assert
            Assert.Equal("1", id.Result);
            Assert.True(HasAttribute(
                nameof(QuestionApiController.CreateAnswer)
                , typeof(HttpPostAttribute)));

            mockRepo.Verify(repo => repo.Create(answer));
            mockRepo.Verify(repo => repo.SaveAsync());
        }

        [Fact]
        public void EditAnswerTest() {
        }

        [Fact]
        public void DeleteAnswerTest() {
        }

        [Fact]
        public void EditQuestionTest() {
        }

        private bool HasAttribute(String method, Type attribute) {
            return !typeof(QuestionApiController)
                .GetMethod(method)
                .GetCustomAttributes(attribute)
                .IsNullOrEmpty();
        }

    }
}
