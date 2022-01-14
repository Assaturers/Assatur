using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace ModdingToolkit.AssetsExtractor
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            string path = null;

            while (path == null)
            {
                Console.Write("Content Path: ");
                path = Console.ReadLine();
            }

            DirectoryInfo contentDir = new(path); 
            DirectoryInfo extractDir = new(Path.Combine(path, ".extracted"));

            Console.WriteLine("Extracting XNBs in folder {0} recursively into folder {1}.", path, extractDir.Name);

            var game = new MainThread();
            game.Start();

            while (!game.IsAlive)
                Thread.Sleep(500);

            game.Game.Content.RootDirectory = contentDir.FullName;
            int baseRoot = path.Length + 1;

            foreach (var file in contentDir.EnumerateFiles("*.xnb", SearchOption.AllDirectories))
            {
                var origin = file.FullName.Substring(baseRoot);
                var dir = Path.Combine(extractDir.FullName, Path.GetDirectoryName(origin));

                FileInfo target = new(Path.Combine(dir, Path.GetFileNameWithoutExtension(origin) + ".tga"));

                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                try
                {
                    game.Game.Extract(origin, target);
                    Console.WriteLine("Extracted {0}", origin);
                }
                catch
                {
                }
            }

            game.Stop();
            
            Console.Write("Press any key to exit...");
            Console.ReadKey();
        }
    }
}

