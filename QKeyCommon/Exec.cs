using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QKeyCommon
{
    //helper class to facilitate launching executables
    public class Exec
    {
        public static string launch(string path, string args)
        {
            // Use ProcessStartInfo class
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = false;
            startInfo.UseShellExecute = false;
            startInfo.FileName = path;
            startInfo.WindowStyle = ProcessWindowStyle.Normal;
            startInfo.Arguments = args;
            startInfo.RedirectStandardOutput = true;
            //startInfo.UseShellExecute = true;
            //startInfo.Verb = "runas";
            string result = "";

            try
            {
                // Start the process with the info we specified.
                // Call WaitForExit and then the using statement will close.
                using (Process exeProcess = Process.Start(startInfo))
                {
                    exeProcess.WaitForExit();
                    result = exeProcess.StandardOutput.ReadToEnd();
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return result;
        }
    }
}
