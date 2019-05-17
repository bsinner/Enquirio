package com.bsinner.enquirio.api;

import com.bsinner.enquirio.entity.Question;
import com.bsinner.enquirio.persistence.GenericDAO;
import com.bsinner.enquirio.util.EnquirioResponses;
import com.bsinner.enquirio.util.UserAccessTokenParser;
import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.fasterxml.jackson.databind.node.ObjectNode;
import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;

import javax.ws.rs.*;
import javax.ws.rs.core.Cookie;
import javax.ws.rs.core.MediaType;
import javax.ws.rs.core.Response;

/**
 * Endpoint to edit question.
 *
 * @autor bsinner
 */
@Path("/editQuestion")
public class EditQuestion {

    private final Logger logger = LogManager.getLogger(this.getClass());

    /**
     * Edit a questions title and/or contents.
     *
     * @param cookie   access token with current user data
     * @param qId      question to edit id
     * @param title    the new title
     * @param contents the new contents
     * @return         JSON response with updated question
     */
    @POST
    @Produces({ MediaType.APPLICATION_JSON })
    public Response editQuestion(
            @CookieParam("access_token") Cookie cookie
            , @QueryParam("q_id") String qId
            , @HeaderParam("question-title") String title
            , @HeaderParam("question-contents") String contents
            ) {

        Question question = new GenericDAO<>(Question.class).getById(Integer.parseInt(qId));

        if (question == null) {
            return EnquirioResponses.get404("Error 404: Question not found");
        }

        Integer user_id = new UserAccessTokenParser().jaxRsGetUserId(cookie);

        if (user_id == null) {
            return EnquirioResponses.get401("Error 401: No user detected");
        }

        if (question.getUser().getId() != user_id) {
            return EnquirioResponses.get403("Error 403: Question does not belong to current user");
        }

        Question updatedQuestion = preformEdits(question, title, contents);

        return Response.ok()
                .entity(createEditJson(updatedQuestion))
                .build();
    }

    /**
     * Add the passed in edits to the question, save it to the database, and return the
     * updated question
     *
     * @param question the question to edit
     * @param title    the new title
     * @param contents the new contents
     * @return         the updated question
     */
    private Question preformEdits(Question question, String title, String contents) {
        int id = question.getId();

        var dao = new GenericDAO<>(Question.class);

        question.setTitle(title);
        question.setContents(contents);

        dao.saveOrUpdate(question);

        return dao.getById(id);
    }

    /**
     * Create a JSON string representing the changes made
     * to a question
     *
     * @param question the edited question
     * @return         JSON string with the new title
     *                 and contents
     */
    private String createEditJson(Question question) {
        ObjectMapper mapper = new ObjectMapper();
        ObjectNode root = mapper.createObjectNode();

        root.put("title", question.getTitle());
        root.put("contents", question.getContents());

        try {
            return mapper.writerWithDefaultPrettyPrinter().writeValueAsString(root);
        } catch (JsonProcessingException jpe) {
            logger.trace(jpe);
        }

        return "{}";
    }
}
