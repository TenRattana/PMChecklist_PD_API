using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;

public class Connection
{
    private readonly string _connectionString;

    public Connection(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") ?? 
                            throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    }

    private IDbConnection GetConnection()
    {
        return new SqlConnection(_connectionString);
    }

    public IEnumerable<T> QueryData<T>(string sql, object? parameters = null)
    {
        parameters = parameters ?? new { };

        using (var connection = GetConnection())
        {
            connection.Open();
            return connection.Query<T>(sql, parameters);
        }
    }

    public int Execute(string sql, object? parameters = null)
    {
        parameters = parameters ?? new { };

        using (var connection = GetConnection())
        {
            connection.Open();
            return connection.Execute(sql, parameters);
        }
    }

    public void ExecuteTransaction(Action<IDbConnection, IDbTransaction> action)
    {
        using (var connection = GetConnection())
        using (var transaction = connection.BeginTransaction())
        {
            connection.Open();
            try
            {
                action(connection, transaction);
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }

    public TResult ExecuteTransaction<TResult>(Func<IDbConnection, IDbTransaction, TResult> action)
    {
        using (var connection = GetConnection())
        using (var transaction = connection.BeginTransaction())
        {
            connection.Open();
            try
            {
                var result = action(connection, transaction);
                transaction.Commit();
                return result;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}
