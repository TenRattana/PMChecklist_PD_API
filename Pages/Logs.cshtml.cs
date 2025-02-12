using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;
using Serilog;
using Microsoft.AspNetCore.Mvc;

public class LogsModel : PageModel
{
    public string? LogContent { get; set; }

    public IActionResult OnGet()
    {
        var logDirectory = Path.Combine(Directory.GetCurrentDirectory(), "logs");
        var logFileName = "myapp20250212.log";
        var logFilePath = Path.Combine(logDirectory, logFileName);

        if (System.IO.File.Exists(logFilePath))
        {
            try
            {
                using (var fileStream = new FileStream(logFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    var fileContents = new StreamReader(fileStream).ReadToEnd();
                    LogContent = fileContents; 
                    return Page(); 
                }
            }
            catch (IOException ex)
            {
                Log.Error(ex, "Error reading the log file.");
                LogContent = "Error reading the log file: " + ex.Message; 
                return Page();
            }
        }
        else
        {
            LogContent = "Log file not found.";
            return Page(); 
        }
    }
}
