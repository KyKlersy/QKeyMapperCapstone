using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMKCGen.helpers
{
    /*
     * A helper class that wraps the launching of an executable process 
     * Here is used to launch MSYS2's mingw64.exe for an isolated 
     * compiling and flashing environment
     */
    class Exec
    {
        public static void launch(string path, string args)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = false;
            startInfo.UseShellExecute = false;
            startInfo.FileName = path;
            startInfo.WindowStyle = ProcessWindowStyle.Normal;
            startInfo.Arguments = args;

            try
            {
                using (Process exeProcess = Process.Start(startInfo))
                {
                    exeProcess.WaitForExit();
                }
            }
            catch(Exception e)
            {
                throw;
            }
        }
    }
}
