using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using qk = QKeyCommon.Keyboard_items;


namespace StubRunner
{
    class DeserialzeJson
    {
        public qk.Keyboard getKeyboardDeserialization ()
        {

            qk.Keyboard keyboard = null;

            /* Cant use these below as this stub is not run in the same namespace as Qkeymapper, you however should use this to load the file
             * instead of hard coding the path like i am in this example. 
            var assembly = Assembly.GetExecutingAssembly();
            string rootDirectory = System.IO.Path.GetDirectoryName(assembly.Location);
            string jsonResourceURI = "Resources" + System.IO.Path.DirectorySeparatorChar + "JsonDefaultLayouts" + System.IO.Path.DirectorySeparatorChar;
            string jsonFilePath = System.IO.Path.Combine(rootDirectory, jsonResourceURI);

            */

            using (StreamReader jsonFile = File.OpenText(AppDomain.CurrentDomain.BaseDirectory + System.IO.Path.DirectorySeparatorChar + "6ball_no_macro.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                keyboard = (qk.Keyboard)serializer.Deserialize(jsonFile, typeof(qk.Keyboard));
            }

            return keyboard;

        }

    }
}
