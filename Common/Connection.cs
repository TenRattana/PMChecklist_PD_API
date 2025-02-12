using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using Serilog;
using System.Diagnostics;

public class Connection
{
    private readonly string _connectionString;
    private readonly ILogger<dynamic> _logger;

    public Connection(IConfiguration configuration, ILogger<dynamic> logger)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") ??
                            throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    private IDbConnection GetConnection()
    {
        return new SqlConnection(_connectionString);
    }

    public IEnumerable<T> QueryData<T>(string sql, object? parameters = null)
    {
        parameters = parameters ?? new { };

        var stopwatch = Stopwatch.StartNew();

        using (var connection = GetConnection())
        {
            connection.Open();
            var result = connection.Query<T>(sql, parameters);

            stopwatch.Stop();
            _logger.LogInformation("Command Executed Successfully: {SQL} with Parameters: {@Parameters} in {ElapsedMilliseconds} ms", sql, parameters, stopwatch.ElapsedMilliseconds);

            return result;
        }
    }

    public int Execute(string sql, object? parameters = null)
    {
        parameters = parameters ?? new { };

        var stopwatch = Stopwatch.StartNew();

        using (var connection = GetConnection())
        {
            connection.Open();
            var result = connection.Execute(sql, parameters);

            stopwatch.Stop();
            _logger.LogInformation("Command Executed Successfully: {SQL} with Parameters: {@Parameters} in {ElapsedMilliseconds} ms", sql, parameters, stopwatch.ElapsedMilliseconds);

            return result;
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
                _logger.LogInformation("Starting Transaction");

                var stopwatch = Stopwatch.StartNew();
                action(connection, transaction);

                transaction.Commit();

                stopwatch.Stop();
                _logger.LogInformation("Transaction Committed Successfully in {ElapsedMilliseconds} ms", stopwatch.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "Transaction Failed and Rolled Back");
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
                _logger.LogInformation("Starting Transaction");

                var stopwatch = Stopwatch.StartNew();
                var result = action(connection, transaction);

                transaction.Commit();

                stopwatch.Stop();
                _logger.LogInformation("Transaction Committed Successfully in {ElapsedMilliseconds} ms", stopwatch.ElapsedMilliseconds);

                return result;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "Transaction Failed and Rolled Back");
                throw;
            }
        }
    }
}
