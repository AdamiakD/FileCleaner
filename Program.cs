using System.IO;

namespace FileCleaner
{
    class Program
    {
        public static string prgPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

        static void Main(string[] args)
        {
            /* Be careful! Your files and directories will be deleted to the Recycle Bin!
            Selecting TESTMODE = TRUE will make the program only display files without deleting them */ 
            
            bool testMode = true;

            
            /* delExtensions indicates the extensions of the files to be deleted */
           
            // string[] delExtensions = File.ReadAllLines($"{prgPath}/Config/Extensions.ini");
            string[] delExtensions = { ".XML", ".PGP", ".XLS", ".XLSX", ".CSV", ".DBF", ".TXT", ".ZIP", ".7Z", ".PDF", ".01", ".02" };
            
            
            /* delExceptions is responsible for deletion exceptions */
            
            // string[] delExceptions = File.ReadAllLines($"{prgPath}/Config/Exceptions.ini");
            string[] delExceptions = { "!", "IMPORTANT", "RAPORT", "RAPORT", "PROOF", "LAY" };

            
            /* The first parameter in string[] setup is the number of days back from today. 
            The other parameters are resources to search for files to delete. */
            
            // string[] setup = { "90", "d:\\work\\test" };
            string[] setup = File.ReadAllLines($"{prgPath}/Config/Setup.ini");


            Cleaner cleanAgent = new Cleaner(setup[0], delExtensions, delExceptions, testMode);
            cleanAgent.WriteLog($"Start");
            
            for (int i = 1; i < setup.Length; i++)
            {
                cleanAgent.DoClean(setup[i]);
            }

            cleanAgent.WriteLog($"End");
        }
    }
}
