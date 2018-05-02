﻿using System;
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
        
        public static Dictionary<string, string> get_dict_from_csv(string namespace_path)
        {
            var lines = new List<string>();
            var assembly = Assembly.GetExecutingAssembly();
            using (Stream stream = assembly.GetManifestResourceStream(namespace_path))
            using (StreamReader reader = new StreamReader(stream))
                while (!reader.EndOfStream) { lines.Add(reader.ReadLine()); }
            return lines.Select(line => line.Split(',')).ToDictionary(line => line[0], line => line[1]);
        }
    }
}
