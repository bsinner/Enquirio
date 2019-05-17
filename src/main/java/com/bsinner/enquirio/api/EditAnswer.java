package com.bsinner.enquirio.api;

import com.bsinner.enquirio.entity.Answer;
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
import javax.ws.rs.core.Response;

@Path("/editAnswer")
public class EditAnswer {

    private final Logger logger = LogManager.getLogger(this.getClass());

    /**
     * Edit an answer's title and/or contents.
     *
     * @param cookie   the access token cookie
     * @param aId      the answer id
     * @param title    the new title
     * @param contents the new contents
     * @return         JSON response with updated answer
     */
    @POST
    public Response editAnswer(
            @CookieParam("access_token") Cookie cookie
            , @QueryParam("a_id") String aId
            , @HeaderParam("answer-title") String title
            , @HeaderParam("answer-contents") String contents) {

        Answer answer = new GenericDAO<>(Answer.class).getById(Integer.parseInt(aId));

        if (answer == null) {
            return EnquirioResponses.get404("Error 404: Answer not found");
        }

        Integer userId = new UserAccessTokenParser().jaxRsGetUserId(cookie);

        if (userId == null) {
            return EnquirioResponses.get401("Error 401: No user detected");
        }

        if (answer.getUser().getId() != userId) {
            return EnquirioResponses.get403("Error 403: Answer does not belong to current user");
        }

        Answer updatedAnswer = preformEdits(answer, title, contents);

        return Response.ok()
                .entity(createEditJson(updatedAnswer))
                .build();
    }

    /**
     * Update the answer, add it to the database, and return the updated answer.
     *
     * @param answer   the answer to update
     * @param title    the new title
     * @param contents the new contents
     * @return         the updated answer
     */
    private Answer preformEdits(Answer answer, String title, String contents) {
        int id = answer.getId();

        var dao = new GenericDAO<>(Answer.class);

        answer.setTitle(title);
        answer.setContents(contents);

        dao.saveOrUpdate(answer);

        return dao.getById(id);
    }

    private String createEditJson(Answer answer) {
        ObjectMapper mapper = new ObjectMapper();
        ObjectNode root = mapper.createObjectNode();

        root.put("title", answer.getTitle());
        root.put("contents", answer.getContents());
        root.put("id", answer.getId());
        root.put("userId", answer.getUser().getId());

        try {
            return mapper.writerWithDefaultPrettyPrinter().writeValueAsString(root);
        } catch (JsonProcessingException jpe) {
            logger.trace(jpe);
        }

        return "{}";
    }

}
