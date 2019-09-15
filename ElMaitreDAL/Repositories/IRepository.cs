using ElMaitre.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ElMaitre.DAL.Repositories
{
    public interface IRepository<T> where T : BaseModel
    {
        IEnumerable<T> GetAll();
        T Get(long id);
        IQueryable<T> Get(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "");
        void Insert(T entity);
        int InsertAll(IEnumerable<T> items);
        void Update(T entity);
        void Delete(T entity);
        void DeleteAll(IEnumerable<T> items);
        void Remove(T entity);
        void SaveChanges();
    }
}
