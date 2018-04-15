using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QMKCGen.helpers
{
    class Self
    {
        //may throw
        public static string getFileContents(Assembly asm, string path)
        {
            string result = "";
            string rootDirectory = System.IO.Path.GetDirectoryName(asm.Location);
            string fullPath = System.IO.Path.Combine(rootDirectory, path);
            using (StreamReader reader = new StreamReader(fullPath))
            {
                result = reader.ReadToEnd();
            }
            return result;
        }
    }
}
