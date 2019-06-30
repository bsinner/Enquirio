﻿using System.Threading.Tasks;
using Enquirio.Controllers;
using Enquirio.Data;
using Enquirio.Models;
using Enquirio.Tests.TestData;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Enquirio.Tests.Tests.Controllers {
    public class QuestionTest {

        [Fact]
        public void ViewQuestionTest() {
            // Arrange
            var mockRepo = new Mock<IRepositoryEnq>();

            mockRepo.Setup(repo => repo.GetByIdAsync<Question>("1", new [] {"Answers"}, null))
                    .Returns(Task.FromResult(QuestionData.TestQuestion));

            QuestionController controller = new QuestionController(mockRepo.Object);

            // Act
            Task<ViewResult> result = controller.ViewQuestion("1");

            // Assert
            Assert.Null(result.Result.ViewName);
            Assert.IsType<Question>(result.Result.ViewData.Model);
            mockRepo.Verify(repo => repo.GetByIdAsync<Question>("1", new[] {"Answers"}, null), Times.Once);
        }

        [Fact]
        public void ViewQuestionNotFoundTest() {
            // Arrange
            var mockRepo = new Mock<IRepositoryEnq>();

            mockRepo.Setup(repo => repo.GetByIdAsync<Question>("foo", new[] {"Answers"}, null))
                .Returns(Task.FromResult<Question>(null));

            var controller = new QuestionController(mockRepo.Object);

            // Act
            Task<ViewResult> result = controller.ViewQuestion("foo");

            // Assert
            Assert.Equal("NotFound", result.Result.ViewName);
            Assert.Null(result.Result.ViewData.Model);
            mockRepo.Verify(repo => repo.GetByIdAsync<Question>("foo", new [] { "Answers" }, null), Times.Once);
        }

        [Fact]
        public void CreateNewQuestionTest() {
            // Arrange
            var question = QuestionData.TestQuestion;
            var mockRepo = new Mock<IRepositoryEnq>();

            mockRepo.Setup(repo => repo.Create(question));
            mockRepo.Setup(repo => repo.SaveAsync()).Callback(() => question.Id = 1);

            var controller = new QuestionController(mockRepo.Object);

            // Act
            Task<RedirectToRouteResult> result = controller.Create(question);

            // Assert
            Assert.Equal(1, question.Id);
            Assert.Equal(question.Id, result.Result.RouteValues["id"]);
            Assert.Equal("question", result.Result.RouteName);
        }

        [Fact]
        public void CreateViewTest() {
            // Arrange
            var mockRepo = new Mock<IRepositoryEnq>();
            var controller = new QuestionController(mockRepo.Object);

            // Act
            ViewResult result = controller.Create();

            // Assert
            Assert.Null(result.ViewName);
            Assert.Null(result.ViewData.Model);
        }
    }
}
