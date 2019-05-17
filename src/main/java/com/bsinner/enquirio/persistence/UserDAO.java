package com.bsinner.enquirio.persistence;

import com.bsinner.enquirio.entity.User;

/**
 * Dao with specialized functionality for user entities.
 *
 * @author bsinner
 */
public class UserDAO extends GenericDAO<User> {

    private static final String PREFIX = "Guest_";

    /**
     * No parameter constructor for UserDAO.
     */
    public UserDAO() {
        setType(User.class);
    }

    /**
     * Create a new anonymous user.
     *
     * @return the created user
     */
    public User createAnonymousUser() {
       User user = new User();
       int id = insert(user);

       user.setUsername(PREFIX + id);

       saveOrUpdate(user);

       return user;
    }

}
