using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Enquirio.Models;
using Microsoft.EntityFrameworkCore;

namespace Enquirio.Data {

    public class DbInitializer {

        private const int Count = 100;
        private const int MaxAnswers = 5;
        private const int MaxDays = 60;
        private static readonly Random _random = new Random();

        public static void Initialize(DbContextEnq context) {
            context.Database.EnsureCreated();

            if (context.Question.Any()) {
                return; 
            }

            context.AddRange(GetQuestions());
            context.SaveChanges();
        }

        private static IEnumerable<Question> GetQuestions() {
            for (int i = 1; i <= Count; i++) {
                var dateDiff = _random.Next(MaxDays * 2) - MaxDays;

                Question question = new Question {
                    Title = $"Question {i}",
                    Contents = LoremIpsum,
                    // Random date within -MaxDays and +MaxDays of today's date
                    Created = DateTime.Now.AddDays(dateDiff),
                    Answers = GetAnswers(dateDiff).ToList()
                };

                yield return question;
            }
        }

        // Get random number of answers, between 0 and MaxAnswers
        private static IEnumerable<Answer> GetAnswers(int diff) {
            for (int i = 0, mx = _random.Next(MaxAnswers); i <= mx; i++) {
                var ansDiff = _random.Next(Math.Abs(diff - MaxDays)) + diff;

                yield return new Answer {
                    Title = $"Answer {i}",
                    Contents = LoremIpsum,
                    // Random date greater than questions created date and less than
                    // MaxDays + questions created date
                    Created = DateTime.Now.AddDays(ansDiff)
                };
            }
        }

        private const string LoremIpsum = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, " +
                                          "sed do eiusmod tempor incididunt ut labore et dolore magna " +
                                          "aliqua. Ut enim ad minim veniam, quis nostrud exercitation " +
                                          "ullamco laboris nisi ut aliquip ex ea commodo consequat. " +
                                          "Duis aute irure dolor in reprehenderit in voluptate velit " +
                                          "esse cillum dolore eu fugiat nulla pariatur. Excepteur " +
                                          "sint occaecat cupidatat non proident, sunt in culpa qui " +
                                          "officia deserunt mollit anim id est laborum.";
    }
}
