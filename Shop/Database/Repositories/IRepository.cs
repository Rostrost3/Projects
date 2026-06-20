using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Database.Repositories
{
    public interface IRepository<T> where T : class
    {
        List<T> GetAll();

        List<T> GetAllByFunc(Expression<Func<T, bool>> predicate);

        T? GetByFunc(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);

        void Add(T entity);

        void Update(T entity);

        void Delete(T entity);
    }
}
