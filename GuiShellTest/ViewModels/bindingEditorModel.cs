using GuiShellTest.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GuiShellTest.ViewModels
{
   public class bindingEditorModel
    {
        public List<KeyMacro> DefaultSingleKeyBinds { get; set; }
        private KeyMacro _selectedKeySingleMacro;
        private KeyMacro _selectedKeyMacroEditor;

        public bindingEditorModel()
        {
            DefaultSingleKeyBinds = new List<KeyMacro>();

            loadCollection();

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
            var assembly = Assembly.GetExecutingAssembly();
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

                    km = new KeyMacro { macroName = tokens[0], macroString = macroString.ToList() };
                    DefaultSingleKeyBinds.Add(km);
                }
            }
        }

        private void loadUserDefinedCollection()
        {


        }
    }
}
