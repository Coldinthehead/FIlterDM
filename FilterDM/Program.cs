using Avalonia;
using FilterDM.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Threading.Tasks;

namespace FilterDM;

internal sealed class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        try
        {

        BuildAvaloniaApp()
        .StartWithClassicDesktopLifetime(args);
        }
        catch (Exception ex)
        {
            using var fs = File.Create($"{DateTime.Now:MM-dd_HH-mm}_crash.txt");
            using var sw = new StreamWriter(fs);
            sw.Write(ex);
            sw.Close();
        }
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}
