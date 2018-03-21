using System;
using System.Collections.Generic;
using System.IO;
using QKeyCommon.Keyboard_items;
using System.Reflection;
using System.Web.Script.Serialization;
using System.Windows.Controls;
using HandlebarsDotNet;

namespace QKeyMapper
{
    public partial class CreateLayout : LayoutEditorPage
    {
        public void CreateNewLayout(string KeyboardLayoutName)
        {
           
            JavaScriptSerializer ser = new JavaScriptSerializer(); // Serializer 
            List<Keyboard> JsonInfo = new List<Keyboard>(1);  //List Contains Json Info
            string output = ser.Serialize(JsonInfo);                 //Serialize the List and add to output string
           
            var assembly = Assembly.GetExecutingAssembly();
            string rootDirectory = System.IO.Path.GetDirectoryName(assembly.Location);
            string hbsResourceURI = "Templates" + System.IO.Path.DirectorySeparatorChar;
            string hbsFilePath = System.IO.Path.Combine(rootDirectory, hbsResourceURI);
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
                files.Add(template_path, hbs_template(output));
            }
            foreach (var thing in files)
            {
                Console.WriteLine("==============================");
                Console.Write(thing.Value);
            }
            Console.ReadKey();
        }
    }



}





