package com.bsinner.enquirio.util;

import com.bsinner.enquirio.entity.User;
import com.bsinner.enquirio.persistence.UserDAO;
import javax.servlet.http.Cookie;
import javax.servlet.http.HttpServletResponse;

/**
 * Class for creating an anonymous user and cookie if none can be found, or
 * for fetching the current user if a cookie is found. Static methods are used
 * by servlets, and the class itself is instantiated and used as an
 * object by JAX RS endpoints.
 *
 * @author bsinner
 */
public class CookieProcessor {

    private static final String COOKIE_NAME = "access_token";
    private static final long ANONYMOUS_TOKEN_EXPIRE = 60 * 60000 * 24;
    private boolean newUser;
    private User user;

    /**
     * Get the current user in a JAX RS cookie, or create one if no user
     * or valid cookie is found.
     *
     * @param cookie the cookie to process
     * @return       the found/created user
     */
    public User getElseCreateUserJaxRs(javax.ws.rs.core.Cookie cookie) {
        if (cookie == null) {
            return getNewUserJaxRs();
        } else {

            UserAccessTokenParser parser = new UserAccessTokenParser();
            String token = cookie.getValue();

            if (parser.validateToken(token)) {
                    user = parser.getUser(token);
                    return user;
            } else {
                return getNewUserJaxRs();
            }
        }
    }

    /**
     * Get the JAX RS usable cookie string from the current CookieProcessor object.
     *
     * @param path the current context path
     * @return     the cookie string
     */
    public String getCookieStringJaxRs(String path) {
        if (newUser) {
            String token = new JwtTokenProvider().getToken(user.getId(), ANONYMOUS_TOKEN_EXPIRE);

            return COOKIE_NAME + "=" + token + "; Path=" + path + "; HttpOnly";
        }

        return null;
    }

    /**
     * Set the current CookieProcessor's user object to a new anonymous
     * user and return it.
     *
     * @return the created user
     */
    private User getNewUserJaxRs() {
        newUser = true;
        user = new UserDAO().createAnonymousUser();
        return user;
    }

    /**
     * Check if a user can be found in a servlets cookies and return it. If no user is found
     * create a new user, set an access token for it in the calling servlets cookies and return
     * the created user.
     *
     * @param cookies  the servlet cookies
     * @param response the servlet HTTP response
     * @return         the created/found user
     */
    public static User findElseCreateUser(Cookie[] cookies, HttpServletResponse response) {
        String token = findToken(cookies);
        UserAccessTokenParser parser = new UserAccessTokenParser();

        if (token == null) {
            return createAndAddUser(response);
        } else {
            if (parser.validateToken(token)) {
                return parser.getUser(token);
            } else {
                return createAndAddUser(response);
            }
        }

    }

    /**
     * Create a new user, add an access token for it to the response cookies, and return
     * the created user.
     *
     * @param response the servlet HTTP response
     * @return         the created user
     */
    private static User createAndAddUser(HttpServletResponse response) {
        User user = new UserDAO().createAnonymousUser();

        String token = new JwtTokenProvider().getToken(user.getId(), ANONYMOUS_TOKEN_EXPIRE);

        Cookie cookie = new Cookie(COOKIE_NAME, token);
        cookie.setHttpOnly(true);
        response.addCookie(cookie);

        return user;
    }

    /**
     * Search an array of cookies and return an access token
     * if a cookie contains one.
     *
     * @param cookies the cookies to search
     * @return        the found token, or null if no
     *                token is found
     */
    private static String findToken(Cookie[] cookies) {
        String token = null;

        for (var cookie : cookies) {
            if (cookie.getName().equals(COOKIE_NAME)) {
                token = cookie.getValue();
            }
        }

        return token;
    }

}
