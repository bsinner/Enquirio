package com.bsinner.enquirio.api;

import com.bsinner.enquirio.entity.Answer;
import com.bsinner.enquirio.entity.Question;
import com.bsinner.enquirio.entity.User;
import com.bsinner.enquirio.persistence.GenericDAO;
import com.bsinner.enquirio.util.CookieProcessor;
import com.bsinner.enquirio.util.DurationMessage;
import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.fasterxml.jackson.databind.node.ObjectNode;
import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;
import javax.servlet.ServletContext;
import javax.ws.rs.*;
import javax.ws.rs.core.*;

@Path("/answer")
public class PostAnswer {

    private final Logger logger = LogManager.getLogger(this.getClass());

    /**
     * Submit an answer and return details about the answer in JSON.
     *
     * @param cookie     the access token cookie, if null one will be created that points to
     *                   a new anonymous user
     * @param context    the Servlet context, needed for Path variable when writing cookies
     * @param questionId the question id, if invalid 404 is returned
     * @param title      the answer title
     * @param contents   the answer contents
     * @return           JSON details about the created answer
     */
    @POST
    @Produces({MediaType.APPLICATION_JSON})
    public Response postAnswer(
            @CookieParam("access_token") Cookie cookie
            , @Context ServletContext context
            , @QueryParam("q_id") String questionId
            , @HeaderParam("answer-title") String title
            , @HeaderParam("answer-contents") String contents) {

        CookieProcessor processor = new CookieProcessor();
        User user = processor.getElseCreateUserJaxRs(cookie);
        String token = processor.getCookieStringJaxRs(context.getContextPath());

        Question question = new GenericDAO<>(Question.class).getById(Integer.valueOf(questionId));

        if (question == null) {
            return questionNotFoundResponse();
        }

        return getOkResponse(
                getAnswer(title, contents, question, user)
                , token
        );

    }

    /**
     * Return an ok response with an access token included if a new one was created.
     *
     * @param answer the answer JSON details
     * @param token  the access token, null if no token was created
     * @return       the ok response
     */
    private Response getOkResponse(Answer answer, String token) {
        String answerJson = createAnswerJson(answer);

        if (token == null) {
            return Response.ok().entity(answerJson).build();
        }

        return Response.ok().header(HttpHeaders.SET_COOKIE, token).entity(answerJson).build();
    }

    /**
     * Create JSON to represent the details of an answer.
     *
     * @param answer the answer to summarize
     * @return       the answer JSON
     */
    private String createAnswerJson(Answer answer) {
        ObjectMapper mapper = new ObjectMapper();
        ObjectNode root = mapper.createObjectNode();

        root.put("id", answer.getId());
        root.put("title", answer.getTitle());
        root.put("contents", answer.getContents());
        root.put("timeString", DurationMessage.timeSince(answer.getCreatedDate()));
        root.put("author", answer.getUser().getUsername());

        try {
            return mapper.writerWithDefaultPrettyPrinter().writeValueAsString(root);
        } catch (JsonProcessingException jpe) {
            logger.trace(jpe);
        }

        return "{}";
    }

    /**
     * Return a 404 response indicating the user tried to answer an unknown question.
     *
     * @return the 404 response
     */
    private Response questionNotFoundResponse() {
        return Response.status(404)
                .entity("Error 404: the question you are trying to answer could not be found")
                .build();
    }

    /**
     * Create an answer, and submit it to the database.
     *
     * @param title    the answer title
     * @param contents the answer contents
     * @param question the question answered
     * @param user     the answer author
     * @return         the inserted answer, fetched from the database
     */
    private Answer getAnswer(String title, String contents, Question question, User user) {
        Answer answer = new Answer(title, contents, question, user);

        var dao = new GenericDAO<>(Answer.class);
        int id = dao.insert(answer);

        return dao.getById(id);
    }

}
