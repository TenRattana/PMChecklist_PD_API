using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using Dapper;
using System.Data;

public class MachineService
{
    private readonly Connection _connection;
    private readonly LogService _logger;

    public MachineService(Connection connection, LogService logger)
    {
        _connection = connection;
        _logger = logger;
    }
    public List<Tuple<string, bool>> SaveMachine(string MachineID, bool status, StringBuilder logs = null!)
    {
        var resultList = new List<Tuple<string, bool>>();

        if (logs == null)
        {
            logs = new StringBuilder();
        }

        try
        {
            var strSQL = "UPDATE Machines SET IsActive = @IsActive WHERE MachineID = @MachineID";

            var parameters = new
            {
                IsActive = !status,
                MachineID = MachineID!
            };

            _connection.Execute(strSQL, parameters);
            logs.AppendLine($"NewIsActive : {!status}");

            resultList.Add(Tuple.Create("Change field status successful", true));
            _logger.LogInfo("Change Success : Change Status Data - Machine", logs);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            resultList.Add(Tuple.Create("Change status not successful", false));
        }

        return resultList;
    }

    public List<Tuple<string, bool>> ChangeMachine(string MachineID, bool status, StringBuilder logs = null!)
    {
        var resultList = new List<Tuple<string, bool>>();

        if (logs == null)
        {
            logs = new StringBuilder();
        }

        try
        {
            var strSQL = "UPDATE Machines SET IsActive = @IsActive WHERE MachineID = @MachineID";

            var parameters = new
            {
                IsActive = !status,
                MachineID = MachineID!
            };

            _connection.Execute(strSQL, parameters);
            logs.AppendLine($"NewIsActive : {!status}");

            resultList.Add(Tuple.Create("Change field status successful", true));
            _logger.LogInfo("Change Success : Change Status Data - Machine", logs);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            resultList.Add(Tuple.Create("Change status not successful", false));
        }

        return resultList;
    }

    public List<Tuple<string, bool>> DeleteMachine(string MachineID, StringBuilder logs = null!)
    {
        var resultList = new List<Tuple<string, bool>>();

        if (logs == null)
        {
            logs = new StringBuilder();
        }

        try
        {
            var strSQL = "DELETE FROM Machines WHERE MachineID = @MachineID";

            var parameters = new
            {
                MachineID = MachineID!
            };

            _connection.Execute(strSQL, parameters);

            resultList.Add(Tuple.Create("Delete success", true));
            _logger.LogInfo("Delete Success : Delete Data - Machine ", logs);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            resultList.Add(Tuple.Create("Delete not successful", false));
        }

        return resultList;
    }
}
