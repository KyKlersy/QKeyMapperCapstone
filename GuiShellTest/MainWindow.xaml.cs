
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
using GuiShellTest.ViewModels;


namespace QKeyMapper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public keyboardInfoModel keyboardinfomodel;
        public layoutEditorModel layouteditormodel;
        public KeyBoardInfoPage keyboardInfoPage;
        public LayoutEditorPage layoutEditorPage;
        public BindingEditorPage bindingEditorPage;

        public MainWindow()
        {
            //0 set to 0 to load default panel
            //1 set to 1 to load layout editor panel
            //2 set to 2 to load binding editor panel
            //3 set to 3 to load macro editor panel
            //4 set to 4 to load flashing page panel
            int panelDebug = 0;

            keyboardinfomodel = new keyboardInfoModel();
            layouteditormodel = new layoutEditorModel();

            keyboardInfoPage = new KeyBoardInfoPage(this);
            layoutEditorPage = new LayoutEditorPage(this);
            bindingEditorPage = new BindingEditorPage(this);

            InitializeComponent();


            
            switch (panelDebug)
            {
                case 0:
                    mainFrame.Content = new KeyBoardInfoPage(this);
                    break;
                case 1:
                    mainFrame.Content = new LayoutEditorPage();
                    break;
                case 2:
                    mainFrame.Content = new BindingEditorPage();
                    break;
                case 3:
                    mainFrame.Content = new MacroEditorPage();
                    break;
            }


            //mainFrame.Content = new KeyBoardInfoPage();
        }
    }
}
