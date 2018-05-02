using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HandlebarsDotNet;
using Newtonsoft.Json;
using System.IO;
using QKeyCommon.Keyboard_items;
using QMKCGen.Utils;
using System.Reflection;
using QMKCGen.helpers;
using QMKCGen.QMKStructures;

namespace QMKCGen
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length != 1)
            {
                Cout.printf("Usage: {0} file.json", AppDomain.CurrentDomain.FriendlyName);
                return 1;
            }
            string json_raw = File.ReadAllText(args[0]);
            Keyboard keyboard = new Keyboard();
            var assembly = Assembly.GetExecutingAssembly();

            try
            {
                keyboard = JsonConvert.DeserializeObject<Keyboard>(json_raw);
            }
            catch
            {
                Console.WriteLine("The format of the provided file is invalid");
                return 2;
            }

            if (!Validations.has_valid_key_codes(keyboard))
            {
                Console.WriteLine("Configuration File contains invalid keycodes");
                return 3;
            }

            //register helpers
            Handlebars.RegisterHelper("keymap_user_friendly", (writer, context, parameters) =>
            {
                writer.Write(matrix_helpers.user_friendly(keyboard));
            });
            Handlebars.RegisterHelper("keymap_with_kc_no", (writer, context, parameters) =>
            {
                writer.Write(matrix_helpers.with_kc_no(keyboard));
            });
            Handlebars.RegisterHelper("keymap", (writer, context, parameters) =>
            {
                writer.Write(matrix_helpers.keymap(keyboard));
            });
            Handlebars.RegisterHelper("macros", (writer, context, parameters) =>
            {
                writer.Write(macro_helpers.macros(keyboard));
            });

            try
            {
                string rootDirectory = System.IO.Path.GetDirectoryName(assembly.Location);
                string directoryStructureJson = Self.getFileContents(assembly, System.IO.Path.Combine(
                    "QMKStructures", "templates", "default_qmk_structure.json"));
                QMKStructure qs = JsonConvert.DeserializeObject<QMKStructure>(directoryStructureJson);
                qs.testTemplates(assembly, keyboard);
                qs.generateDirectories(assembly, keyboard);
                qs.generateFiles(assembly, keyboard);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("The format of the provided file is invalid");
                return 4;
            }

            if(!Validations.is_valid_filename(keyboard.desc.product_name))
            {
                Console.WriteLine("The product name of your keyboard is not a valid filename");
                return 5;
            }

            string qmk_firmware_path = System.IO.Path.Combine(
                new DirectoryInfo(System.IO.Path.GetDirectoryName(assembly.Location)).Parent.Parent.FullName, 
                "qmk_firmware"
            );

            string exec_args = "/bin/bash -lic 'cd $(cygpath \"" + qmk_firmware_path + "\"); make " + keyboard.desc.product_name + ":default:avrdude'";
            string mintty_path = @"C:/msys64/usr/bin/mintty.exe";
            string mingw64_path = @"C:\msys64\mingw64.exe";
            //open up the flashing utility
            Exec.launch(mingw64_path, exec_args);
            return 0;
        }
    }
}