using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;
using Serilog;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

public class LogsModel : PageModel
{
    public string? LogContent { get; set; }

    public IActionResult OnGet()
    {
        var logDirectory = Path.Combine(Directory.GetCurrentDirectory(), "logs");
        var logFileName = "myapp20250213.log";
        var logFilePath = Path.Combine(logDirectory, logFileName);

        if (System.IO.File.Exists(logFilePath))
        {
            try
            {
                var logLines = new List<string>();

                using (var fileStream = new FileStream(logFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (var reader = new StreamReader(fileStream))
                    {
                        string? line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            string coloredLine = GetColoredLog(line);
                            logLines.Add(coloredLine);
                        }
                    }
                }

                LogContent = string.Join("<br/>", logLines);
                return Page();
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

    private string GetColoredLog(string logLine)
    {
        var regex = new Regex(@"^(\d{4}-\d{2}-\d{2}\s\d{2}:\d{2}:\d{2}.\d{3}\s[+-]\d{2}:\d{2})\s\[(\w+)\](.*)");

        var match = regex.Match(logLine);

        if (match.Success)
        {
            string timestamp = match.Groups[1].Value;
            string logLevel = match.Groups[2].Value;
            string details = match.Groups[3].Value;

            string coloredTimestamp = $"<span>{timestamp}</span>";
            string coloredDetails = details;

            if (logLevel == "INF")
            {
                coloredDetails = $"<span class='info'>{coloredDetails}</span>";
            }
            else if (logLevel == "WRN")
            {
                coloredDetails = $"<span class='wrn'>{coloredDetails}</span>";
            }
            else if (logLevel == "ERR")
            {
                coloredDetails = $"<span class='err'>{coloredDetails}</span>";
            }
            else if (logLevel == "FTL")
            {
                coloredDetails = $"<span class='ftl'>{coloredDetails}</span>";
            }

            return $"{coloredTimestamp} {coloredDetails}";
        }

        return logLine;
    }
}
