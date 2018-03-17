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
using QMKCGen.Utils;
using System.Reflection;

namespace QMKCGen
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            { 
                Cout.printf("Usage: {0} file.json", AppDomain.CurrentDomain.FriendlyName, "hello");
                return;
            }
            string json_raw = File.ReadAllText(args[0]);
            Keyboard keyboard = JsonConvert.DeserializeObject<Keyboard>(json_raw);

            ////register hbs helpers
            //Handlebars.RegisterHelper("comma_list", (output, context, arguments) =>
            //{
            //    output.Write(String.Join(",", arguments));
            //});

            var assembly = Assembly.GetExecutingAssembly();
            string rootDirectory = System.IO.Path.GetDirectoryName(assembly.Location);
            string hbsResourceURI = "Templates" + System.IO.Path.DirectorySeparatorChar;
            string hbsFilePath = System.IO.Path.Combine(rootDirectory, hbsResourceURI);
            //roughly, what I want to do is
            var files = new Dictionary<string, string>();
            foreach (string template_path in Directory.GetFiles(hbsFilePath, "*.hbs", SearchOption.AllDirectories))
            {
                string hbs_raw = null;
                try
                { 
                    using (StreamReader reader = new StreamReader(template_path))
                    {
                        hbs_raw = reader.ReadToEnd();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("The file could not be read:");
                    Console.WriteLine(e.Message);
                }
                var hbs_template = Handlebars.Compile(hbs_raw);
                files.Add(template_path, hbs_template(keyboard));
            }
            foreach(var thing in files)
            {
                Console.WriteLine("==============================");
                Console.Write(thing.Value);
            }
            //then organize into directory
            Console.ReadKey();
        }
    }
}