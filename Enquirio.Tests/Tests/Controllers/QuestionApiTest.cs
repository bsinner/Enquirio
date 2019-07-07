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
                mockRepo.Setup(repo => repo.ExistsAsync<Question>(q => q.Id == 1))
                    .ReturnsAsync(true);

                // Act
                var result = await controller
                    .CreateAnswer((Answer) answer) as OkObjectResult;

                // Assert
                Assert.Equal("50", result.Value);
                
                mockRepo.Verify(repo => repo.Create(answer), Times.Once);
                mockRepo.Verify(repo => repo.SaveAsync(), Times.Once);
                mockRepo.Verify(repo => repo.ExistsAsync<Question>
                    (q => q.Id == 1), Times.Once);
                mockRepo.VerifyNoOtherCalls();
            });
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
        public void EditAnswerTest() {
            RunTest(async (answer, err, mockRepo, controller) => {
                
                // Arrange
                mockRepo.Setup(repo => repo.ExistsAsync<Question>(q => q.Id == 1))
                    .ReturnsAsync(true);
                mockRepo.Setup(repo => repo.ExistsAsync<Answer>(a => a.Id == answer.Id))
                    .ReturnsAsync(true);

                // Act
                var result = await controller.EditAnswer((Answer)answer);
                
                // Assert
                Assert.IsType<OkResult>(result);
                mockRepo.Verify(repo => repo.Update(answer), Times.Once);
                mockRepo.Verify(repo => repo.SaveAsync(), Times.Once);
                mockRepo.Verify(repo => repo.ExistsAsync<Question>
                    (q => q.Id == 1), Times.Once);
                mockRepo.Verify(repo => repo.ExistsAsync<Answer>
                    (a => a.Id == answer.Id), Times.Once);
                mockRepo.VerifyNoOtherCalls();
            });
        }

        [Fact]
        public void EditAnswerErrorTest() {
            RunTest(async (answer, errAnswer, mockRepo, controller) => {
                
                // Arrange
                mockRepo.Setup(repo => repo.ExistsAsync<Question>(q => q.Id == answer.Id))
                    .ReturnsAsync(false);
                mockRepo.Setup(repo => repo.ExistsAsync<Answer>(a => a.Id == answer.Id))
                    .ReturnsAsync(false);

                // Act
                var notFoundResult = await controller.EditAnswer((Answer)answer);
                var badReqResult = await controller.EditAnswer((Answer)errAnswer);

                // Assert
                Assert.IsType<NotFoundResult>(notFoundResult);
                Assert.IsType<BadRequestResult>(badReqResult);
                Assert.Equal(1, mockRepo.Invocations.Count);

                mockRepo.Verify(repo => repo.ExistsAsync<Question>(q => q.Id == 99));
                mockRepo.Verify(repo => repo.ExistsAsync<Answer>(a => a.Id == answer.Id));
                mockRepo.VerifyNoOtherCalls();
            });
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
        public void EditQuestionTest() {
            RunTest(async (question, err, mockRepo, controller) => {

                // Arrange
                mockRepo.Setup(repo => repo.ExistsAsync<Question>(q => q.Id == question.Id))
                    .ReturnsAsync(true);

                // Act
                var result = await controller.EditQuestion((Question)question);

                // Assert
                Assert.IsType<OkResult>(result);
                mockRepo.Verify(repo => repo.ExistsAsync<Question>
                    (q => q.Id == question.Id), Times.Once);
                mockRepo.Verify(repo => repo.Update(question), Times.Once);
                mockRepo.Verify(repo => repo.SaveAsync(), Times.Once);
                mockRepo.VerifyNoOtherCalls();
            }, true);
        }

        [Fact]
        public void EditQuestionErrorTest() {
            RunTest(async (question, errQuestion, mockRepo, controller) => {

                // Arrange
                mockRepo.Setup(repo => repo.ExistsAsync<Question>(q => q.Id == question.Id))
                    .ReturnsAsync(false);
                
                // Act
                var notFoundResult = await controller.EditQuestion((Question)question);
                var badReqResult = await controller.EditQuestion((Question)errQuestion);

                // Assert
                Assert.IsType<NotFoundResult>(notFoundResult);
                Assert.IsType<BadRequestResult>(badReqResult);

                mockRepo.Verify(repo => repo.ExistsAsync<Question>
                    (q => q.Id == question.Id), Times.Once);
                mockRepo.VerifyNoOtherCalls();
            }, true);
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
