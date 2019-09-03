namespace RizumuBotApp
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using log4net;
    using log4net.Config;
    using Microsoft.Extensions.Configuration;
    using RizumuBot;

    internal class Program
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static async Task Main(string[] args)
        {
            var logRepository = LogManager.GetRepository(Assembly.GetExecutingAssembly());
            var processName = Assembly.GetEntryAssembly().GetName().Name;
            XmlConfigurator.Configure(logRepository, new FileInfo(@"log4net.config"));
            GlobalContext.Properties["pname"] = processName;
            GlobalContext.Properties["pid"] = Process.GetCurrentProcess().Id;


            var environmentName = Environment.GetEnvironmentVariable("RIZUMUBOT_ENVIRONMENT") ?? "local";

            var builder = new ConfigurationBuilder()
                //.AddJsonFile($"appsettings.json", true, true)
                .AddJsonFile($"{environmentName}.settings.json", true, true)
                .AddEnvironmentVariables();
            var configuration = builder.Build();

            builder.AddJsonFile(configuration["Values:TwitchBot.SecretsPath"], true, true);
            configuration = builder.Build();

            Config.Init(configuration);

            Console.OutputEncoding = Encoding.UTF8;
            var exitCode = 0;

            Logger.Info($"{processName} started!");
            var stopWatch = Stopwatch.StartNew();
            try
            {
                await RizumuBotApp.Main.Work(args);
            }
            catch (AggregateException ae)
            {
                exitCode = -1;
                Logger.Error("One or more exceptions occurred:");

                foreach (var exception in ae.Flatten().InnerExceptions)
                {
                    Logger.Error(exception.ToString());
                }
            }
            catch (Exception ex)
            {
                exitCode = -1;
                Logger.Error($"Exception occurred: {ex}");
            }
            finally
            {
                stopWatch.Stop();
            }

            Logger.Info($"{processName} finished! Time taken: {(double)stopWatch.ElapsedMilliseconds / 1000:0.000} secs");

            Console.WriteLine("Press enter to close.");
            Console.ReadLine();

            Environment.ExitCode = exitCode;
        }
    }
}
