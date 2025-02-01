using PMChecklist_PD_API.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;

public class LogService
{
    private readonly Connection _connection;
    private readonly string _IP;
    private readonly string _Host;
    private readonly string _User;
    private readonly ILogger<LogService> _logger;

    public LogService(Connection connection, ILogger<LogService> logger, IHttpContextAccessor httpContextAccessor)
    {
        _Host = httpContextAccessor.HttpContext?.Request.Headers["Host"].FirstOrDefault() ?? "Unknown Host";
        _IP = httpContextAccessor.HttpContext?.Request.Headers["X-Forwarded-For"].FirstOrDefault() ?? 
              httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? "Unknown IP";

        var authorizationHeader = httpContextAccessor.HttpContext?.Request.Headers["Authorization"].FirstOrDefault();
        if (authorizationHeader != null && authorizationHeader.StartsWith("Bearer "))
        {
            var userName = httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "UserName")?.Value;

            _User = userName ?? "Unknown User";  
        }
        else
        {
            _User = "Unknown User"; 
        }

        _connection = connection;
        _logger = logger;
    }

    public void LogInfo(Log value)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value), "Log value cannot be null.");
        }

        var formattedMessage = $"Title : {value.Title} \n " +
                               $"User : {_User} \n " +
                               $"IP Address : {_IP} \n " +
                               $"Date : {DateTime.Now:yyyy-MM-dd HH:mm:ss} \n " +
                               $"Host : {_Host} \n " +
                               $"Message : {value.Messages}";

        try
        {
            var strSQL = "INSERT INTO Logs (Title, Author, Messages, Type) VALUES (@Title, @Author, @Messages, @Type)";

            _connection.Execute(strSQL, new
            {
                Title = value.Title!,
                Author = _User,
                Messages = formattedMessage,
                Type = "Info"
            });

            _logger.LogInformation($"Info Log Inserted: {formattedMessage}");
        }
        catch (SqlException sqlEx)
        {
            throw new Exception("Database insert failed for LogInfo: " + sqlEx.Message);
        }
    }

    public void LogError(Log value)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value), "Log value cannot be null.");
        }

        var formattedMessage = $"Title: {value.Title} \n " +
                               $"User: {_User} \n " +
                               $"IP Address: {_IP} \n " +
                               $"Date: {DateTime.Now:yyyy-MM-dd HH:mm:ss} \n " +
                               $"Host: {_Host} \n " +
                               $"Error Message: {value.Messages}";

        try
        {
            var strSQL = "INSERT INTO Logs (Title, Author, Messages, Type) VALUES (@Title, @Author, @Messages, @Type)";

            _connection.Execute(strSQL, new
            {
                Title = value.Title!,
                Author = _User, 
                Messages = formattedMessage,
                Type = "Error"
            });

            _logger.LogError($"Error Log Inserted: {formattedMessage}");
        }
        catch (SqlException sqlEx)
        {
            throw new Exception("Database insert failed for LogError: " + sqlEx.Message);
        }
    }
}
