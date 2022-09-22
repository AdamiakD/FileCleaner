using System;
using System.Linq;
using System.IO;

namespace FileCleaner
{
    class Cleaner
    {
        string daysCntBack;
        static DateTime timeNow = DateTime.Now;
        string[] delExtension;
        string[] delException;

        private static string saveFile = $"{timeNow.Year.ToString()}{timeNow.Month.ToString().PadLeft(2, '0')}{timeNow.Day.ToString().PadLeft(2, '0')}_report.txt";

        public Cleaner(string daysCntBack, string[] delExtension, string[] delException)
        {
            this.daysCntBack = daysCntBack;
            this.delExtension = delExtension;
            this.delException = delException;
        }

        public void DoClean(string searchDir, bool testMode)
        {
            try
            {
                var filterdDirs = Directory.EnumerateDirectories(searchDir).Where(en => 
                    !delException.Any(en.ToUpper().Contains));

                foreach (string d in filterdDirs)
                {
                    var filtredFiles = Directory.EnumerateFiles(d).Where(en => 
                        !delException.Any(en.ToUpper().Contains) 
                        & delExtension.Any(System.IO.Path.GetExtension(en).ToUpper().StartsWith));
                    
                    foreach (string f in filtredFiles)
                    {
                        if (timeNow >= File.GetLastWriteTime(f).AddDays(Convert.ToInt32(this.daysCntBack)))
                        {
                            if(!testMode)
                            {
                                File.Delete(f);
                                this.WriteLog($"File: {f} - deleted");
                            }
                            else
                            {
                                this.WriteLog($"File: {f} - ready to delete");
                            }
                        }
                    }
            
                    this.DoClean(d, testMode);
                    if (Directory.EnumerateDirectories(d).Count() + Directory.EnumerateFiles(d).Count() == 0)
                    {
                        if (!testMode)
                        {
                            Directory.Delete(d, false);
                            this.WriteLog($"Dir: {d} - deleted");
                        }
                        else
                        {
                            this.WriteLog($"Dir: {d} - ready to delete");
                        }
                    }
                }
            }
            catch (System.Exception excpt)
            {
                this.WriteLog($"*** Exception:\n{excpt.ToString()}");
            }
        }

        public void WriteLog(string logMsg)
        {
            if(!Directory.Exists($"{Program.prgPath}/Reports/"))
            {
                Directory.CreateDirectory($"{Program.prgPath}/Reports/");
            }
            Console.WriteLine(logMsg);
            File.AppendAllText($"{Program.prgPath}/Reports/{saveFile}", $"{logMsg}\n");
        }
    }
}
