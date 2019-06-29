using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Enquirio.Data { 
    public interface IRepositoryEnq {

        void Create<T>(T entity) where T : class, IEntity;

        T GetById<T>(object id
                , string[] navPropCollections
                , string[] navPropFks) where T : class, IEntity;
        Task<T> GetByIdAsync<T>(object id
                , string[] navPropCollections
                , string[] navPropFks) where T : class, IEntity;

        List<T> Get<T>(Expression<Func<T, bool>> filter
                , Expression<Func<T, IComparable>> orderBy
                , bool sortDesc
                , int? take
                , int? skip
                , string[] includedNavProps) where T : class, IEntity;
        List<T> GetAll<T>(Expression<Func<T, bool>> filter
                , Expression<Func<T, IComparable>> orderBy
                , bool sortDesc
                , int? take
                , int? skip
                , string[] includedNavProps) where T : class, IEntity;
        Task<List<T>> GetAsync<T>(Expression<Func<T, bool>> filter
                , Expression<Func<T, IComparable>> orderBy
                , bool sortDesc
                , int? take
                , int? skip
                , string[] includedNavProps) where T : class, IEntity;
        Task<List<T>> GetAllAsync<T>(Expression<Func<T, bool>> filter
                , Expression<Func<T, IComparable>> orderBy
                , bool sortDesc
                , int? take
                , int? skip
                , string[] includedNavProps) where T : class, IEntity;

        void Delete<T>(T entity) where T : class, IEntity;
        void DeleteById<T>(object id) where T : class, IEntity;

        void Update<T>(T entity) where T : class, IEntity;

        void Save();
        Task SaveAsync();
    }
}
