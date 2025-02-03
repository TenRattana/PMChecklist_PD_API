using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using Dapper;
using System.Data;
using PMChecklist_PD_API.Models;

public class GroupMachineService
{
    private readonly Connection _connection;
    private readonly LogService _logger;
    private readonly Common _common;

    public GroupMachineService(Connection connection, LogService logger, Common common)
    {
        _common = common;
        _connection = connection;
        _logger = logger;
    }

    public List<Tuple<string, bool>> SaveGroupMachine(GroupMachines groupMachines, StringBuilder logs = null!)
    {
        var resultList = new List<Tuple<string, bool>>();

        if (logs == null)
        {
            logs = new StringBuilder();
        }

        try
        {
            var before = _connection.QueryData<GroupMachines>("EXEC GetGroupMachinesInPage @ID = @GMachineID", new { GMachineID = groupMachines.GMachineID! }).FirstOrDefault();

            if (!String.IsNullOrEmpty(groupMachines.GMachineID))
            {
                var strSQL = "UPDATE GroupMachines SET GMachineName = NULLIF(@GMachineName, '') ,Description = NULLIF(@Description, '') ,IsActive = NULLIF(@IsActive, '') WHERE GMachineID = @GMachineID";
                _connection.Execute(strSQL, new { GMachineName = groupMachines.GMachineName!, Description = groupMachines.Description!, IsActive = groupMachines.IsActive!, GMachineID = groupMachines.GMachineID! });

                logs.AppendLine("Change Data - Before");
                logs.AppendLine("----------------------------------------------------");

                _logger.AppendObjectPropertiesToLog(ref logs, before!, new string[] { "GMachineID", "GMachineName", "Description", "IsActive" });
            }
            else
            {
                var prefix = _connection.QueryData<dynamic>("SELECT TOP(1) PF_GroupMachine AS Prefix FROM AppConfig", new { }).FirstOrDefault();
                var max = _common.GetMaxID("GroupMachines", "GMachineID", $"{prefix!.Prefix}%") + 1;
                var formattedId = _common.FormattedId(prefix!.Prefix, max);

                var strSQL = "INSERT INTO GroupMachines (GMachineID , GMachineName , Description, IsActive) VALUES (NULLIF(@GMachineID, ''),NULLIF(@GMachineName, ''),NULLIF(@Description, ''), NULLIF(@IsActive, ''))";
                _connection.Execute(strSQL, new { GMachineName = groupMachines.GMachineName!, Description = groupMachines.Description!, IsActive = groupMachines.IsActive!, GMachineID = formattedId });
                groupMachines.GMachineID = formattedId;
            }

            logs.AppendLine("Update Data");
            logs.AppendLine("----------------------------------------------------");

            var data = _connection.QueryData<GroupMachines>("EXEC GetGroupMachinesInPage @ID = @GMachineID", new { GMachineID = groupMachines.GMachineID! }).FirstOrDefault();

            _logger.AppendObjectPropertiesToLog(ref logs, data!, new string[] { "GMachineID", "GMachineName", "Description", "IsActive" });
            _logger.LogInfo("Save Success : Save Data - Group Machine", logs);

            resultList.Add(Tuple.Create("Save data successful", true));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            resultList.Add(Tuple.Create("Save data not successful", false));
        }

        return resultList;
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
