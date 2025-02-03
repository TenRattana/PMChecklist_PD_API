using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using Dapper;
using System.Data;
using PMChecklist_PD_API.Models;

public class CheckListOptionService
{
    private readonly Connection _connection;
    private readonly LogService _logger;
    private readonly Common _common;

    public CheckListOptionService(Connection connection, LogService logger, Common common)
    {
        _common = common;
        _connection = connection;
        _logger = logger;
    }

    public List<Tuple<string, bool>> SaveCheckList(CheckLists checkList, StringBuilder logs = null!)
    {
        var resultList = new List<Tuple<string, bool>>();

        if (logs == null) logs = new StringBuilder();

        try
        {
            var before = _connection.QueryData<CheckLists>("EXEC GetCheckListInPage @ID = @CListID", new { CListID = checkList.CListID! }).FirstOrDefault();

            if (!String.IsNullOrEmpty(checkList.CListID))
            {
                var strSQL = "UPDATE CheckLists SET CListName = @CListName, IsActive = @IsActive WHERE CListID = @CListID";
                _connection.Execute(strSQL, new { CListName = checkList.CListName!, IsActive = checkList.IsActive!, CListID = checkList.CListID! });

                logs.AppendLine("Change Data - Before");
                logs.AppendLine("----------------------------------------------------");

                _logger.AppendObjectPropertiesToLog(ref logs, before!, new string[] { "CListID", "CListName", "IsActive" });
            }
            else
            {
                var prefix = _connection.QueryData<dynamic>("SELECT TOP(1) PF_CheckList AS Prefix FROM AppConfig", new { }).FirstOrDefault();
                var max = _common.GetMaxID("CheckLists", "CListID", $"{prefix!.Prefix}%") + 1;
                var formattedId = _common.FormattedId(prefix!.Prefix, max);

                var strSQL = "INSERT INTO CheckLists (CListID , CListName, IsActive) VALUES ( @CListID ,@CListName ,@IsActive)";
                _connection.Execute(strSQL, new { CListName = checkList.CListName!, IsActive = checkList.IsActive!, CListID = formattedId });
                checkList.CListID = formattedId;
            }

            logs.AppendLine("Update Data");
            logs.AppendLine("----------------------------------------------------");

            var data = _connection.QueryData<CheckLists>("EXEC GetCheckListInPage @ID = @CListID", new { CListID = checkList.CListID! }).FirstOrDefault();

            _logger.AppendObjectPropertiesToLog(ref logs, data!, new string[] { "CListID", "CListName", "IsActive" });
            _logger.LogInfo("Save Success : Save Data - CheckList ", logs);

            resultList.Add(Tuple.Create("Save data successful", true));
        }
        catch (Exception ex)
        {
            _logger.LogActionError(ex);
            resultList.Add(Tuple.Create("Save data not successful", false));
        }

        return resultList;
    }


    public List<Tuple<string, bool>> ChangeCheckList(string CListID, bool Status, StringBuilder logs = null!)
    {
        var resultList = new List<Tuple<string, bool>>();

        if (logs == null) logs = new StringBuilder();

        try
        {
            _connection.Execute("UPDATE CheckLists SET IsActive = @IsActive WHERE CListID = @CListID", new { IsActive = !Status, CListID });
            logs.AppendLine($"NewIsActive : {!Status}");

            _logger.LogInfo("Change Success : Change Status Data - CheckList ", logs);

            resultList.Add(Tuple.Create("Change field status successful", true));
        }
        catch (Exception ex)
        {
            _logger.LogActionError(ex);
            resultList.Add(Tuple.Create("Change status not successful", false));
        }

        return resultList;
    }

    public List<Tuple<string, bool>> DeleteCheckList(string CListID, StringBuilder logs = null!)
    {
        var resultList = new List<Tuple<string, bool>>();

        if (logs == null) logs = new StringBuilder();

        try
        {
            _connection.Execute("DELETE FROM CheckLists WHERE CListID = @CListID", new { CListID });
            _logger.LogInfo("Delete Success : Delete Data - CheckList ", logs);

            resultList.Add(Tuple.Create("Delete success", true));
        }
        catch (Exception ex)
        {
            _logger.LogActionError(ex);
            resultList.Add(Tuple.Create("Delete not successful", false));
        }

        return resultList;
    }
}
