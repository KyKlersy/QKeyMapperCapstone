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
    /// Interaction logic for Page3.xaml
    /// </summary>
    public partial class FlashingPage : Page
    {
        public FlashingPage()
        {
            InitializeComponent();
        }

        private void returnButton_Click(object sender, RoutedEventArgs e)
        {
            KeyBoardInfoPage keyboardInfoPage = new KeyBoardInfoPage();
            NavigationService.Navigate(keyboardInfoPage);
        }
    }
}
