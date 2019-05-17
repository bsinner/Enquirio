DROP TABLE IF EXISTS user_details, answer, question, users;

CREATE TABLE users (
    id INT NOT NULL AUTO_INCREMENT PRIMARY KEY UNIQUE
    , username VARCHAR(32) NULL
    , email VARCHAR(255) NULL
    , password VARCHAR(255) NULL
) AUTO_INCREMENT = 500;


CREATE TABLE question (
    id INT NOT NULL AUTO_INCREMENT PRIMARY KEY UNIQUE
    , title VARCHAR(50) NOT NULL
    , contents TEXT(5000) NOT NULL
    , created_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP
    , edit_date TIMESTAMP NULL
    , edited BOOLEAN DEFAULT FALSE
    , user_id INT NOT NULL
    , CONSTRAINT fk_users_question FOREIGN KEY (user_id) REFERENCES users(id)
            ON UPDATE CASCADE
);

CREATE TABLE answer (
    id INT NOT NULL AUTO_INCREMENT PRIMARY KEY UNIQUE
    , title VARCHAR(50) NOT NULL
    , contents TEXT(3000) NOT NULL
    , created_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP
    , edit_date TIMESTAMP NULL
    , edited BOOLEAN DEFAULT FALSE
    , q_id INT NOT NULL
    , user_id INT NOT NULL
    , CONSTRAINT fk_question_answer FOREIGN KEY (q_id) REFERENCES question(id)
            ON DELETE CASCADE ON UPDATE CASCADE
    , CONSTRAINT fk_users_answer FOREIGN KEY (user_id) REFERENCES users(id)
            ON UPDATE CASCADE
);


# Sample data

INSERT INTO users (id, username, email, password) VALUES
    (1, "lsmith", "lsmith@gmail.com", "password1")
    , (2, "zsmith", "zsmith@gmail.com", "password2")
    , (3, "ksmith", "ksmith@gmail.com", "password3");

INSERT INTO users (id, username) VALUES
    (4, "Guest_123")
    , (5, "Guest_122");

INSERT INTO question (id, title, contents, user_id) VALUES
    (1, "a", "has 2 answers", 1)
    , (2, "b", "has 1 answer, anonymous asker", 5)
    , (3, "c", "has 0 answers", 2);

INSERT INTO answer (title, contents, q_id, user_id) VALUES
    ("a", "...", 1, 3)
    , ("b", "anonymous answer", 1, 4)
    , ("c", "...", 2, 2);

