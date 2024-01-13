using MineSweeperApp;
using Serilog;
using Serilog.Events;
using System.Configuration;


string logFileDirectory = ConfigurationManager.AppSettings["LogFileDirectory"];
string logFileName = ConfigurationManager.AppSettings["LogFileName"];
string logDateFormat = ConfigurationManager.AppSettings["LogDateFormat"];
string logFilePath = $"{logFileDirectory}/{logFileName}{DateTime.Now.ToString(logDateFormat)}.txt";
string outputTemplateFormat = ConfigurationManager.AppSettings["OutputTemplateFormat"];


Log.Logger = new LoggerConfiguration()
    .WriteTo.File(
        logFilePath,
        outputTemplate: outputTemplateFormat
    )
    .CreateLogger();
Log.Information("Mine Sweeper App Started!");


Console.WriteLine(MSPConstants.welcomeMsg);
Console.WriteLine(MSPConstants.gridSizePromptMsg);
int gridSize = GetValidInput(3, 10);

Console.WriteLine(MSPConstants.mineNumberToPlaceMsg);
int maxMines = (int)(gridSize * gridSize * 0.35);
int minesCount = GetValidInput(1, maxMines);
IMineSweeper mineSweeper = new MineSweeper(gridSize, minesCount, Log.Logger);
mineSweeper.PlayGame();

static int GetValidInput(int min, int max)
{
    while (true)
    {
        if (int.TryParse(Console.ReadLine(), out int result) && result >= min && result <= max)
        {
            return result;
        }
        else
        {
            Console.WriteLine("Invalid input. Enter a number between " + min + " and " + max + ": ");
        }
    }
}