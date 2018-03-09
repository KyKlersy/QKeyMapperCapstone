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

namespace QKeyMapper
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class KeyBoardInfoPage : Page
    {
        public KeyBoardInfoPage()
        {
            InitializeComponent();
        }

        private void beginMappingButton_Click(object sender, RoutedEventArgs e)
        {
            if (keyboardLayoutComboBox.SelectedItem == null)
            {
                BindingEditorPage bindingEditorPage = new BindingEditorPage();
                NavigationService.Navigate(bindingEditorPage);
            }
            else
            {
                LayoutEditorPage layoutEditorPage = new LayoutEditorPage();
                NavigationService.Navigate(layoutEditorPage);
            }


        }
    }
}
