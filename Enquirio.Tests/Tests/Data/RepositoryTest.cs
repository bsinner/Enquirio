using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Enquirio.Data;
using Enquirio.Models;
using Enquirio.Tests.Util;
using Xunit;

// Test repository class using in memory Sqlite database as data source.
namespace Enquirio.Tests {
    public class RepositoryTest {

        private readonly List<string> _data = SqlTestData.QuestionData();

        [Fact]
        public void TestGetAll() {
            RunTest(repo => {
                Assert.Equal(9, repo.GetAll<Question>().Count);
            });
        }

        [Fact]
        public async Task TestGetAllAsync() {
            await RunTestAsync(async repo => {
                Assert.Equal(4
                    , (await repo.GetAllAsync<Answer>()).Count());
            });
        }

        [Fact]
        public void TestCreateWithDateInit() {
            RunTest(testMulti: (repo, repo2) => {
                string uid = SqlTestData.UserId;
                Question q = new Question { Title = "q", Contents = "...", UserId = uid };
                Answer a = new Answer { Title = "a", Contents = "...", UserId = uid};
                q.Answers.Add(a);

                repo.Create(q);
                repo.Save();

                var result = repo2.GetById<Question>(q.Id);

                Assert.Equal(DateTime.Now.Year, result.Created.Year);
                Assert.NotEmpty(result.Answers);
            });
        }

        // Get and GetAll filtered are tested by the same method because the only difference
        // between them is an optional filter param in GetAll and a mandatory filter in Get
        [Fact]
        public void TestGetFiltered() {
            RunTest(repo => {

                var results = repo.GetAll<Question>(
                        q => q.Id < 5
                        , q => q.Id, true
                        , skip: 1
                        , includedNavProps: new[] { "Answers" });

                int last = results.Count - 1;

                Assert.Equal(3, results.Count);
                // Assert descending order
                Assert.Equal(1, results[last].Id);
                Assert.Equal(3, results[last].Answers.Count);
            });
        }

        [Fact]
        public async Task TestGetFilteredAsync() {
            await RunTestAsync(async repo => {

                var results = await repo.GetAsync<Answer>(
                        a => a.QuestionId == 1
                        , a => a.Title, skip: 1
                        , take: 3, includedNavProps: new[] { "Question" });

                Assert.Equal(2, results.Count);
                // Assert ascending order
                Assert.Equal("A3", results[0].Title);
                Assert.True(results[0].Question.Id > 0);
            });
        }

        [Fact]
        public void TestGetById() {
            RunTest(repo => {
                var result = 
                    repo.GetById<Answer>("1", navPropFks : new [] { "Question" });

                Assert.NotNull(result);
                Assert.True(result.Question.Title.Length > 0);
            });
        }

        [Fact]
        public async Task TestGetByIdAsync() {
            await RunTestAsync(async repo => {

                var result = await repo.GetByIdAsync<Question>
                    (2, new [] { "Answers" });

                Assert.NotNull(result);
                Assert.Single(result.Answers);
            });
        }

        // Delete and DeleteById are tested by the same method
        // because DeleteById invokes Delete
        [Fact]
        public void TestDeleteById() {
            RunTest(testMulti: (repo, repo2) => {
                repo.DeleteById<Question>(1);
                repo.DeleteById<Question>("2");
                repo.Save();

                var q = repo2.GetAll<Question>();
                var a = repo2.GetAll<Answer>();

                Assert.Equal(7, q.Count);
                Assert.Empty(a);
            });
        }

        [Fact]
        public async Task TestUpdateWithAutoDateChange() {
            await RunTestAsync(testMulti : async (repo, repo2) => {
                var id = 2;
                var newContents = "----";
                
                Question q = repo.GetById<Question>(id, new [] { "Answers" });
                Answer a = new Answer { Title = "A5", Contents = "...", UserId = SqlTestData.UserId };

                q.Contents = newContents;
                q.Answers.Add(a);

                repo.Update(q);
                await repo.SaveAsync();

                var result = repo2.GetById<Question>(id);

                Assert.Equal(newContents, result.Contents);
                Assert.Equal(2, result.Answers.Count);
                Assert.Equal(DateTime.Now.Year, result.Edited?.Year);
            });
        }

        [Fact]
        public void TestExists() {
            RunTest(repo => {
                Assert.True(repo.Exists<Question>(e => e.Id == 1));
                Assert.False(repo.Exists<Question>(e => e.Id == 999));
            });
        }

        [Fact]
        public async Task TestExistsAsync() {
            await RunTestAsync(async repo => {
                Assert.True(await repo.ExistsAsync<Question>(e => e.Id == 1));
                Assert.False(await repo.ExistsAsync<Question>(e => e.Id == 999));
            });
        }

        [Fact]
        public async Task TestGetCountAsync() {
            await RunTestAsync(async repo => {
                Assert.Equal(9, await repo.GetCountAsync<Question>());

                Assert.Equal(0, await 
                    repo.GetCountAsync<Question>(q => q.Id == 99));

                Assert.Equal(3, await 
                    repo.GetCountAsync<Question>(q => q.Id >= 2 && q.Id <= 4));
            });
        }

        [Fact]
        public void TestGetCount() {
            RunTest(repo => {
                Assert.Equal(4, repo.GetCount<Answer>());

                Assert.Equal(0, 
                    repo.GetCount<Answer>(q => q.Contents == "z"));

                Assert.Equal(1,
                    repo.GetCount<Answer>(q => q.Id == 3));
            });
        }

        // Create test data and pass repositories to access it in callback
        private void RunTest(Action<RepositoryEnq> test = null
                , Action<RepositoryEnq, RepositoryEnq> testMulti = null) {

            using var factory = new TestContextFactory(_data);
            using var context = factory.GetContext();

            var repo = new RepositoryEnq(context);
            
            test?.Invoke(repo);
            testMulti?.Invoke(repo, new RepositoryEnq(context));
        }

        private async Task RunTestAsync(Func<RepositoryEnq, Task> test = null
                , Func<RepositoryEnq, RepositoryEnq, Task> testMulti = null) {

            using var factory = new TestContextFactory(_data);
            await using var context = factory.GetContext();

            RepositoryEnq repo = new RepositoryEnq(context);

            await (test?.Invoke(repo) ?? Task.CompletedTask);
            await (testMulti?.Invoke(repo, new RepositoryEnq(context)) ?? Task.CompletedTask);
        }
    }
}
