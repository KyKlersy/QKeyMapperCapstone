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
        private Assembly assembly = Assembly.GetExecutingAssembly();
        private string approot = AppDomain.CurrentDomain.BaseDirectory;
        private string macroFolderPath;
        public ObservableCollection<KeyMacro> DefaultSingleKeyBinds { get; set; }
        public ObservableCollection<KeyMacro> MacroKeyBinds { get; set; }
        public List<KeyMacro> validHoldMacro { get; set; }
        public HashSet<string> keyNames;
        public HashSet<string> customMacroNames;

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
            //foreach(var item in DefaultSingleKeyBinds)
            //{
            //    Debug.WriteLine("String: " + item.macroString);
            //}
        }


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
                    Debug.WriteLine("Single key Macro: ");
                    if(_selectedKeySingleMacro != null)
                        _selectedKeySingleMacro.macroString.ForEach(elm => { Debug.Write(elm, " "); });
                }

            }
        }

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
