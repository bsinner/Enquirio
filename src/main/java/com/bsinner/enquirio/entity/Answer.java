package com.bsinner.enquirio.entity;

import com.bsinner.enquirio.util.DurationMessage;
import lombok.*;
import org.hibernate.annotations.GenericGenerator;

import javax.persistence.*;
import java.time.LocalDateTime;

/**
 * Class for an question answer.
 *
 * @author bsinner
 */
@Data
@Entity(name = "Answer")
@Table(name = "answer")
@ToString(exclude = {"question", "user"})
@NoArgsConstructor
@AllArgsConstructor
@EqualsAndHashCode(exclude = {"question", "user"})
public class Answer {

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

    @ManyToOne
    @JoinColumn(name = "q_id")
    private Question question;

    @ManyToOne
    @JoinColumn(name = "user_id")
    private User user;

    /**
     * Constructor with fields required by database.
     *
     * @param title    the answer title
     * @param contents the answer contents
     * @param question the answered question
     * @param user     the answer author
     */
    public Answer(String title, String contents, Question question, User user) {
        this.title = title;
        this.contents = contents;
        this.question = question;
        this.user = user;
    }

    // TODO: find another way to pass this data to the webpage
    public String getTimeSince() {
        return DurationMessage.timeSince(createdDate);
    }

}
