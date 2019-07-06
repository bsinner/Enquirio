﻿using System;
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
        public void CreateAnswerTest() {
            RunTest(async (answer, err, mockRepo, controller) => {
                
                // Arrange
                mockRepo.Setup(repo => repo.SaveAsync()).Callback(() => answer.Id = 50);
                mockRepo.Setup(repo => repo.ExistsAsync<Question>(e => e.Id == 1))
                    .ReturnsAsync(true);

                // Act
                var result = await controller
                    .CreateAnswer((Answer) answer, 1) as OkObjectResult;

                // Assert
                Assert.Equal("50", result.Value);

                mockRepo.Verify(repo => repo.Create(answer), Times.Once);
                mockRepo.Verify(repo => repo.SaveAsync(), Times.Once);
                mockRepo.Verify(repo => repo.ExistsAsync<Question>(e => e.Id == 1), Times.Once);
            });
        }

        [Fact]
        public void CreateAnswerErrorTest() {
            RunTest(async (answer, errAnswer, mockRepo, controller) => {
                
                // Arrange
                mockRepo.Setup(repo => repo.ExistsAsync<Question>(e => e.Id == 999))
                    .ReturnsAsync(false);

                // Act
                var notFoundResult = await controller.CreateAnswer((Answer)answer, 999);
                var badReqResult = await controller.CreateAnswer((Answer)errAnswer, 999);

                // Assert
                Assert.IsType<BadRequestResult>(badReqResult);
                Assert.IsType<NotFoundResult>(notFoundResult);

                mockRepo.Verify(repo => repo.ExistsAsync<Question>
                    (It.IsAny<Expression<Func<Question, bool>>>()), Times.Once);
            });
        }

        [Fact]
        public void EditAnswerTest() {
            RunTest(async (answer, err, mockRepo, controller) => {
                // Arrange
                mockRepo.Setup(repo => repo.ExistsAsync<Question>(e => e.Id == 1))
                    .ReturnsAsync(true);
                mockRepo.Setup(repo => repo.ExistsAsync<Answer>(e => e.Id == answer.Id))
                    .ReturnsAsync(true);

                // Act
                var result = await controller.EditAnswer(answer, 1);
                
                // Assert
                Assert.IsType<OkResult>(result);
                mockRepo.Verify(repo => repo.Update(answer), Times.Once);
                mockRepo.Verify(repo => repo.SaveAsync(), Times.Once);
            });
        }

        [Fact]
        public void EditAnswerErrorTest() {
            RunTest(async (answer, errAnswer, mockRepo, controller) => { });
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
//            Assert.True(HasAttribute(nameof(QuestionApiController.CreateAnswer)
//                , typeof(HttpPostAttribute)));
//
//            Assert.True(HasAttribute(nameof(QuestionApiController.DeleteAnswer)
//                , typeof(HttpDeleteAttribute)));
//
//            Assert.True(HasAttribute(nameof(QuestionApiController.EditAnswer)
//                , typeof(HttpPutAttribute)));
//
//            Assert.True(HasAttribute(nameof(QuestionApiController.EditQuestion)
//                , typeof(HttpPutAttribute)));
        }

        // Run test, pass valid entity, invalid entity, repository, and controller
        private void RunTest(Func<IPost, IPost, Mock<IRepositoryEnq>, QuestionApiController, Task> test, bool questionTest = false) {
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
