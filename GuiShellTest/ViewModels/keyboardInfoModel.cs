using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.IO;
using GuiShellTest.Models;
using System.Collections.ObjectModel;

namespace GuiShellTest.ViewModels
{
    public class keyboardInfoModel : INotifyPropertyChanged
    {
        public ObservableCollection<Microprocessor> SupportedProcs { get; set; }
        public ObservableCollection<JsonTemplateLayout> JsonLayouts { get; set; }
        private Microprocessor _selectedMicroProc;
        private JsonTemplateLayout _selectedJsonLayout;

        public keyboardInfoModel()
        {
            SupportedProcs = new ObservableCollection<Microprocessor>();
            JsonLayouts = new ObservableCollection<JsonTemplateLayout>();
            loadCollection();
        }

        /* Public binding property for Selected Microprocessor */
        public Microprocessor SelectedMicroProc
        {
            get
            {
                return _selectedMicroProc;
            }

            set
            {
                if(value != _selectedMicroProc)
                {
                    _selectedMicroProc = value;
                    
                    //Notify the property has changed by raising the event using the name of the property.
                    onPropertyRaised(nameof(SelectedMicroProc));
                }
            }
        }

        /* Public binding property for Selected Json Layout */
        public JsonTemplateLayout SelectedJsonLayout
        {
            get
            {
                return _selectedJsonLayout;
            }

            set
            {
                if (value != _selectedJsonLayout)
                {
                    _selectedJsonLayout = value;

                    //Notify the property has changed by raising the event using the name of the property.
                    onPropertyRaised(nameof(SelectedJsonLayout));
                }       

            }
        }

        //Event handler and function to raise event.
        public event PropertyChangedEventHandler PropertyChanged;
        private void onPropertyRaised(String propertyName)
        {
            //If the property on the view has changed, raise new event and construct new event args with the name of the property.
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
 
        /* This function is used in loading the obersable collections reading data from embedded resources and program data folder */

            /** ??? Todo: check for possible error case, user deletes the program data folder that it tries to read from see lines 98 - 113 **/
        private void loadCollection()
        {

            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "GuiShellTest.Resources.supportedProcs.csv";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                while (!reader.EndOfStream)
                {
                    var readLine = reader.ReadLine();
                    var tokens = readLine.Split(',');
                    SupportedProcs.Add(new Microprocessor(tokens[0], tokens[1]));
                }
            }

            string rootDirectory = System.IO.Path.GetDirectoryName(assembly.Location);
            string jsonResourceURI = "Resources" + System.IO.Path.DirectorySeparatorChar + "JsonDefaultLayouts" + System.IO.Path.DirectorySeparatorChar;
            string jsonFilePath = System.IO.Path.Combine(rootDirectory, jsonResourceURI);

            string[] supportedJsonLayouts = Directory.GetFiles(jsonFilePath, "*.json");


            foreach (string jsonConfigPath in supportedJsonLayouts)
            {
                string fileName = System.IO.Path.GetFileName(jsonConfigPath);
                fileName = fileName.Substring(0, (fileName.Length - 5));

                JsonLayouts.Add( new JsonTemplateLayout(fileName, jsonConfigPath));
            }

            JsonLayouts.Add(new JsonTemplateLayout("Custom", "Custom"));

        }

    }
}
