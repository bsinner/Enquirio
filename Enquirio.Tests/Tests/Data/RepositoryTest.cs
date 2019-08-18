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

        private readonly List<string> _data = SqlTestData.Questions();

        [Fact]
        public void TestGetAll() {
            RunTest(async (repo, repo2) => {
                var results = repo.GetAll<Question>();
                Assert.Equal(9, results.Count());

                var resultsAsync = await repo2.GetAllAsync<Question>();
                Assert.Equal(9, resultsAsync.Count());
            });
        }

        [Fact]
        public void TestCreateWithDateInit() {
            RunTest((repo, repo2) => {
                Question q = new Question() { Title = "q", Contents = "..." };
                Answer a = new Answer() { Title = "a", Contents = "..." };
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
            RunTest((repo, repo2) => {

                var results = repo.GetAll<Question>(
                        q => q.Id < 5
                        , q => q.Id, true
                        , skip : 1
                        , includedNavProps : new[] { "Answers" });

                int last = results.Count - 1;

                Assert.Equal(3, results.Count);
                // Assert descending order
                Assert.Equal(1, results[last].Id);
                Assert.Equal(3, results[last].Answers.Count);
            });
        }

        [Fact]
        public void TestGetFilteredAsync() {
            RunTest(async (repo, repo2) => {

                var results = await repo.GetAsync<Answer>(
                        a => a.QuestionId == 1
                        , a => a.Title
                        , take : 3
                        , skip : 1
                        , includedNavProps : new[] { "Question" });

                Assert.Equal(2, results.Count);
                // Assert ascending order
                Assert.Equal("A3", results[0].Title);
                Assert.True(results[0].Question.Id > 0);
            });
        }

        [Fact]
        public void TestGetById() {
            RunTest((repo, repo2) => {
                var result = 
                    repo.GetById<Answer>("1", navPropFks : new [] { "Question" });

                Assert.NotNull(result);
                Assert.True(result.Question.Title.Length > 0);
            });
        }

        [Fact]
        public void TestGetByIdAsync() {
            RunTest(async (repo, repo2) => {
                var result = await repo.GetByIdAsync<Question>
                    (2, navPropCollections : new [] { "Answers" });

                Assert.NotNull(result);
                Assert.Single(result.Answers);
            });
        }

        // Delete and DeleteById are tested by the same method
        // because DeleteById invokes Delete
        [Fact]
        public void TestDeleteById() {
            RunTest((repo, repo2) => {
                repo.DeleteById<Question>(1);
                repo.DeleteById<Question>("2");
                repo.Save();

                var q = repo2.GetAll<Question>();
                var a = repo2.GetAll<Answer>();

                Assert.Equal(7, q.Count());
                Assert.Empty(a);
            });
        }

        [Fact]
        public void TestUpdateWithAutoDateChange() {
            RunTest(asyncTest : async (repo, repo2) => {
                var id = 2;
                var newContents = "----";

                Question q = repo.GetById<Question>(id, navPropCollections : new [] { "Answers" });
                Answer a = new Answer() { Title = "A5", Contents = "..." };

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
            RunTest((repo, repo2) => {
                Assert.True(repo.Exists<Question>(e => e.Id == 1));
                Assert.False(repo.Exists<Question>(e => e.Id == 999));
            });
        }

        [Fact]
        public void TestExistsAsync() {
            RunTest(asyncTest: async (repo, repo2) => {
                Assert.True(await repo.ExistsAsync<Question>(e => e.Id == 1));
                Assert.False(await repo.ExistsAsync<Question>(e => e.Id == 999));
            });
        }

        [Fact]
        public void TestGetCountAsync() {
            RunTest(asyncTest: async (repo, repo2) => {
                Assert.Equal(9, await repo.GetCountAsync<Question>());
                Assert.Equal(0, await 
                    repo.GetCountAsync<Question>(q => q.Id == 99));
                Assert.Equal(3, await 
                    repo.GetCountAsync<Question>(q => q.Id >= 2 && q.Id <= 4));
                Assert.Equal(99, await repo.GetCountAsync<Question>());
                
            });
        }

        [Fact]
        public void TestGetCount() {
            RunTest((repo, repo2) => {
                Assert.Equal(4, repo.GetCount<Answer>());
                Assert.Equal(0, 
                    repo.GetCount<Answer>(q => q.Contents == "z"));
                Assert.Equal(1, 
                    repo.GetCount<Answer>(q => q.Id == 3));
            });
        }

        // Create test data and pass repositories to access it in callback
        private void RunTest(Action<RepositoryEnq, RepositoryEnq> test = null
                , Func<RepositoryEnq, RepositoryEnq, Task> asyncTest = null) {

            using var factory = new TestContextFactory(_data);
            using var context = factory.GetContext();

            // Two repositories are created so tests can ignore 
            // cached data from the first repo and fetch from db
            RepositoryEnq repo = new RepositoryEnq(context);
            RepositoryEnq repo2 = new RepositoryEnq(context);

            asyncTest?.Invoke(repo, repo2);
            test?.Invoke(repo, repo2);
        }

        private async Task RunTestAsync(Func<RepositoryEnq, RepositoryEnq, Task> test) {
            using var factory = new TestContextFactory(_data);
            using var context = factory.GetContext();

            RepositoryEnq repo = new RepositoryEnq(context);
            RepositoryEnq repo2 = new RepositoryEnq(context);

            await test.Invoke(repo, repo2);
        }
    }
}
