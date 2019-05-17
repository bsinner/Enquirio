package com.bsinner.enquirio.persistence;

import org.hibernate.SessionFactory;
import org.hibernate.boot.Metadata;
import org.hibernate.boot.MetadataSources;
import org.hibernate.boot.registry.StandardServiceRegistry;
import org.hibernate.boot.registry.StandardServiceRegistryBuilder;

/**
 * Setup and provide instance of SessionFactory.
 *
 * @author bsinner
 */
public class SessionFactoryProvider {

    private static SessionFactory factory;

    /**
     * Setup the SessionFactory.
     */
    private static void setUpSessionFactory() {
        StandardServiceRegistry registry = new StandardServiceRegistryBuilder()
                .configure("hibernate.cfg.xml")
                .build();

        Metadata metadata = new MetadataSources(registry)
                .getMetadataBuilder()
                .build();

        factory = metadata.getSessionFactoryBuilder().build();
    }

    /**
     * Provide the current SessionFactory.
     *
     * @return the SessionFactory
     */
    public static SessionFactory getSessionFactory() {
        if (factory == null) setUpSessionFactory();
        return factory;
    }
}

