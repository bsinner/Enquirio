using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Enquirio.Data;
using Enquirio.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Enquirio.Infrastructure {

    public class DbInitializer {

        private const int QuestionsToCreate = 100;
        private const int UsersToCreate = 25;
        private const int MaxAnswers = 5;
        private const int QuestionMaxAge = 60;
        private const string SamplePw = "Aaaa1!";
        private static readonly Random _random = new Random();

        // If the database has no questions add sample questions, 
        // answers, and an admin user
        public static async Task Initialize(DbContextEnq context
                , IServiceProvider provider, IConfiguration config) {

            context.Database.EnsureCreated();

            if (context.Question.Any()) {
                return;
            }

            var userIds = await CreateUsers(provider, config);
            context.AddRange(GetQuestions(userIds.ToList()));
            context.SaveChanges();
        }

        // Create one admin and multiple regular users
        private static async Task<IEnumerable<string>> CreateUsers(IServiceProvider provider
                , IConfiguration config) {

            var userManager = provider
                .GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = provider
                .GetRequiredService<RoleManager<IdentityRole>>();

            var ids = new List<string> {
                await CreateAdmin(userManager, roleManager, config)
            };

            ids.AddRange(await CreateRegularUsers(userManager, roleManager));

            return ids;
        }

        // Create a list of questions, each with random created dates
        // and a random number of answers
        private static IEnumerable<Question> GetQuestions(List<string> userIds) {
            for (int i = 1; i <= QuestionsToCreate; i++) {
                var dateDiff = _random.Next(QuestionMaxAge * 2) - QuestionMaxAge;

                Question question = new Question {
                    Title = $"Question {i}",
                    Contents = LoremIpsum,
                    // Random date within -QuestionMaxAge and +QuestionMaxAge of today's date
                    Created = DateTime.Now.AddDays(dateDiff),
                    Answers = GetAnswers(dateDiff, userIds).ToList(),
                    UserId = RndId(userIds)
                };

                yield return question;
            }
        }

        // Get list of answers, length is random
        private static IEnumerable<Answer> GetAnswers(int diff, List<string> userIds) {
            for (int i = 0, mx = _random.Next(MaxAnswers); i <= mx; i++) {
                var ansDiff = _random.Next(Math.Abs(diff - QuestionMaxAge)) + diff;

                yield return new Answer {
                    Title = $"Answer {i}",
                    Contents = LoremIpsum,
                    // Random date greater than questions created date and less than
                    // the max question age
                    Created = DateTime.Now.AddDays(ansDiff),
                    UserId = RndId(userIds)
                };
            }
        }

        // Get admin credentials from appsettings.json and create admin, return admin id
        private static async Task<string> CreateAdmin(UserManager<ApplicationUser> userManager
                , RoleManager<IdentityRole> roleManager, IConfiguration config) {

            var uname = config["Users:AdminUser:Name"];
            var email = config["Users:AdminUser:Email"];
            var pw = config["Users:AdminUser:Password"];
            var role = config["Users:AdminUser:Role"];

            return await CreateUser(userManager, roleManager, uname, email, pw, role);
        }

        // Create multiple non admin users and return a list of ids
        private static async Task<IEnumerable<string>> CreateRegularUsers(
                UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager) {
            var ids = new List<string>(UsersToCreate);

            for (int i = 0; i < UsersToCreate; i++) {
                ids.Add(await CreateUser(userManager, roleManager
                    , $"Sample_User_{i}", $"u{i}@example.net", SamplePw));
            }

            return ids;
        }

        // Create or find a user and return user id, or throw an exception if the user couldn't be
        // found or created
        private static async Task<string> CreateUser(UserManager<ApplicationUser> userManager
            , RoleManager<IdentityRole> roleManager
            , string uname, string email, string pw, string role = null) {

            var user = await userManager.FindByNameAsync(uname);

            if (user == null) {

                // Create role if role is not null
                await CreateRole(role, roleManager);

                // Create user
                var newUser = new ApplicationUser { Email = email, UserName = uname };
                var result = await userManager.CreateAsync(newUser, pw);
                
                if (result.Succeeded) {

                    // Add user to role if role is not null
                    var roleResult = await AddToRole(role, userManager, newUser);

                    if (roleResult.Succeeded) {
                        return newUser.Id;
                    }
                    ThrowIdentityError(roleResult.Errors);

                } else {
                    ThrowIdentityError(result.Errors);
                }
            }

            return user.Id;
        }

        // Create a role if it doesn't exist 
        private static async Task CreateRole(string role, RoleManager<IdentityRole> manager) {
            if (role != null && !await manager.RoleExistsAsync(role)) {
                await manager.CreateAsync(new IdentityRole(role));
            }
        }

        // Add a user to a role, returns null on success or if passed null role
        private static async Task<IdentityResult> AddToRole(
            string role, UserManager<ApplicationUser> manager, ApplicationUser user) {

            if (role == null) {
                return IdentityResult.Success;
            }

            return await manager.AddToRoleAsync(user, role);
        }

        private static void ThrowIdentityError(IEnumerable<IdentityError> errs) {
            var nl = Environment.NewLine;

            throw new Exception($"Error creating sample users:{nl}"
                       + $"{string.Join(null, errs.Select(e => $"{e.Description}{nl}"))}");
        }

        // Get random id from list of user ids
        private static string RndId(List<string> li) => li[_random.Next(li.Count)];

        private const string LoremIpsum = "Lorem ipsum dolor sit amet, consectetur adipiscing elit,"
                                        + " sed do eiusmod tempor incididunt ut labore et dolore magna"
                                        + " aliqua. Ut enim ad minim veniam, quis nostrud exercitation"
                                        + " ullamco laboris nisi ut aliquip ex ea commodo consequat."
                                        + " Duis aute irure dolor in reprehenderit in voluptate velit"
                                        + " esse cillum dolore eu fugiat nulla pariatur. Excepteur"
                                        + " sint occaecat cupidatat non proident, sunt in culpa qui"
                                        + " officia deserunt mollit anim id est laborum.";
    }
}
