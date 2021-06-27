using EjemploApiRest.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EjemploApiRest.DataAccess
{
    public class DbContext<T> : IDbContext<T> where T : class,IEntity
    {
        DbSet<T> _items;

        ApiDbContext _apiDbContext;

        public DbContext(ApiDbContext apiDbContext)
        {
            _apiDbContext = apiDbContext;
            _items = apiDbContext.Set<T>();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IList<T> GetAll()
        {
            return _items.ToList();
        }

        public T GetById(int id)
        {
            return _items.Where(x => x.Id.Equals(id)).FirstOrDefault();
        }

        public T Save(T entity)
        {
            _items.Add(entity);
            _apiDbContext.SaveChanges();
            return entity;
        }
    }
}
