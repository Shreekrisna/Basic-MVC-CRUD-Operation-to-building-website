using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace VoluminousBook.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T:class
    {
        //Right now assume T : Category 
        //Common methods we want to implement
        T GetFirstOrDefault(Expression<Func<T, bool>> filter);
        IEnumerable<T> GetAll();
        void Add(T entity);

        void Remove(T entity);//we will receive one entity
        void RemoveRange(IEnumerable<T> entity);//We will receuve more than one entity


    }
}
