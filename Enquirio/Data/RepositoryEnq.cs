using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

// Generic repository following example on:
// https://cpratt.co/truly-generic-repository/
//
namespace Enquirio.Data {
    public class RepositoryEnq : IRepositoryEnq {

        private readonly DbContext _context;

        public RepositoryEnq(DbContextEnq context) {
            _context = context;
        }

        public void Create<T>(T entity) where T : class, IEntity {
            if (entity is IPost) {
                ((IPost) entity).Created = DateTime.Now;
            }

            ContextAdd(entity);
        }

        public void Delete<T>(T entity) where T : class, IEntity {
            _context.Remove(entity);
        }

        public void DeleteById<T>(object id) where T : class, IEntity {
            var entity = GetById<T>(id);

            if (entity != null) {
                Delete(entity);
            }
        }

        public void Update<T>(T entity) where T : class, IEntity {
            if (entity is IPost) {
                ((IPost) entity).Edited = DateTime.Now;
            } 

            ContextUpdate(entity);
        }

        // If ordering by multiple properties, order results in the calling class,
        // or create a repository that extends this repo
        public List<T> Get<T>(Expression<Func<T, bool>> filter
                , Expression<Func<T, IComparable>> orderBy = null
                , bool sortDesc = false
                , int? take = null
                , int? skip = null
                , string[] includedNavProps = null) where T : class, IEntity {

            return ContextGet<T>
                (filter, orderBy, sortDesc, take, skip, includedNavProps).ToList();
        }

        public List<T> GetAll<T>(Expression<Func<T, bool>> filter = null
                , Expression<Func<T, IComparable>> orderBy = null
                , bool sortDesc = false
                , int? take = null
                , int? skip = null
                , string[] includedNavProps = null) where T : class, IEntity {

            return ContextGet<T>
                (filter, orderBy, sortDesc, take, skip, includedNavProps).ToList();
        }

        public Task<List<T>> GetAsync<T>(Expression<Func<T, bool>> filter
                , Expression<Func<T, IComparable>> orderBy = null
                , bool sortDesc = false
                , int? take = null
                , int? skip = null
                , string[] includedNavProps = null) where T : class, IEntity {

            return ContextGet
                (filter, orderBy, sortDesc, take, skip, includedNavProps).ToListAsync();
        }

        public Task<List<T>> GetAllAsync<T>(Expression<Func<T, bool>> filter = null
            , Expression<Func<T, IComparable>> orderBy = null
            , bool sortDesc = false
            , int? take = null
            , int? skip = null
            , string[] includedNavProps = null) where T : class, IEntity {

            return ContextGet<T>
                (filter, orderBy, sortDesc, take, skip, includedNavProps).ToListAsync();
        }

        public T GetById<T>(object id
                , string[] navPropCollections = null
                , string[] navPropFks = null) where T : class, IEntity {

            ConvertId(ref id);

            var entity = _context.Set<T>().Find(id);

            AddProps(navPropCollections
                , p => _context.Entry(entity).Collection(p).Load());
            AddProps(navPropFks
                , p => _context.Entry(entity).Reference(p).Load());

            return entity;
        }

        public async Task<T> GetByIdAsync<T>(object id
                , string[] navPropCollections = null
                , string[] navPropFks = null) where T : class, IEntity {

            ConvertId(ref id);

            var entity = await _context.Set<T>().FindAsync(id);

            AddProps(navPropCollections, async p => 
                    await _context.Entry(entity).Collection(p).LoadAsync());
            AddProps(navPropFks, async p => 
                    await _context.Entry(entity).Reference(p).LoadAsync());

            return entity;
        }

        public void Save() => _context.SaveChanges();
        public Task SaveAsync() => _context.SaveChangesAsync();

        // Used by add and update methods
        private void ContextAdd<T>(T entity) where T : class {
            _context.Set<T>().Add(entity);
        }

        private void ContextUpdate<T>(T entity) where T : class {
            _context.Update(entity);
        }

        // Used by get methods, all properties are optional
        private IQueryable<T> ContextGet<T>(
                Expression<Func<T, bool>> filter = null
                , Expression<Func<T, IComparable>> orderBy = null
                , bool sortDesc = false
                , int? take = null
                , int? skip = null
                , string[] includedNavProps = null) where T : class {

            IQueryable<T> query = _context.Set<T>();

            if (filter != null) {
                query = query.Where(filter);
            }

            if (orderBy != null) {
                query = sortDesc
                    ? query.OrderByDescending(orderBy)
                    : query.OrderBy(orderBy);
            }

            AddProps(includedNavProps
                    , p => query = query.Include(p));

            if (take != null) query = query.Take(take.Value);
            if (skip != null) query = query.Skip(skip.Value);

            return query;
        }

        // AddProps uses a callback because nav properties are added differently 
        // in GetById and Get/GetAll
        private void AddProps(string[] props, Action<string> addProp) {
            if (props == null) return;
            foreach (var p in props) addProp(p); 
        }

        // Parse string ids to int
        private void ConvertId(ref object id) {
            if (id is string) id = int.Parse((string)id);
        }

    }
}
