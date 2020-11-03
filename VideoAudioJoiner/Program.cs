using System;

namespace VideoAudioJoiner
{
    class Program
    {
        static void Main(string[] args)
        {
            var mainFolder = GetMainFolderPath(args);

            ValidatePath(mainFolder);

            var joiner = new Joiner(mainFolder);
            joiner.Join();
        }

        private static string GetMainFolderPath(string[] args)
        {
            if (args.Length > 0)
                return args[0];

            return AppDomain.CurrentDomain.BaseDirectory;
        }

        private static void ValidatePath(string path)
        {
            if (!System.IO.Directory.Exists(path))
            {
                Console.WriteLine($"The path \"{path}\" is not valid!");
                Environment.Exit(-1);
            }
        }
    }
}
