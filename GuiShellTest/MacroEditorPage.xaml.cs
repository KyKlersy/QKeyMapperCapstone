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

        private void goBackToBinding(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(mainwindow.bindingEditorPage);
        }

        private void addKeyDown(object sender, RoutedEventArgs e)
        {
            if(model.SelectedKeyMacroEditor != null)
                MacroStringTxt.Text += "+,"+ model.SelectedKeyMacroEditor.macroName + ",10ms,";
        }

        private void addKeyUp(object sender, RoutedEventArgs e)
        {
            if (model.SelectedKeyMacroEditor != null)
                MacroStringTxt.Text += "-," + model.SelectedKeyMacroEditor.macroName + ",10ms,";
        }

        private void clearMacroString(object sender, RoutedEventArgs e)
        {
            MacroStringTxt.Text = "";
        }

        private void createAndSaveMacro(object sender, RoutedEventArgs e)
        {
            string macrostring = "";
            string customName = "";

            try
            {
                if (customMacroName.Text != null || customMacroName.Text != "")
                {
                    customName = customMacroName.Text;
                }
                else
                {
                    throw new Exception("Macro name must not be empty.");
                }

                macrostring = MacroStringTxt.Text;

                tryLexMacroString(macrostring);

                if (!model.customMacroNames.Contains(customName))
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

            }
            catch(Exception err)
            {
                MessageBox.Show("Error: " + err.Message);
                return;
            }
        }

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
