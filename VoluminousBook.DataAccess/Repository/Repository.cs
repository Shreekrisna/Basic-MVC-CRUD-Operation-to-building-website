using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VoluminousBook.DataAccess.Repository.IRepository;

namespace VoluminousBook.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbset;

        public Repository(ApplicationDbContext db)
        {
            _db = db;
            this.dbset = _db.Set<T>();
        }

        public void Add(T entity)
        {
            dbset.Add(entity);
        }

        // includeProp-"Category,CoverType"
        public IEnumerable<T> GetAll(string? includeProperies = null)
        {
            IQueryable<T> query = dbset;
            if (includeProperies != null)
            {
                foreach (var includeProp in includeProperies.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                query = query.Include(includeProp);
                }
            }
            return query.ToList();
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperies = null)
        {
            IQueryable<T> query = dbset;
            query=query.Where(filter);
            if (includeProperies != null)
            {
                foreach (var includeProp in includeProperies.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query.FirstOrDefault();
        }

        public void Remove(T entity)
        {
            dbset.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            dbset.RemoveRange(entity);
        }
    }
}
