package com.bsinner.enquirio.api;

import javax.ws.rs.GET;
import javax.ws.rs.Path;
import javax.ws.rs.core.Response;

@Path("/hello")
public class HelloWorld {

    @GET
    public Response ping() {
        return Response.ok().entity("HELLO WORLD").build();
    }
}
