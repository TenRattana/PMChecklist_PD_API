using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using Dapper;
using System.Data;
using PMChecklist_PD_API.Models;

public class MachineService
{
    private readonly Connection _connection;
    private readonly LogService _logger;
    private readonly Common _common;

    public MachineService(Connection connection, LogService logger, Common common)
    {
        _common = common;
        _connection = connection;
        _logger = logger;
    }
    public List<Tuple<string, bool>> SaveMachine(Machines machines, StringBuilder logs = null!)
    {
        var resultList = new List<Tuple<string, bool>>();

        if (logs == null)
        {
            logs = new StringBuilder();
        }

        try
        {
            var before = _connection.QueryData<Machines>("EXEC GetMachinesInPage @ID = @MachineID", new { MachineID = machines.MachineID! }).FirstOrDefault();

            if (!String.IsNullOrEmpty(machines.MachineID))
            {
                logs.AppendLine("Change Data - Before");
                logs.AppendLine("----------------------------------------------------");

                _logger.AppendObjectPropertiesToLog(ref logs, before!, new string[] { "MachineID", "GMachineID", "MachineName", "Description", "IsActive", "Building", "Floor", "Area" });

                var strSQL = "UPDATE Machines SET GMachineID = @GMachineID ,MachineName = @MachineName ,Description = NULLIF(@Description, '') ,IsActive = @IsActive ,MachineCode = @MachineCode ,Building = NULLIF(@Building, '') ,Floor = NULLIF(@Floor, '') ,Area = NULLIF(@Area, '') WHERE MachineID = @MachineID";
                _connection.Execute(strSQL, new { GMachineID = machines.GMachineID!, MachineName = machines.MachineName!, Description = machines.Description!, IsActive = machines.IsActive!, MachineCode = machines.MachineCode!, Building = machines.Building!, Floor = machines.Floor!, Area = machines.Area!, MachineID = machines.MachineID! });
            }
            else
            {
                var prefix = _connection.QueryData<dynamic>("SELECT TOP(1) PF_Machine AS Prefix FROM AppConfig", new { }).FirstOrDefault();
                var max = _common.GetMaxID("Machines", "MachineID", $"{prefix!.Prefix}%") + 1;
                var formattedId = _common.FormattedId(prefix!.Prefix, max);

                var strSQL = "INSERT INTO Machines (MachineID , GMachineID , MachineName , Description , IsActive , MachineCode , Building , Floor , Area) VALUES (@MachineID ,@GMachineID ,@MachineName ,NULLIF(@Description,''), @IsActive ,@MachineCode ,NULLIF(@Building, '') ,NULLIF(@Floor, '') ,NULLIF(@Area, ''))";
                _connection.Execute(strSQL, new { GMachineID = machines.GMachineID!, MachineName = machines.MachineName!, Description = machines.Description!, IsActive = machines.IsActive!, MachineCode = machines.MachineCode!, Building = machines.Building!, Floor = machines.Floor!, Area = machines.Area!, MachineID = formattedId });
                machines.MachineID = formattedId;
            }

            logs.AppendLine("Update Data");
            logs.AppendLine("----------------------------------------------------");

            var data = _connection.QueryData<Machines>("EXEC GetMachinesInPage @ID = @MachineID", new { MachineID = machines.MachineID! }).FirstOrDefault();
            _logger.AppendObjectPropertiesToLog(ref logs, data!, new string[] { "MachineID", "GMachineID", "MachineName", "Description", "IsActive", "Building", "Floor", "Area" });
            _logger.LogInfo("Save Success : Save Data - Machine", logs);

            resultList.Add(Tuple.Create("Save data successful", true));
        }
        catch (Exception ex)
        {
            _logger.LogActionError(ex);
            resultList.Add(Tuple.Create("Save data not successful", false));
        }

        return resultList;
    }

    public List<Tuple<string, bool>> ChangeMachine(string MachineID, bool Status, StringBuilder logs = null!)
    {
        var resultList = new List<Tuple<string, bool>>();

        if (logs == null) logs = new StringBuilder();

        try
        {
            _connection.Execute("UPDATE Machines SET IsActive = @IsActive WHERE MachineID = @MachineID", new { IsActive = !Status, MachineID });
            logs.AppendLine($"NewIsActive : {!Status}");

            resultList.Add(Tuple.Create("Change field status successful", true));
            _logger.LogInfo("Change Success : Change Status Data - Machine", logs);
        }
        catch (Exception ex)
        {
            _logger.LogActionError(ex);
            resultList.Add(Tuple.Create("Change status not successful", false));
        }

        return resultList;
    }

    public List<Tuple<string, bool>> DeleteMachine(string MachineID, StringBuilder logs = null!)
    {
        var resultList = new List<Tuple<string, bool>>();

        if (logs == null) logs = new StringBuilder();

        try
        {
            _connection.Execute("DELETE FROM Machines WHERE MachineID = @MachineID", new { MachineID });

            resultList.Add(Tuple.Create("Delete success", true));
            _logger.LogInfo("Delete Success : Delete Data - Machine ", logs);
        }
        catch (Exception ex)
        {
            _logger.LogActionError(ex);
            resultList.Add(Tuple.Create("Delete not successful", false));
        }

        return resultList;
    }
}
