package com.bsinner.enquirio.util;

import com.bsinner.enquirio.entity.User;
import com.bsinner.enquirio.persistence.UserDAO;
import io.jsonwebtoken.*;
import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;
import java.util.Date;

/**
 * Class for validating access tokens and for parsing user
 * ids and user objects from of them.
 *
 * TODO: convert class to use optionals instead of returning null values
 *
 * @author bsinner
 */
public class UserAccessTokenParser {

    private static final String USER_ID_CLAIM = "uid";
    private final Logger logger = LogManager.getLogger(this.getClass());
    private static final UserDAO dao = new UserDAO();

    /**
     * Validate a token by trying to parse it and then checking if the expiration date is later
     * than the current date.
     *
     * @param token JWT token to validate
     * @return      true if the token is valid, false if it's invalid
     */
    public boolean validateToken(String token) {

        Jws<Claims> claims = parseToken(token);

        if (claims != null) {
            Date expire = claims.getBody().getExpiration();
            Date now = new Date();

            return expire.after(now);
        }

        return false;
    }

    /**
     * Get the user id stored in a token under the claim name described in instance
     * variable USER_ID_CLAIM.
     *
     * @param token the token to parse
     * @return      the user id, or null of no user is was found
     */
    public Integer getUserId(String token) {
        Jws<Claims> claims = parseToken(token);

        if (claims != null) {
            return (Integer)claims.getBody().get(USER_ID_CLAIM);
        }

        return null;
    }

    /**
     * Get the user contained in the current token.
     *
     * @param token the token to parse
     * @return      the found user
     */
    public User getUser(String token) {
        Integer userId = getUserId(token);

        if (userId != null) {
            return dao.getById(userId);
        }

        return null;
    }

    /**
     * Extract the user id from a JAX RS cookie containing an access token.
     *
     * @param cookie the access cookie
     * @return       the found user id, or null if no valid user id is found
     */
    public Integer jaxRsGetUserId(javax.ws.rs.core.Cookie cookie) {
        if (cookie == null) return null;

        String token = cookie.getValue();

        if (!validateToken(token)) return null;

        return getUserId(token);
    }

    /**
     * Get the claims from a token, returns null if the token is malformed and can't be parsed.
     *
     * @param token the token to parse
     * @return      the token claims, or null if the token is malformed
     */
    private Jws<Claims> parseToken(String token) {

        try {
            return Jwts.parser()
                    .setSigningKey(new JwtSecretLoader().getSecret().getBytes())
                    .parseClaimsJws(token);

        } catch (JwtException jwe) {
            logger.trace(jwe);
        }

        return null;
    }

}
