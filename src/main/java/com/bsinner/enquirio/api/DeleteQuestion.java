package com.bsinner.enquirio.api;

import com.bsinner.enquirio.entity.Question;
import com.bsinner.enquirio.persistence.GenericDAO;
import com.bsinner.enquirio.util.EnquirioResponses;
import com.bsinner.enquirio.util.UserAccessTokenParser;

import javax.ws.rs.*;
import javax.ws.rs.core.Cookie;
import javax.ws.rs.core.Response;

/**
 * Endpoint for deleting questions.
 *
 * @author bsinner
 */
@Path("/deleteQuestion")
public class DeleteQuestion {


    /**
     * Delete the passed in question.
     *
     * @param cookie     the access token cookie
     * @param questionId the question id
     * @return           response indicating success of delete
     */
    @DELETE
    public Response deleteQuestion(
            @CookieParam("access_token") Cookie cookie
            , @QueryParam("q_id") String questionId) {

        Question question = new GenericDAO<>(Question.class)
                .getById(Integer.parseInt(questionId));

        if (question == null) {
            return EnquirioResponses.get404("Error 404: Question not Found");
        }

        Integer userId = new UserAccessTokenParser().jaxRsGetUserId(cookie);

        if (userId == null) {
            return EnquirioResponses.get401("Error 401: No user logged in");
        }

        if (question.getUser().getId() != userId) {
            return EnquirioResponses.get403("Error 403: Question does not belong to current user");
        }

        deleteQuestionFromDb(question);

        return Response.status(204).build();
    }

    /**
     * Delete the passed in question from the database.
     *
     * @param question the question to delete
     */
    private void deleteQuestionFromDb(Question question) {
        var dao = new GenericDAO<>(Question.class);
        dao.delete(question);
    }
}
