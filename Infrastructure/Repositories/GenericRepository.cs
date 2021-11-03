using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Infrastructure.Factories;
using Infrastructure.Helpers;

namespace Infrastructure.Repositories
{
    public class GenericRepository : IGenericRepository
    {
        private readonly ISqlConnectionFactory _connectionFactory;
        private readonly string _tableName;

        public GenericRepository(ISqlConnectionFactory connectionFactory,
            string tableName)
        {
            _connectionFactory = connectionFactory;
            _tableName = tableName;
        }

        /// <summary>
        /// Open new connection and return it for use
        /// </summary>
        /// <returns></returns>
        private IDbConnection CreateConnection()
        {
            var conn = _connectionFactory.SqlConnection();
            conn.Open();
            return conn;
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>()
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<T>($"SELECT * FROM {_tableName}");
        }

        public async Task DeleteRowAsync<T>(T id)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync($"DELETE FROM {_tableName} WHERE Id=@Id", new { Id = id });
        }

        public async Task<T> GetAsync<T>(Guid id)
        {
            using var connection = CreateConnection();
            var result = await connection.QuerySingleOrDefaultAsync<T>($"SELECT * FROM {_tableName} WHERE Id=@Id", new { Id = id });
            if (result == null)
                throw new KeyNotFoundException($"{_tableName} with id [{id}] could not be found.");

            return result;
        }

        public async Task<int> SaveRangeAsync<T>(IEnumerable<T> list)
        {
            var inserted = 0;
            var query = GenerateInsertQuery<T>();
            using var connection = CreateConnection();
            inserted += await connection.ExecuteAsync(query, list);

            return inserted;
        }

        public async Task UpdateAsync<T>(T t)
        {
            var updateQuery = GenerateUpdateQuery<T>();

            using var connection = CreateConnection();
            await connection.ExecuteAsync(updateQuery, t);
        }

        public async Task<int> InsertAsync<T>(T t)
        {
            var insertQuery = GenerateInsertQuery<T>();

            using var connection = CreateConnection();
            return await connection.ExecuteAsync(insertQuery, t);
        }

        public async Task DeleteAsync()
        {
            await using var connection = _connectionFactory.SqlConnection();
            await connection.ExecuteAsync($"DELETE FROM {_tableName}");
        }

        #region PrivateMethods

        private string GenerateUpdateQuery<T>()
        {
            var updateQuery = new StringBuilder($"UPDATE {_tableName} SET ");
            var properties = GenerateListOfProperties(typeof(T).GetFilteredProperties());

            properties.ForEach(property =>
            {
                if (!property.Equals("Id"))
                {
                    updateQuery.Append($"{property}=@{property},");
                }
            });

            updateQuery.Remove(updateQuery.Length - 1, 1); //remove last comma
            updateQuery.Append(" WHERE Id=@Id");

            return updateQuery.ToString();
        }

        private static List<string> GenerateListOfProperties(IEnumerable<PropertyInfo> listOfProperties)
        {
            return (from prop in listOfProperties
                let attributes = prop.GetCustomAttributes(typeof(DescriptionAttribute), false)
                where attributes.Length <= 0 || (attributes[0] as DescriptionAttribute)?.Description != "ignore"
                select prop.Name).ToList();
        }

        private string GenerateInsertQuery<T>()
        {
            var insertQuery = new StringBuilder($"INSERT INTO {_tableName} ");

            insertQuery.Append("(");

            var properties = GenerateListOfProperties(typeof(T).GetFilteredProperties());
            properties.ForEach(prop => { insertQuery.Append($"[{prop}],"); });

            insertQuery
                .Remove(insertQuery.Length - 1, 1)
                .Append(") VALUES (");

            properties.ForEach(prop => { insertQuery.Append($"@{prop},"); });

            insertQuery
                .Remove(insertQuery.Length - 1, 1)
                .Append(")");

            return insertQuery.ToString();
        }

        #endregion


    }
}
