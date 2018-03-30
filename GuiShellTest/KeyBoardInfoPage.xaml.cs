using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Reflection;

namespace QKeyMapper
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class KeyBoardInfoPage : Page
    {
        public Dictionary<String, String> supportedMC { get; set; }
        public Dictionary<String, String> jsonLayouts { get; set; }

        public KeyBoardInfoPage()
        {

            supportedMC = new Dictionary<string, string>();
            jsonLayouts = new Dictionary<string, string>();
            
            this.DataContext = this;

            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "GuiShellTest.Resources.csvTest.csv";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                while (!reader.EndOfStream)
                {
                    var readLine = reader.ReadLine();
                    var tokens = readLine.Split(',');
                    supportedMC.Add(tokens[0], tokens[1]);
                }
            }

            string rootDirectory = System.IO.Path.GetDirectoryName(assembly.Location);
            string jsonResourceURI = "Resources" + System.IO.Path.DirectorySeparatorChar + "JsonDefaultLayouts" + System.IO.Path.DirectorySeparatorChar;
            string jsonFilePath = System.IO.Path.Combine(rootDirectory, jsonResourceURI);

            string[] supportedJsonLayouts = Directory.GetFiles(jsonFilePath, "*.json");


            foreach(string jsonConfigPath in supportedJsonLayouts )
            {
                string fileName = System.IO.Path.GetFileName(jsonConfigPath);
                fileName = fileName.Substring(0, (fileName.Length - 5));

                jsonLayouts.Add(fileName, jsonConfigPath);
            }

            jsonLayouts.Add("Custom", "Custom");

            InitializeComponent();

            microControllerComboBox.ItemsSource = supportedMC;
            microControllerComboBox.DisplayMemberPath = "Key";
            microControllerComboBox.SelectedValuePath = "Value";
            
            
            keyboardLayoutComboBox.ItemsSource = jsonLayouts;
            keyboardLayoutComboBox.DisplayMemberPath = "Key";
            keyboardLayoutComboBox.SelectedValuePath = "Value";

        }

        //Todo :: add check for null selection, refuse navigation until choice is made
        // Add passing parameters of choice selections to binding editor page
        private void beginMappingButton_Click(object sender, RoutedEventArgs e)
        {
            if (keyboardLayoutComboBox.SelectedValue.Equals("Custom"))
            {
                LayoutEditorPage layoutEditorPage = new LayoutEditorPage();
                NavigationService.Navigate(layoutEditorPage);
            }
            else
            {
                BindingEditorPage bindingEditorPage = new BindingEditorPage();
                NavigationService.Navigate(bindingEditorPage);

            }


        }


    }
}
