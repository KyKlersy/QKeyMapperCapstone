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
    /// Interaction logic for Page2.xaml
    /// </summary>
    public partial class BindingEditorPage : Page
    {
        public BindingEditorPage()
        {
            InitializeComponent();
        }

        private void beginFlashingButton_Click(object sender, RoutedEventArgs e)
        {
            FlashingPage flashingPage = new FlashingPage();
            NavigationService.Navigate(flashingPage);
        }

        private void createNewMacroButton_Click(object sender, RoutedEventArgs e)
        {
            KeyBoardInfoPage keyboardInfoPage = new KeyBoardInfoPage();
            NavigationService.Navigate(keyboardInfoPage);
        }
    }
}
