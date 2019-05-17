DELETE FROM question;

INSERT INTO question (id, title, contents) VALUES
    (1, "Title1", "Question Contents1")
    , (2, "Title2", "Question Contents2")
    , (3, "Title3", "Question Contents3");

INSERT INTO question (id, title, contents, edit_date, edited, user_id) VALUES
    (4, "Title4", "Question Contents4", "2020-03-23", TRUE, 4)
    , (5, "Title5", "Question Contents5", "2019-08-10", TRUE, 2);