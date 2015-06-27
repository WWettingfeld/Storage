using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Company.StorageDomain.Interfaces;
using Company.StorageDomain.Entities;

namespace Company.StorageDomain
{
    public class SpecificWebServiceStorage<T> : IStorage<T>
    {
        private string TypeName { get; set; }

        public SpecificWebServiceStorage()
        {
            TypeName = typeof(T).Name;
        }

        public T GetById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> ExecuteQueryEntity(IQuery query)
        {
            throw new NotImplementedException();
        }

        public int ExecuteQueryScalar(IQuery query)
        {
            throw new NotImplementedException();
        }

        public void ExecuteNonQuery(IQuery query)
        {
            throw new NotImplementedException();
        }

        public int Insert(T entity)
        {
            throw new NotImplementedException();
        }

        public void Update(T entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
