using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using Dapper;
using System.Data;

public class GroupMachineService
{
    private readonly Connection _connection;
    private readonly LogService _logger;

    public GroupMachineService(Connection connection, LogService logger)
    {
        _connection = connection;
        _logger = logger;
    }

    public List<Tuple<string, bool>> ChangeGroupMachine(string GMachineID, bool status, StringBuilder logs = null!)
    {
        var resultList = new List<Tuple<string, bool>>();

        if (logs == null)
        {
            logs = new StringBuilder();
        }

        try
        {
            var strSQL = "UPDATE GroupMachines SET IsActive = @IsActive WHERE GMachineID = @GMachineID";

            var parameters = new
            {
                IsActive = !status,
                GMachineID = GMachineID!
            };

            _connection.Execute(strSQL, parameters);
            logs.AppendLine($"NewIsActive : {!status}");

            resultList.Add(Tuple.Create("Change field status successful", true));
            _logger.LogInfo("Change Success : Change Status Data - Group Machine", logs);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            resultList.Add(Tuple.Create("Change status not successful", false));
        }

        return resultList;
    }

    public List<Tuple<string, bool>> DeleteGroupMachine(string GMachineID, StringBuilder logs = null!)
    {
        var resultList = new List<Tuple<string, bool>>();

        if (logs == null)
        {
            logs = new StringBuilder();
        }

        try
        {
            var strSQL = "DELETE FROM GroupMachines WHERE GMachineID = @GMachineID";

            var parameters = new
            {
                GMachineID = GMachineID!
            };

            _connection.Execute(strSQL, parameters);

            resultList.Add(Tuple.Create("Delete success", true));
            _logger.LogInfo("Delete Success : Delete Data - Group Machine ", logs);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            resultList.Add(Tuple.Create("Delete not successful", false));
        }

        return resultList;
    }
}
