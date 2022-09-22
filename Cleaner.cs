using System;
using System.Linq;
using System.IO;

namespace FileCleaner
{
    class Cleaner
    {
        string daysCnt;
        private static string[] deleteExt = { ".XML", ".PGP",".XLS", ".XLSX", ".CSV",".DBF",".TXT",".ZIP",".7Z",".PDF"};
        private static string saveName = $"Raporty/{Program.timeNow.Year.ToString()}{Program.timeNow.Month.ToString().PadLeft(2, '0')}{Program.timeNow.Day.ToString().PadLeft(2, '0')}_raport.txt";

        public Cleaner(string daysCnt)
        {
            this.daysCnt = daysCnt;
        }

        public void DoClean(string dir)
        {
            try
            {
                var filtredDirs = Directory.EnumerateDirectories(dir).Where(x => x.IndexOf("!") == -1 
                    & x.ToUpper().IndexOf("LAY") == -1 & x.ToUpper().IndexOf("PROOF") == -1);
                foreach (string d in filtredDirs)
                {
                    
                    var filtredFiles = Directory.EnumerateFiles(d).Where(x => x.IndexOf("!") == -1 
                        & x.ToUpper().IndexOf("RAPORT") == -1 
                        & x.ToUpper().IndexOf("PROOF") == -1 
                        & deleteExt.Any(x.ToUpper().EndsWith) 
                        | System.IO.Path.GetExtension(x).IndexOf(".0") > -1 
                        | System.IO.Path.GetExtension(x).IndexOf(".1") > -1
                        | System.IO.Path.GetExtension(x).IndexOf(".2") > -1);
                    
                    foreach (string f in filtredFiles)
                    {
                        // Console.WriteLine($"***{f}");
                        DateTime fileData = File.GetLastWriteTime(f);
                        if (Program.timeNow >= fileData.AddDays(Convert.ToInt32(this.daysCnt)))
                        {
                            string ext = System.IO.Path.GetExtension(f).ToUpper();
                            if (!(ext.ToUpper() == ".PDF" & d.ToUpper().IndexOf("PODAJNIK") > 0))
                            {
                                this.DeleteFile(f, d);
                            }
                        }
                    }
            
                    this.DoClean(d);
                    if (Directory.EnumerateDirectories(d).Count() + Directory.EnumerateFiles(d).Count() == 0)
                    {
                        Console.WriteLine($"Dir {d} - deleted");
                        Directory.Delete(d, false);
                        File.AppendAllText($"{Program.prgPath}/{saveName}", $"{d}\n");
                    }
                }
            }
            catch (System.Exception excpt)
            {
                Console.WriteLine($"***{excpt}");
                File.AppendAllText($"{Program.prgPath}/log.log", $"{Program.timeNow.ToString()}  Exception:\n{excpt.ToString()}\n");
            }
        }

        private void DeleteFile(string f, string d)
        {
            Console.WriteLine($"File {f} - deleted");
            File.Delete(f);
            File.AppendAllText($"{Program.prgPath}/{saveName}", $"{f}\n");
        }
    }
}
