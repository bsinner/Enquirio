package com.bsinner.enquirio.entity;

import lombok.*;
import org.hibernate.annotations.GenericGenerator;
import javax.persistence.*;
import java.util.Set;
import static javax.persistence.CascadeType.*;

/**
 * Class to represent a user.
 */
@Data
@Entity(name = "User")
@Table(name = "users")
@ToString(exclude = {"questions", "answers"})
@NoArgsConstructor
@AllArgsConstructor
@EqualsAndHashCode(exclude = {"questions", "answers"})
public class User {

    @Id
    @GeneratedValue(strategy = GenerationType.AUTO, generator = "native")
    @GenericGenerator(name = "native", strategy = "native")
    private int id;

    private String username;
    private String email;
    private String password;

    @OneToMany(mappedBy = "user", cascade = {MERGE, DETACH, PERSIST, REFRESH})
    private Set<Question> questions;

    @OneToMany(mappedBy = "user", cascade = {MERGE, DETACH, PERSIST, REFRESH})
    private Set<Answer> answers;

    /**
     * Constructor for new user without id and child entities.
     *
     * @param username the user username
     * @param email    the user email
     * @param password the user password
     */
    public User(String username, String email, String password) {
        this.username = username;
        this.email = email;
        this.password = password;
    }

    /**
     * Constructor for all fields except id.
     *
     * @param username  the user username
     * @param email     the user email
     * @param password  the user password
     * @param questions the user questions
     * @param answers   the user answers
     */
    public User(String username, String email, String password, Set<Question> questions, Set<Answer> answers) {
        this.username = username;
        this.email = email;
        this.password = password;
        this.questions = questions;
        this.answers = answers;
    }

}
