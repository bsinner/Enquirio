package com.bsinner.enquirio.api;

import com.bsinner.enquirio.util.UserAccessTokenParser;

import javax.ws.rs.CookieParam;
import javax.ws.rs.GET;
import javax.ws.rs.Path;
import javax.ws.rs.core.Cookie;
import javax.ws.rs.core.Response;

@Path("/currUserId")
public class GetCurrentUserId {

    @GET
    public Response getCurrentUser(@CookieParam("access_token") Cookie cookie) {
        Integer id = findId(cookie);

        if (id == null) {
            return Response.status(401).entity("Error 401: No logged in user").build();
        }

        return Response.ok().entity(id).build();
    }

    private Integer findId(Cookie cookie) {
        if (cookie == null) {
            return null;
        }

        UserAccessTokenParser parser = new UserAccessTokenParser();
        String token = cookie.getValue();

        if (!parser.validateToken(token)) {
            return null;
        }

        return parser.getUserId(token);
    }
}

// TODO: investigate other ways of getting user id