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
    public class SqlServerStorage<T> : IStorage<T>
    {
        private string ConnectionString { get; set; }

        public SqlServerStorage(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public T GetById(int id)
        {
            var item = Activator.CreateInstance<T>();

            using (var connection = new SqlConnection(ConnectionString))
            {
                var typeName = typeof(T).Name;
                var storedProcedure = String.Format("usp_{0}_GetById", typeName);

                var command = new SqlCommand(storedProcedure, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", id);

                connection.Open();
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Map(item, reader);
                }
            }

            return item;
        }

        public IEnumerable<T> GetAll()
        {
            var entities = new List<T>();

            using (var connection = new SqlConnection(ConnectionString))
            {
                var typeName = typeof(T).Name;
                var storedProcedure = String.Format("usp_{0}_GetAll", typeName);

                var command = new SqlCommand(storedProcedure, connection);
                command.CommandType = CommandType.StoredProcedure;

                connection.Open();
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var entity = Activator.CreateInstance<T>();
                    Map(entity, reader);

                    entities.Add(entity);
                }
            }

            return entities;
        }

        public int Insert(T entity)
        {
            var id = -1;

            using (var connection = new SqlConnection(ConnectionString))
            {
                var typeName = typeof(T).Name;
                var storedProcedure = String.Format("usp_{0}_Insert", typeName);

                var command = new SqlCommand(storedProcedure, connection);
                command.CommandType = CommandType.StoredProcedure;

                foreach (PropertyInfo propertyInfo in entity.GetType().GetProperties())
                {
                    if (propertyInfo.Name != typeName + "Id")
                    command.Parameters.AddWithValue("@" + propertyInfo.Name, propertyInfo.GetValue(entity));
                }

                connection.Open();
                id = (int)command.ExecuteScalar();
            }

            return id;
        }

        public void Update(T entity)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var typeName = typeof(T).Name;
                var storedProcedure = String.Format("usp_{0}_Update", typeName);

                var command = new SqlCommand(storedProcedure, connection);
                command.CommandType = CommandType.StoredProcedure;

                foreach (PropertyInfo propertyInfo in entity.GetType().GetProperties())
                {
                    command.Parameters.AddWithValue("@" + propertyInfo.Name, propertyInfo.GetValue(entity));
                }

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var typeName = typeof(T).Name;
                var storedProcedure = String.Format("usp_{0}_Delete", typeName);

                var command = new SqlCommand(storedProcedure, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", id);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public IEnumerable<T> ExecuteQueryEntity(IQuery query)
        {
            var items = new List<T>();

            using (var connection = new SqlConnection(ConnectionString))
            {
                var storedProcedure = query.Name;

                var command = new SqlCommand(storedProcedure, connection);
                command.CommandType = CommandType.StoredProcedure;

                foreach (var parameter in query.Parameters)
                {
                    command.Parameters.AddWithValue("@" + parameter.Key, parameter.Value);
                }

                connection.Open();
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var item = Activator.CreateInstance<T>();
                    Map(item, reader);

                    items.Add(item);
                }
            }

            return items;
        }

        public int ExecuteQueryScalar(IQuery query)
        {
            var id = -1;

            using (var connection = new SqlConnection(ConnectionString))
            {
                var storedProcedure = query.Name;

                var command = new SqlCommand(storedProcedure, connection);
                command.CommandType = CommandType.StoredProcedure;

                foreach (var parameter in query.Parameters)
                {
                    command.Parameters.AddWithValue("@" + parameter.Key, parameter.Value);
                }

                connection.Open();
                id = (int)command.ExecuteScalar();
            }

            return id;
        }

        public void ExecuteNonQuery(IQuery query)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var storedProcedure = query.Name;

                var command = new SqlCommand(storedProcedure, connection);
                command.CommandType = CommandType.StoredProcedure;

                foreach (var parameter in query.Parameters)
                {
                    command.Parameters.AddWithValue("@" + parameter.Key, parameter.Value);
                }

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        private void Map(T item, SqlDataReader reader)
        {
            foreach (PropertyInfo propertyInfo in item.GetType().GetProperties())
            {
                if (HasColumn(reader, propertyInfo.Name))
                {
                    var field = reader[propertyInfo.Name];

                    if (field != DBNull.Value)
                    {
                        propertyInfo.SetValue(item, field);
                    }
                }
            }
        }

        private bool HasColumn(SqlDataReader reader, string columnName)
        {
            var hasColumn = false;

            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (reader.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                {
                    hasColumn = true;
                }
            }

            return hasColumn;
        }
    }
}
