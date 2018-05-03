using GuiShellTest.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;


namespace GuiShellTest.ViewModels
{
   public class bindingEditorModel
    {
        private Assembly assembly = Assembly.GetExecutingAssembly(); //Reflect and get the current executing assembly

        private string approot = AppDomain.CurrentDomain.BaseDirectory; //Get the current root directory of the application
        private string macroFolderPath; //Hold the folder path to user defined macros.

        public ObservableCollection<KeyMacro> DefaultSingleKeyBinds { get; set; } //List collection of single key binds.
        public ObservableCollection<KeyMacro> MacroKeyBinds { get; set; } //List collection of macro key binds.

        public List<KeyMacro> validHoldMacro { get; set; } //Valid macro list.

        public HashSet<string> keyNames; 
        public HashSet<string> customMacroNames; //check to make sure no duplicates exist

        //Private data variables for binding.
        private KeyMacro _selectedKeySingleMacro; 
        private KeyMacro _selectedKeyMacroEditor;

        public bindingEditorModel()
        {
            DefaultSingleKeyBinds = new ObservableCollection<KeyMacro>();
            MacroKeyBinds = new ObservableCollection<KeyMacro>();

            validHoldMacro = new List<KeyMacro>();
            keyNames = new HashSet<string>();
            customMacroNames = new HashSet<string>();

            macroFolderPath = approot + Path.DirectorySeparatorChar + "Macros" + Path.DirectorySeparatorChar + "Macros.csv";

            loadCollection();
            loadPredefinedMacroCollection();
            loadUserDefinedCollection();
            loadValidOnHoldMacros();

        }

        /* Public binding property for selected single key macro */
        public KeyMacro SelectedKeySingleMacro
        {
            get
            {
                return _selectedKeySingleMacro;
            }

            set
            {
                if (value != _selectedKeySingleMacro)
                {
                    _selectedKeySingleMacro = value;
                    onPropertyRaised(nameof(SelectedKeySingleMacro));

                    //if(_selectedKeySingleMacro != null)
                    //    _selectedKeySingleMacro.macroString.ForEach(elm => { Debug.Write(elm, " "); });
                }

            }
        }

        /* Public binding property for selected macro key */
        public KeyMacro SelectedKeyMacroEditor
        {
            get
            {
                return _selectedKeyMacroEditor;
            }

            set
            {
                if (value != _selectedKeyMacroEditor)
                {
                    _selectedKeyMacroEditor = value;
                    onPropertyRaised(nameof(SelectedKeyMacroEditor));
                    
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

        /* Loads this models data lists for combo box binding */
        private void loadCollection()
        {
            var resourceName = "GuiShellTest.Resources.singleKeyMacros.csv";
            KeyMacro km;
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                while (!reader.EndOfStream)
                {
                    var readLine = reader.ReadLine();
                    var tokens = readLine.Split(',');
                     
                    var macroString = tokens[1].Split('\\');

                    keyNames.Add(tokens[0]);

                    km = new KeyMacro { macroName = tokens[0], macroString = macroString.ToList() };
                    DefaultSingleKeyBinds.Add(km);
                }
            }
        }

        /* Loads predefined macro list collection */
        private void loadPredefinedMacroCollection()
        {

            var resourceName = "GuiShellTest.Resources.defaultKeyMacros.csv";
            KeyMacro km;
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                while (!reader.EndOfStream)
                {
                    var readLine = reader.ReadLine();
                    var tokens = readLine.Split(',');

                    var macroString = tokens[1].Split('\\');

                    keyNames.Add(tokens[0]);

                    km = new KeyMacro { macroName = tokens[0], macroString = macroString.ToList() };
                    MacroKeyBinds.Add(km);
                }
            }
        }

        /* Loads user defined macros to the collection */
        private void loadUserDefinedCollection()
        {
            KeyMacro km;
            if(File.Exists(macroFolderPath))
            {
                using (StreamReader reader = new StreamReader(macroFolderPath))
                {
                    while (!reader.EndOfStream)
                    {
                        var readLine = reader.ReadLine();
                        var tokens = readLine.Split(',');

                        if(tokens.Length > 1)
                        {
                            var macroString = tokens[1].Split('\\');

                            keyNames.Add(tokens[0]);

                            km = new KeyMacro { macroName = tokens[0], macroString = macroString.ToList() };
                            MacroKeyBinds.Add(km);
                        }
                    }
                }
            }

            return;
        }

        /* Loads the valid on hold macro list to be checked when a user tries to set an on hold macro. */
        private void loadValidOnHoldMacros()
        {
            var resourceName = "GuiShellTest.Resources.supportedOnHoldMacroModifiers.csv";
            KeyMacro km;
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                while (!reader.EndOfStream)
                {
                    var readLine = reader.ReadLine();
                    var tokens = readLine.Split(',');

                    var macroString = tokens[1].Split('\\');

                    keyNames.Add(tokens[0]);

                    km = new KeyMacro { macroName = tokens[0], macroString = macroString.ToList() };
                    validHoldMacro.Add( km );
                }
            }
        }
    }
}
