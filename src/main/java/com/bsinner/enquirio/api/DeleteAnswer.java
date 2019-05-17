package com.bsinner.enquirio.api;

import com.bsinner.enquirio.entity.Answer;
import com.bsinner.enquirio.persistence.GenericDAO;
import com.bsinner.enquirio.util.EnquirioResponses;
import com.bsinner.enquirio.util.UserAccessTokenParser;
import javax.ws.rs.*;
import javax.ws.rs.core.Cookie;
import javax.ws.rs.core.Response;

/**
 * Endpoint for deleting answers.
 *
 * @author bsinner
 */
@Path("/deleteAnswer")
public class DeleteAnswer {

    /**
     * Delete the passed in answer.
     *
     * @param cookie   the access token cookie
     * @param answerId the answer id
     * @return         the response indicating deletion success
     */
    @DELETE
    public Response deleteAnswer(
            @CookieParam("access_token") Cookie cookie
            , @QueryParam("a_id") String answerId
    ) {
        // TODO: eliminate duplicate code?
        Answer answer = new GenericDAO<>(Answer.class)
                .getById(Integer.parseInt(answerId));

        if (answer == null) {
            return EnquirioResponses.get404("Error 404: Answer not found");
        }

        Integer userId = new UserAccessTokenParser().jaxRsGetUserId(cookie);

        if (userId == null) {
            return EnquirioResponses.get401("Error 401: No user logged in");
        }

        if (answer.getUser().getId() != userId) {
            return EnquirioResponses.get403("Error 403: Answer does not belong to the current user");
        }

        deleteAnswerFromDb(answer);

        return Response.status(204).build();
    }

    /**
     * Delete the passed in answer from the database.
     *
     * @param answer the answer to delete
     */
    private void deleteAnswerFromDb(Answer answer) {
        var dao = new GenericDAO<>(Answer.class);
        dao.delete(answer);
    }

}
