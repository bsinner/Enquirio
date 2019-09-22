using System.Collections.Generic;
using System.Linq;

// Sample data for test classes
namespace Enquirio.Tests.Util {
    class SqlTestData {

        public static readonly string UserId = "sample-user-id";
        private const string Date = "1972-01-02 12:34:56";

        public static List<string> QuestionData()
                => new List<string> { User(), Questions(), Answers() };

        private static string User()
                => CreateInsert("AspNetUsers"
                    , "Id, EmailConfirmed, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount"
                    , new [] { $"'{UserId}', 1, 1, 0, 0, 0" });

        private static string Questions() {
            string[] values = { $"'Q1', '...', 1, '{Date}', '{UserId}'"
                              , $"'Q2', '...', 2, '{Date}', '{UserId}'"
                              , $"'Q3', '...', 3, '{Date}', '{UserId}'"
                              , $"'Q4', '...', 4, '{Date}', '{UserId}'"
                              , $"'Q5', '...', 5, '{Date}', '{UserId}'"
                              , $"'Q6', '...', 6, '{Date}', '{UserId}'"
                              , $"'Q7', '...', 7, '{Date}', '{UserId}'"
                              , $"'Q8', '...', 8, '{Date}', '{UserId}'"
                              , $"'Q9', '...', 9, '{Date}', '{UserId}'"
            };

            return CreateInsert("Question", "title, contents, Id, Created, userId", values);
        }

        private static string Answers() {
            string[] values = {
                  $"'A3', '...', 1, 1, '{Date}', '{UserId}'"
                , $"'A2', '...', 2, 1, '{Date}', '{UserId}'"
                , $"'A4', '...', 3, 1, '{Date}', '{UserId}'"
                , $"'A1', '...', 4, 2, '{Date}', '{UserId}'"
            };

            return CreateInsert("Answers", "title, contents, Id, QuestionId, Created, userId", values);
        }

        private static string CreateInsert(string table, string columns, string[] values)
                => $"INSERT INTO {table} ({columns}) VALUES {FormatValues(values)}";

        private static string FormatValues(string[] values)
                => string.Join(", ", values.Select(v => $"({v})"));
    }
}
