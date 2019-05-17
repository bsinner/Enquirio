package com.bsinner.enquirio.entity;

import com.bsinner.enquirio.util.DurationMessage;
import lombok.*;
import org.hibernate.annotations.GenericGenerator;
import javax.persistence.*;
import java.time.Duration;
import java.time.LocalDateTime;
import java.util.HashSet;
import java.util.Set;

/**
 * Class to represent a question.
 *
 * @author bsinner
 */
@Data
@Entity(name = "Question")
@Table(name = "question")
@ToString(exclude = {"answers", "user"})
@NoArgsConstructor
@AllArgsConstructor
@EqualsAndHashCode(exclude = {"answers", "user"})
public class Question {

    @Id
    @GeneratedValue(strategy = GenerationType.AUTO, generator = "native")
    @GenericGenerator(name = "native", strategy = "native")
    private int id;

    private String title;
    private String contents;

    @Column(name = "created_date", insertable = false)
    private LocalDateTime createdDate;

    @Column(name = "edit_date")
    private LocalDateTime editDate;

    @Column(name = "edited")
    private boolean isEdited;

    @OneToMany(mappedBy = "question", cascade = CascadeType.ALL, orphanRemoval = true, fetch = FetchType.EAGER)
    private Set<Answer> answers = new HashSet<>();

    @ManyToOne
    @JoinColumn(name = "user_id")
    private User user;

    /**
     * Constructor with fields required by the database.
     *
     * @param title    the question title
     * @param contents the question contents
     * @param user     the question author
     */
    public Question(String title, String contents, User user) {
        this.title = title;
        this.contents = contents;
        this.user = user;
    }

}
