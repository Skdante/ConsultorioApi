﻿using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ConsultorioApi.DataAccess
{
    public class BaseRepository : IBaseRepository
    {
        private readonly IConnectionFactory _connectionFactory;

        /// <summary>
        /// Constructor BaseRepository
        /// </summary>
        /// <param name="connectionFactory">IConnectionFactory <see cref="IConnectionFactory"/></param>
        public BaseRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        /// <summary>
        /// This method is responsible for ensuring that the connection is opened and closed safely and also ensures that we are always using an asynchronous connection.
        /// We open and close the connection with each method since SQL is going to manage our connection pooling and optimize this for us anyway
        /// We'll use a delegate here that matches a method that takes an argument of type IDbConnection and returns a Task of type T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="getData">Delegate that matches a method that takes an argument of type IDbConnection and returns a Task of type T</param>
        /// <returns>Task of type T - we'll be using this to build and execute our query</returns>
        public async Task<T> WithConnection<T>(Func<IDbConnection, Task<T>> getData)
        {
            try
            {
                using (var connection = _connectionFactory.CreateConnection())
                {
                    try
                    {
                        return await getData(connection);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            catch (TimeoutException ex)
            {
                throw new ArgumentNullException($"{GetType().FullName}.WithConnection() experienced a SQL timeout", ex);
            }
            catch (SqlException ex)
            {
                throw new ArgumentNullException($"{GetType().FullName}.WithConnection() experienced a SQL exception (not a timeout)", ex);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"{GetType().FullName}.WithConnection() experienced a exception", ex);
            }
        }
    }
}
