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
            if(args.Length != 1)
            {
                Console.WriteLine("Usage: {} file.json", System.AppDomain.CurrentDomain.FriendlyName);
                return;
            }
            string json_raw = File.ReadAllText(args[0]);
            Keyboard keyboard = JsonConvert.DeserializeObject<Keyboard>(json_raw);

            Console.ReadKey();
        }
    }
}
