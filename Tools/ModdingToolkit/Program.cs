using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DiffPatch;
using ModdingToolkit.Magicka.Finding;
using ModdingToolkit.Texts;
using ModdingToolkit.Tools.Modding;
using ModdingToolkit.Tools.Modding.Modder.Commands;
using ModdingToolkit.Tools.User;
using Webmilio.Commons.Console;
using Webmilio.Commons.DependencyInjection;

namespace ModdingToolkit
{
    internal class Program
    {
        public const int MagickaAppId = 42910;
        private static Assembly _assembly = Assembly.GetExecutingAssembly();

        private static Task Main(string[] args)
        {
            var services = new ServiceProvider(true);
            services.AddSingleton<Program>();

            return services.GetService<Program>().MainAsync(args);
        }

        public Program(ServiceProvider services)
        {
            Services = services;
        }

        private async Task MainAsync(string[] args)
        {
            if (args.Length > 0)
            {
                System.Diagnostics.Debugger.Launch();

                var lArgs = args.ToList();

                if (lArgs.Contains("-m:diff"))
                {
                    var loader = Services.GetService<ModderCommandLoader>();
                    var command = loader.Generic<DiffCommand>();

                    await command.Execute();
                }

                return;
            }

            await TextsProvider.Load(_assembly);

            {
                var lines = TextsProvider.Wizard.Split('\n').Length;

                if (Console.WindowHeight < lines)
                {
                    try
                    {
                        Console.WindowHeight = lines + 6;
                    }
                    catch (PlatformNotSupportedException)
                    {
                        // Do nothing
                    }
                }
            }

            Console.WriteLine("MAGICKA MODDING TOOLKIT v{0}", _assembly.GetName().Version);
            Console.WriteLine(TextsProvider.Wizard);

            Console.WriteLine();
            Console.WriteLine(TextsProvider.Instructions);
            Console.WriteLine();

            var locationStore = Services.GetService<ILocationStore>();

            Console.Write("Initializing Location Store...");
            _ = Services.GetService<ILocationStore>();
            Console.WriteLine("Done. Found on {0}.", locationStore.MagickaExe);

            bool modder;

#if RELEASE_USER
            modder = false;
#else
            modder = InputHelper.Choose("First, are you a modder", false, true);
            Console.WriteLine();
#endif

            try
            {
                if (modder)
                {
                    await Services.GetService<IModdingTool>().Execute();
                }
                else
                {
                    await Services.GetService<IUserTool>().Execute();
                }
            }
            catch (ExecutionException e)
            {
                Console.WriteLine();
                ConsoleHelper.WriteLineError("Error while executing toolkit:{0}Code: {1} ({2}){0}Message: {3}", 
                    Environment.NewLine, (int) e.Code, e.Code.ToString(), e.Message);
            }

#if DEBUG
            Console.Write("Press any key to exit!");
            Console.ReadKey();
#endif
        }

        public ServiceProvider Services { get; }
        [Service] public TextsProvider TextsProvider { get; set; }
    }
}
