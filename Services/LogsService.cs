using PMChecklist_PD_API.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;
using System.Text;

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

    public void LogInfo(string Title, StringBuilder Message)
    {
        var formattedMessage = $"Title : {Title} \n " +
                               $"User : {_User} \n " +
                               $"IP Address : {_IP} \n " +
                               $"Date : {DateTime.Now:yyyy-MM-dd HH:mm:ss} \n " +
                               $"Host : {_Host} \n " +
                               $"{Message}";

        try
        {
            var strSQL = "INSERT INTO Logs (Title, Author, Messages, Type) VALUES (@Title, @Author, @Messages, @Type)";

            _connection.Execute(strSQL, new
            {
                Title = Title!,
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

    public void LogError(string Title, List<string> Messages, StringBuilder Message = null!)
    {
        var logMessage = Title;

        foreach (var kvp in Messages)
        {
            logMessage += $"{Environment.NewLine} {kvp}";
        }

        var formattedMessage = $"Title: {Title} \n " +
                               $"User: {_User} \n " +
                               $"IP Address: {_IP} \n " +
                               $"Date: {DateTime.Now:yyyy-MM-dd HH:mm:ss} \n " +
                               $"Host: {_Host} \n " +
                               $"{logMessage} \n {Message}";

        try
        {
            var strSQL = "INSERT INTO Logs (Title, Author, Messages, Type) VALUES (@Title, @Author, @Messages, @Type)";

            _connection.Execute(strSQL, new
            {
                Title = Title!,
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

    public void LogActionError(Exception exception)
    {
        var errorTitle = exception.Message.Replace("'", "''");
        var stackTrace = string.IsNullOrEmpty(exception.StackTrace) ? "" : exception.StackTrace.Replace("'", "''");

        var formattedMessage = $"Title: {errorTitle} \n " +
                               $"User: {_User} \n " +
                               $"IP Address: {_IP} \n " +
                               $"Date: {DateTime.Now:yyyy-MM-dd HH:mm:ss} \n " +
                               $"Host: {_Host} \n " +
                               $"{stackTrace}";

        try
        {
            var strSQL = "INSERT INTO Logs (Title, Author, Messages, Type) VALUES (@Title, @Author, @Messages, @Type)";

            _connection.Execute(strSQL, new
            {
                Title = errorTitle,
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

    public void AppendObjectPropertiesToLog(ref StringBuilder logs, object data, string[] propertiesToLog)
    {
        if (data == null)
        {
            logs.AppendLine("Data is null.");
            return;
        }

        var properties = data.GetType().GetProperties();

        foreach (var property in properties)
        {
            var propertyName = property.Name;

            if (propertiesToLog.Contains(propertyName))
            {
                var propertyValue = property.GetValue(data) ?? "null";
                logs.AppendLine($"{propertyName}: {propertyValue}");
            }
        }
        logs.AppendLine("----------------------------------------------------");
    }

}
