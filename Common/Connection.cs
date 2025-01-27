using PMChecklist_PD_API.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

public class Connection
{
    private readonly PCMhecklistContext _context;

    public Connection(PCMhecklistContext context)
    {
        _context = context;
    }

    public async Task<int> ExecuteData(string sqlQuery, params SqlParameter[] parameters)
    {
        try
        {
            return await _context.Database.ExecuteSqlRawAsync(sqlQuery, parameters);
        }
        catch (Exception ex)
        {
            throw new Exception($"SQL execution failed: {ex.Message}", ex);
        }
    }
}
