using System;
using System.Collections.Generic;
using System.Text;

// Sample data for test classes
namespace Enquirio.Tests.Util {
    class SampleData {
        
        public static List<string> Questions() => new List<string> {
              "INSERT INTO Question (title, contents, Id, Created) VALUES ('Q1', '...', 1, '1972-01-02 12:34:56');"
            , "INSERT INTO Question (title, contents, Id, Created) VALUES ('Q2', '...', 2, '1972-01-02 12:34:56');"
            , "INSERT INTO Question (title, contents, Id, Created) VALUES ('Q3', '...', 3, '1972-01-02 12:34:56');"
            , "INSERT INTO Question (title, contents, Id, Created) VALUES ('Q4', '...', 4, '1972-01-02 12:34:56');"
            , "INSERT INTO Question (title, contents, Id, Created) VALUES ('Q5', '...', 5, '1972-01-02 12:34:56');"
            , "INSERT INTO Question (title, contents, Id, Created) VALUES ('Q6', '...', 6, '1972-01-02 12:34:56');"
            , "INSERT INTO Question (title, contents, Id, Created) VALUES ('Q7', '...', 7, '1972-01-02 12:34:56');"
            , "INSERT INTO Question (title, contents, Id, Created) VALUES ('Q8', '...', 8, '1972-01-02 12:34:56');"
            , "INSERT INTO Question (title, contents, Id, Created) VALUES ('Q9', '...', 9, '1972-01-02 12:34:56');"

            , "INSERT INTO Answers (title, contents, Id, QuestionId, Created) VALUES ('A3', '...', 1, 1, '1972-01-02 12:34:56')"
            , "INSERT INTO Answers (title, contents, Id, QuestionId, Created) VALUES ('A2', '...', 2, 1, '1972-01-02 12:34:56')"
            , "INSERT INTO Answers (title, contents, Id, QuestionId, Created) VALUES ('A4', '...', 3, 1, '1972-01-02 12:34:56')"
            , "INSERT INTO Answers (title, contents, Id, QuestionId, Created) VALUES ('A1', '...', 4, 2, '1972-01-02 12:34:56')"
        };

    }
}
