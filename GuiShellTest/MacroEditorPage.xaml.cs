using GuiShellTest.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.IO;
using System.Text.RegularExpressions;
using GuiShellTest.Models;
using System.Diagnostics;

namespace QKeyMapper
{
    /// <summary>
    /// Interaction logic for MacroEditorPage.xaml
    /// Macro editor page,
    /// 
    /// </summary>
    public partial class MacroEditorPage : Page
    {
        
        private string approot = AppDomain.CurrentDomain.BaseDirectory;
        private string macroFolderPath;
        private static string MacrosTextFile = "Macros.csv";
        private MainWindow mainwindow;
        private bindingEditorModel model;
        private List<string> macro = new List<string>();

        public MacroEditorPage()
        {
            InitializeComponent();
        }

        public MacroEditorPage(MainWindow cmainwindow)
        {
            mainwindow = cmainwindow;
            model = mainwindow.bindingeditormodel;
            DataContext = model;
            InitializeComponent();

            macroFolderPath = approot + Path.DirectorySeparatorChar + "Macros";
            Directory.CreateDirectory(macroFolderPath);

        }

        /* Navigation event on click of back button */
        private void goBackToBinding(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(mainwindow.bindingEditorPage);
        }

        /* Adds a key down selection to the macro page */
        private void addKeyDown(object sender, RoutedEventArgs e)
        {
            if(model.SelectedKeyMacroEditor != null)
                MacroStringTxt.Text += "+,"+ model.SelectedKeyMacroEditor.macroName + ",10ms,";
        }

        /* Adds a key up selection to the macro page */
        private void addKeyUp(object sender, RoutedEventArgs e)
        {
            if (model.SelectedKeyMacroEditor != null)
                MacroStringTxt.Text += "-," + model.SelectedKeyMacroEditor.macroName + ",10ms,";
        }

        /* Clears the macro string currently constructed */
        private void clearMacroString(object sender, RoutedEventArgs e)
        {
            MacroStringTxt.Text = "";
        }

        /* Creates the custom macro saving it out to the custom macro file list and updating the custom macro loaded data list */
        private void createAndSaveMacro(object sender, RoutedEventArgs e)
        {
            string macrostring = "";
            string customName = "";

            try
            {
                if (customMacroName.Text != null && customMacroName.Text != "")
                {
                    
                    customName = customMacroName.Text;

                    if(customName.Length <= 0 || customName.Length > 100)
                    {
                        throw new Exception("Invalid Name length must be between 1 and 100");
                    }
                }
                else
                {
                    throw new Exception("Macro name must not be empty.");
                }

                macrostring = MacroStringTxt.Text;

                tryLexMacroString(macrostring);

                if (!model.keyNames.Contains(customName))
                {
                    KeyMacro km = new KeyMacro { macroName = customName, macroString = macro };
                    model.MacroKeyBinds.Add(km);

                    using (StreamWriter writer = File.AppendText(macroFolderPath + Path.DirectorySeparatorChar + MacrosTextFile))
                    {
                        string ms ="";

                        
                        for(int t = 0; t < km.macroString.Count; t++)
                        {


                            if (t == km.macroString.Count - 1)
                            {
                                ms += km.macroString[t].ToString();
                            }
                            else
                            {
                                ms += km.macroString[t].ToString() + "\\";
                            }
                        }

                        writer.WriteLine(km.macroName + "," + ms);
                    }
                    
                    NavigationService.Navigate(mainwindow.bindingEditorPage);
                }
                else
                {
                    throw new Exception("Macro name must be unique.");
                }

            }
            catch(Exception err)
            {
                MessageBox.Show("Error: " + err.Message);
                return;
            }
        }

        /* Tries to reparse a string that could be modified by the user back into a valid macro string */
        private void tryLexMacroString(String ms)
        {
            string pattern = @"[1-9][0-9]*ms";
            var tokens = ms.Split(',');
            tokens = tokens.Take(tokens.Count() - 1).ToArray();

            for (int count = 0; count< tokens.Count(); count+=3)
            {
                if (tokens[count] == "+" || tokens[count] == "-")
                {
                    macro.Add(tokens[count]);
                }
                else
                {
                    throw new Exception("Bad Key press token.");
                }

                if (model.keyNames.Contains(tokens[count + 1]))
                {
                    macro.Add(tokens[count + 1]);
                }
                else
                {
                    throw new Exception("Invalid key name.");
                }

                if (tokens[count+2].Contains("ms"))
                {

                    var timedelay = Regex.Match(tokens[count + 2], pattern);

                    if(timedelay.Value == null || timedelay.Value == "")
                    {
                        throw new Exception("Invalid Time.");
                    }
                    
                    macro.Add(timedelay.Value);
                }
            }
        }
    }
}
