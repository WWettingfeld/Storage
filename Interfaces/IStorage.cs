using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.StorageDomain.Interfaces
{
    public interface IStorage<T>
    {
        T GetById(int id);
        IEnumerable<T> GetAll();
        IEnumerable<T> ExecuteQueryEntity(IQuery query);
        int ExecuteQueryScalar(IQuery query);
        void ExecuteNonQuery(IQuery query);
        int Insert(T entity);
        void Update(T entity);
        void Delete(int id);
    }
}
