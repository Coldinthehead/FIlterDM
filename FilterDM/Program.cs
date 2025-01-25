using Avalonia;
using FilterDM.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace FilterDM;

internal sealed class Program
{
    public static bool CrashReport = false;
    public static string? CrashExceptionString = null;
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        if (args.Length >= 2)
        {
            if (args[0].Equals("--show_crash"))
            {
                CrashReport = true;
                if (File.Exists(args[1]))
                {
                    string errorMessage = File.ReadAllText(args[1]);
                    CrashExceptionString = errorMessage;
                    
                }
                else
                {
                    CrashReport = false;
                }
            }
        }

        try
        {
        BuildAvaloniaApp()
        .StartWithClassicDesktopLifetime(args);
        }
        catch (Exception ex)
        {
            string crashName = $"{DateTime.Now:MM-dd_HH-mm}_crash.txt";
            using var fs = File.Create(crashName);
            using var sw = new StreamWriter(fs);
            sw.Write(ex);
            sw.Close();
            var executable = Environment.ProcessPath;
            if (File.Exists(executable))
            {
                try
                {
                    string[] errArgs = ["--show_crash", crashName];
                    using var process = Process.Start(new ProcessStartInfo(executable, errArgs));
                    Process.GetCurrentProcess().Kill();
                }
                catch(Exception runEx)
                {

                }
            }
        }
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}
