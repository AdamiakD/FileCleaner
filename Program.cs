using System;
using System.IO;

namespace FileCleaner
{
    class Program
    {
        public static string prgPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        public static DateTime timeNow = DateTime.Now;

        static void Main(string[] args)
        {
            File.AppendAllText($"{prgPath}/log.log", $"{timeNow.ToString()}  Start\n");
            Console.WriteLine("Start");

            string[] config = File.ReadAllLines($"{prgPath}\\config.ini");
            
            Cleaner cleanAgent = new Cleaner(config[0]);
            
            for (int i = 1; i < config.Length; i++)
            {
                cleanAgent.DoClean(config[i]);
            }
            File.AppendAllText($"{prgPath}/log.log", $"{timeNow.ToString()}  End\n");
            Console.WriteLine("Finish");
        }
    }
}
