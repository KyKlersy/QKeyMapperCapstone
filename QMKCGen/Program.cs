using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HandlebarsDotNet;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;
using QKeyCommon.Keyboard_items;
using QMKCGen.Utils;
using System.Reflection;
using QMKCGen.helpers;
using QMKCGen.directory_structures;

namespace QMKCGen
{
    class Program
    { 
        static void Main(string[] args)
        {
            if (args.Length != 1)
            { 
                Cout.printf("Usage: {0} file.json", AppDomain.CurrentDomain.FriendlyName);
                return;
            }
            string json_raw = File.ReadAllText(args[0]);
            Keyboard keyboard = JsonConvert.DeserializeObject<Keyboard>(json_raw);

            //register helpers
            Handlebars.RegisterHelper("keymap_user_friendly", (writer, context, parameters) => {
                writer.Write(matrix_helpers.user_friendly(keyboard));
            });
            Handlebars.RegisterHelper("keymap_with_kc_no", (writer, context, parameters) => {
                writer.Write(matrix_helpers.with_kc_no(keyboard));
            });
            Handlebars.RegisterHelper("keymap", (writer, context, parameters) => {
                writer.Write(matrix_helpers.keymap(keyboard));
            });

            var assembly = Assembly.GetExecutingAssembly();
            string rootDirectory = System.IO.Path.GetDirectoryName(assembly.Location);

            string directoryStructureJson = Self.getFileContents(assembly,
                "directory_structures" +
                System.IO.Path.DirectorySeparatorChar +
                "templates" +
                System.IO.Path.DirectorySeparatorChar +
                "default_project_structure.json");
            DirectoryStructure defaultStructure = JsonConvert.DeserializeObject<DirectoryStructure>(directoryStructureJson);
            //generate the folders needed
            foreach (var pathArray in defaultStructure.directories)
            {
                string newDirectory = "";
                foreach (var str in pathArray)
                {
                    newDirectory += str + System.IO.Path.DirectorySeparatorChar;
                }
                var f = new FileInfo(System.IO.Path.Combine(
                    rootDirectory, 
                    keyboard.desc.product_name +
                    System.IO.Path.DirectorySeparatorChar +
                    newDirectory
                ));
                if (!f.Directory.Exists)
                {
                    System.IO.Directory.CreateDirectory(f.DirectoryName);
                }
            }
            //fill them out
            foreach (var template_file in defaultStructure.files)
            {
                template_file.resolve_hbs(keyboard);
                using (System.IO.StreamWriter file =
                     new System.IO.StreamWriter(
                        keyboard.desc.product_name + 
                        System.IO.Path.DirectorySeparatorChar +
                        template_file.relative_path
                ))
                {
                    string hbs_raw = Self.getFileContents(assembly, template_file.template_path);
                    var hbs_template = Handlebars.Compile(hbs_raw);
                    file.Write(hbs_template(keyboard));
                }
            }

            //string rootDirectory = System.IO.Path.GetDirectoryName(assembly.Location);
            //string hbsResourceURI = "Templates";// + System.IO.Path.DirectorySeparatorChar;
            //string hbsFilePath = System.IO.Path.Combine(rootDirectory, hbsResourceURI);
            //var files = new Dictionary<string, string>();
            //foreach (string template_path in Directory.GetFiles(hbsFilePath, "*.hbs", SearchOption.AllDirectories))
            //{
            //    string hbs_raw = null;
            //    try
            //    { 
            //        using (StreamReader reader = new StreamReader(template_path))
            //        {
            //            hbs_raw = reader.ReadToEnd();
            //        }
            //    }
            //    catch (Exception e)
            //    {
            //        Console.WriteLine("The file could not be read:");
            //        Console.WriteLine(e.Message);
            //    }
            //    var hbs_template = Handlebars.Compile(hbs_raw);
            //    files[template_path] = hbs_template(keyboard);
            //}
            //foreach(var thing in files)
            //{
            //    Console.WriteLine("==============================");
            //    Console.Write(thing.Value);
            //}
        }
    }
}