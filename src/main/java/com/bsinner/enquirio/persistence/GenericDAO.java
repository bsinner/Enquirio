package com.bsinner.enquirio.persistence;

import org.hibernate.Session;
import org.hibernate.SessionFactory;
import org.hibernate.Transaction;

import javax.persistence.criteria.CriteriaBuilder;
import javax.persistence.criteria.CriteriaQuery;
import javax.persistence.criteria.Predicate;
import javax.persistence.criteria.Root;
import java.util.*;

/**
 * Generic DAO with CRUD methods.
 *
 * @param <T> the DAO type
 * @author    bsinner
 */
public class GenericDAO<T> {

    private final SessionFactory sessionFactory = SessionFactoryProvider.getSessionFactory();
    private Class<T> type;

    /**
     * Constructor to provide generic dao with a type.
     *
     * @param type the entity type to be accessed
     */
    public GenericDAO(Class<T> type) {
        this.type = type;
    }

    /**
     * No args constructor.
     */
    public GenericDAO() {}

    /**
     * Setter for type.
     *
     * @param type the entity type
     */
    public void setType(Class<T> type) {
        this.type = type;
    }

    /**
     * Get all results.
     *
     * @return list of all entities
     */
    public List<T> getAll() {
        Session session = sessionFactory.openSession();

        CriteriaBuilder builder = session.getCriteriaBuilder();
        CriteriaQuery<T> query = builder.createQuery(type);
        Root<T> root = query.from(type);

        List<T> results = session.createQuery(query).getResultList();

        session.close();
        return results;
    }

    /**
     * Get results by property equal.
     *
     * @param column the property to search by
     * @param query  the value to search for
     * @return       list of found entities
     */
    public List<T> getByPropertyEqual(String column, String query) {
        Session session = sessionFactory.openSession();

        CriteriaBuilder builder = session.getCriteriaBuilder();
        CriteriaQuery<T> criteria = builder.createQuery(type);
        Root<T> root = criteria.from(type);

        criteria.where(builder.equal(root.get(column), query));

        List<T> results = session.createQuery(criteria).getResultList();

        session.close();
        return results;
    }

    /**
     * Get results by property like.
     *
     * @param column the property to search by
     * @param query  the value to search for
     * @return       list of found entities
     */
    public List<T> getByPropertyLike(String column, String query) {
        Session session = sessionFactory.openSession();

        CriteriaBuilder builder = session.getCriteriaBuilder();
        CriteriaQuery<T> criteria = builder.createQuery(type);
        Root<T> root = criteria.from(type);

        criteria.where(builder.like(root.get(column), "%" + query + "%"));

        List<T> results = session.createQuery(criteria).getResultList();

        session.close();
        return results;
    }

    /**
     * Get entities by id
     *
     * @param id the id to search for
     * @return   the found entity
     */
    public T getById(int id) {
        Session session = sessionFactory.openSession();

        T entity = (T)session.get(type, id);

        session.close();
        return entity;
    }

    /**
     * Save entity, or update entity's properties.
     *
     * @param entity the entity to save or update
     */
    public void saveOrUpdate(T entity) {
        Session session = sessionFactory.openSession();

        Transaction transaction = session.beginTransaction();
        session.saveOrUpdate(entity);
        transaction.commit();

        session.close();
    }

    /**
     * Save entity.
     *
     * @param entity the entity to save
     * @return       the id of the saved entity
     */
    public int insert(T entity) {
        Session session = sessionFactory.openSession();

        Transaction transaction = session.beginTransaction();
        int id = (int)session.save(entity);
        transaction.commit();

        session.close();
        return id;
    }

    /**
     * Delete entity.
     *
     * @param entity the entity to delete.
     */
    public void delete(T entity) {
        Session session = sessionFactory.openSession();

        Transaction transaction = session.beginTransaction();
        session.delete(entity);
        transaction.commit();

        session.close();
    }

    /**
     * Get by properties equal.
     *
     * @param properties map of property names and values
     * @return           the found entities
     */
    public List<T> getByPropertiesEqual(Map<String, String> properties) {
        Session session = sessionFactory.openSession();

        CriteriaBuilder builder = session.getCriteriaBuilder();
        CriteriaQuery<T> criteria = builder.createQuery(type);
        Root<T> root = criteria.from(type);

        List<Predicate> predicates = new ArrayList<>();

        for (var property : properties.entrySet()) {
            predicates.add(builder.equal(root.get(property.getKey()), property.getValue()));
        }

        criteria.select(root).where(builder.and(predicates.toArray(new Predicate[predicates.size()])));

        List<T> results = session.createQuery(criteria).getResultList();

        session.close();
        return results;
    }
}
