using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ModdingToolkit.Commands;
using Webmilio.Commons.Extensions;

namespace ModdingToolkit
{
    public static class InputHelper
    {
        public static bool Choose(string question, bool def, bool otherIsFalse = false)
        {
            bool? answer = default;
            StringBuilder sb = new("(");

            void AddChar(char c, bool surround)
            {
                if (surround)
                    sb.Append('[');
                sb.Append(c);
                if (surround)
                    sb.Append(']');
            }

            AddChar('y', def);
            sb.Append('/');
            AddChar('n', !def);

            if (otherIsFalse)
                sb.Append('!');

            sb.Append(')');

            Console.Write("{0} {1}? ", question, sb);

            while (answer == default)
            {
                var response = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(response))
                {
                    return def;
                }

                var a = response[0];

                if (a == 'y' || a == 't')
                {
                    answer = true;
                }
                else if (otherIsFalse || a == 'n' || a == 'f')
                {
                    answer = false;
                }
                else
                {
                    Console.WriteLine("Value not accepted.");
                }
            }

            return answer.Value;
        }
        
        public static bool Choose<T>(IList<T> commands, out Command command) where T : Command
        {
            command = default;
            bool success = true;

            Console.WriteLine("Please choose one of the following commands:");

            int index;
            while (command == default && success)
            {
                index = 0;
                Console.WriteLine($"{index++}: Exit");

                commands.Do(c => Console.WriteLine("{0}: {1}", index++, c.Name));
                Console.Write("Choice: ");

                if (int.TryParse(Console.ReadLine(), out int iChoice))
                {
                    if (iChoice == 0)
                    {
                        success = false;
                    }
                    else
                    {
                        if (iChoice < 0 || iChoice > commands.Count)
                        {
                            Console.WriteLine($"Your choice must be an available choice (from 0 to {commands.Count}).");
                        }
                        else
                        {
                            command = commands[iChoice - 1];
                        }
                    }
                }
                else
                {
                    Console.WriteLine("You must enter a numerical (integral) value.");
                }
            }

            return success;
        }

        public static DirectoryInfo EnterDirectory(string question)
        {
            DirectoryInfo directory = default;

            while (directory == default)
            {
                Console.Write(question);

                string path = Console.ReadLine();

                if (Directory.Exists(path))
                    directory = new DirectoryInfo(path);
                else
                    Console.WriteLine("Specified path is invalid.");
            }

            return directory;
        }

        public static FileInfo EnterFile(string question)
        {
            FileInfo file = default;

            while (file == default)
            {
                Console.Write(question);

                string path = Console.ReadLine();

                if (File.Exists(path))
                    file = new FileInfo(path);
                else
                    Console.WriteLine("Specified path is invalid.");
            }

            return file;
        }
    }
}