using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HandlebarsDotNet;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;
using QMKCGen.Keyboard_items;

namespace QMKCGen
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine(string.Format("Usage: {0} file.json", System.AppDomain.CurrentDomain.FriendlyName));
                return;
            }
            string json_raw = File.ReadAllText(args[0]);
            Keyboard keyboard = JsonConvert.DeserializeObject<Keyboard>(json_raw);
            Console.Write(JsonConvert.SerializeObject(keyboard, Formatting.Indented));
            Console.ReadKey();
        }
    }
}