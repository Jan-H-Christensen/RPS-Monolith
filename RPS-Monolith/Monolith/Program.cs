// See https://aka.ms/new-console-template for more information

using Monolith;
using Serilog;
using Monitoring;
using OpenTelemetry.Trace;

public class Program
{
    public static void Main()
    {
        using var activity = MonitoringService.ActivitySource.StartActivity("Start Game");
        var game = new Game();
        for (int i = 1; i <= 1000; i++)
        {
            using var games = MonitoringService.ActivitySource.StartActivity();
            MonitoringService.Log.Information("Game number: {i}", i);
            game.Start();
        }
        Console.WriteLine("Finished");

        MonitoringService.Log.Information("Shutting down...");

        Log.CloseAndFlush();
        MonitoringService.TracerProvider.ForceFlush();
    }
}