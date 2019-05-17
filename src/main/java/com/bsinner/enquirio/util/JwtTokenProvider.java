package com.bsinner.enquirio.util;

import io.jsonwebtoken.JwtBuilder;
import io.jsonwebtoken.Jwts;
import io.jsonwebtoken.SignatureAlgorithm;
import io.jsonwebtoken.security.Keys;

import java.util.Collections;
import java.util.Date;
import java.util.Map;

/**
 * Class for creating JWT tokens.
 *
 * @author bsinner
 */
public class JwtTokenProvider {

    private static final long DEFAULT_LIFESPAN = 60 * 60000;
    private static final String ISSUER = "Enquirio";
    private static final String USER_ID_CLAIM = "uid";

    /**
     * Get token with default lifespan.
     *
     * @param claims the token claims
     * @return       the token string
     */
    public String getToken(Map<String, Object> claims) {
        return createToken(claims, DEFAULT_LIFESPAN);
    }

    /**
     * Get token with expiration time specified.
     *
     * @param claims   the token claims
     * @param lifespan length of time token is valid
     * @return         the token string
     */
    public String getToken(Map<String, Object> claims, long lifespan) {
        return createToken(claims, lifespan);
    }

    /**
     * Get token with specified claims, user id, and lifespan.
     *
     * @param claims   the token claims
     * @param user_id  the user id claim
     * @param lifespan length of time token is valid
     * @return         the token string
     */
    public String getToken(Map<String, Object> claims, int user_id, long lifespan) {
        claims.put(USER_ID_CLAIM, user_id);
        return createToken(claims, lifespan);
    }

    /**
     * Get token with user id claim and specified lifespan.
     *
     * @param user_id  the user id claim
     * @param lifespan length of time token is valid
     * @return         the token string
     */
    public String getToken(int user_id, long lifespan) {
        return createToken(Collections.singletonMap(USER_ID_CLAIM, user_id), lifespan);
    }

    /**
     * Get token with user id claim.
     *
     * @param user_id the user id claim
     * @return        the token string
     */
    public String getToken(int user_id) {
        return createToken(Collections.singletonMap(USER_ID_CLAIM, user_id), DEFAULT_LIFESPAN);
    }


    /**
     * Create a new token.
     *
     * @param claims   the token claims
     * @param lifespan length of time token is valid
     * @return         the token string
     */
    private String createToken(Map<String, Object> claims, long lifespan) {
        Date expiration = new Date();
        expiration.setTime(expiration.getTime() + lifespan);

        JwtBuilder builder = Jwts.builder()
                .setIssuer(ISSUER)
                .setIssuedAt(new Date())
                .setClaims(claims)
                .setExpiration(expiration)
                .signWith(
                        Keys.hmacShaKeyFor(getSecret())
                        , SignatureAlgorithm.HS256
                );

        return builder.compact();
    }

    /**
     * Get the current secret used to sign tokens.
     *
     * @return byte array of the secret string
     */
    private byte[] getSecret() {
        return new JwtSecretLoader().getSecret().getBytes();
    }

}
