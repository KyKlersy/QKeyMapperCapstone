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
                "QMKStructures" +
                System.IO.Path.DirectorySeparatorChar +
                "templates" +
                System.IO.Path.DirectorySeparatorChar +
                "default_qmk_structure.json");
            QMKStructure qs = JsonConvert.DeserializeObject<QMKStructure>(directoryStructureJson);
            qs.generateDirectories(assembly, keyboard);
            qs.generateFiles(assembly, keyboard);
            return 0;
        }
    }
}