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
using GuiShellTest.ViewModels;

namespace QKeyMapper
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class KeyBoardInfoPage : Page
    {
        private MainWindow mainWindow;
        public keyboardInfoModel keyboardinfomodel;

        public KeyBoardInfoPage()
        {

            keyboardinfomodel = new keyboardInfoModel();

            DataContext = keyboardinfomodel;

            InitializeComponent();


        }

        public KeyBoardInfoPage(MainWindow mainwindow)
        {
            mainWindow = mainwindow;

            keyboardinfomodel = mainwindow.keyboardinfomodel;

            DataContext = keyboardinfomodel;

            InitializeComponent();
        }

        //Todo :: add check for null selection, refuse navigation until choice is made
        // Add passing parameters of choice selections to binding editor page
        private void beginMappingButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (keyboardinfomodel.SelectedJsonLayout == null || keyboardinfomodel.SelectedMicroProc == null)
                {


                    throw new Exception("Both Microprocessor and Json layout must be selected.");

                }

                if (keyboardLayoutComboBox.SelectedValue.Equals("Custom"))
                {

                    //LayoutEditorPage layoutEditorPage = new LayoutEditorPage(mainWindow);
                    NavigationService.Navigate(mainWindow.layoutEditorPage);
                }
                else
                {

                    //BindingEditorPage bindingEditorPage = new BindingEditorPage(mainWindow);
                    NavigationService.Navigate(mainWindow.bindingEditorPage);

                }
            }
            catch(Exception err)
            {
                MessageBox.Show("Error: " + err.Message);
            }
        }        
    }
}
