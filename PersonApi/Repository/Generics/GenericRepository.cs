using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PersonApi.Models.Base;
using PersonApi.Models.Context;

namespace PersonApi.Repository.Generics
{
    public class GenericRepository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly MySQLContext _context;
        private readonly DbSet<T> dataset;

        public GenericRepository(MySQLContext context)
        {
            _context = context;
            dataset = _context.Set<T>();
        }

        public T Create(T item)
        {
            try
            {
                dataset.Add(item);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }

            return item;
        }

        public T FindById(long id)
        {
            var result = dataset.SingleOrDefault(i => i.Id.Equals(id));
            if (result == null) return null;
            return result;
        }

        public List<T> FindAll()
        {
            return dataset.ToList();
        }

        public T Update(T item)
        {
            if (!Exists(item.Id)) return null;
            var result = dataset.SingleOrDefault(i => i.Id.Equals(item.Id));

            try
            {
                _context.Entry(result).CurrentValues.SetValues(item);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }

            return item;
        }

        public void Delete(long id)
        {
            var result = dataset.SingleOrDefault(i => i.Id.Equals(id));
            try
            {
                if (result != null)
                {
                    dataset.Remove(result);
                    _context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool Exists(long? id)
        {
            return dataset.Any(i => i.Id.Equals(id));
        }
    }
}