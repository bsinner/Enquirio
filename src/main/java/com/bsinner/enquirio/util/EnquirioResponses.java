package com.bsinner.enquirio.util;

import javax.ws.rs.core.Response;

/**
 * Utility class with common responses used in this app.
 *
 * @author bsinner
 */
public class EnquirioResponses {

    /**
     * Gets unauthorized response.
     *
     * @param message the entity message
     * @return        the 401 unauthorized response
     */
    public static Response get401(String message) {
        return Response.status(401).entity(message).build();
    }

    /**
     * Gets 403 forbidden response.
     *
     * @param message the entity message
     * @return        the 403 forbidden response
     */
    public static Response get403(String message) {
        return Response.status(403).entity(message).build();
    }

    /**
     * Gets 404 not found response.
     *
     * @param message the entity message
     * @return        the 404 not found response
     */
    public static Response get404(String message) {
        return Response.status(404).entity(message).build();
    }

}
