
using System.Windows;
using GuiShellTest.ViewModels;
using System.Windows.Navigation;
using System.Windows.Input;
using System;
using System.IO;

namespace QKeyMapper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// Main window of application used to bootstrap all pages and view models.
    /// </summary>
    public partial class MainWindow : Window
    {
        /* View models */
        public keyboardInfoModel keyboardinfomodel;
        public layoutEditorModel layouteditormodel;
        public bindingEditorModel bindingeditormodel;

        /* Pages */
        public KeyBoardInfoPage keyboardInfoPage;
        public LayoutEditorPage layoutEditorPage;
        public BindingEditorPage bindingEditorPage;
        public MacroEditorPage macroEditorPage;

        public NavigationService nav;

        public string approot = AppDomain.CurrentDomain.BaseDirectory;
        public string userTemplatesFolderPath;
        public string macroFolderPath;


        public MainWindow()
        {

            userTemplatesFolderPath = approot + "UserTemplates";
            Directory.CreateDirectory(userTemplatesFolderPath);

            macroFolderPath = approot + "UserMacros";
            Directory.CreateDirectory(macroFolderPath);

            //0 set to 0 to load default panel
            //1 set to 1 to load layout editor panel
            //2 set to 2 to load binding editor panel
            //3 set to 3 to load macro editor panel
            int panelDebug = 0;

            keyboardinfomodel = new keyboardInfoModel();
            layouteditormodel = new layoutEditorModel();
            bindingeditormodel = new bindingEditorModel();

            /* Pass the main window for reference to other models in the page constructors. */
            keyboardInfoPage = new KeyBoardInfoPage(this);
            layoutEditorPage = new LayoutEditorPage(this);
            bindingEditorPage = new BindingEditorPage(this);
            macroEditorPage = new MacroEditorPage(this);

            InitializeComponent();

            nav = NavigationService.GetNavigationService(mainFrame);

          

            switch (panelDebug)
            {
                case 0:
                    //mainFrame.Content = new KeyBoardInfoPage(this);
                    mainFrame.Navigate(keyboardInfoPage);
                    break;
                case 1:
                    //mainFrame.Content = new LayoutEditorPage(this);
                    mainFrame.Navigate(layoutEditorPage);
                    break;
                case 2:
                    //mainFrame.Content = new BindingEditorPage(this);
                    mainFrame.Navigate(bindingEditorPage);
                    break;
                case 3:
                    //mainFrame.Content = new MacroEditorPage();
                    mainFrame.Navigate(macroEditorPage);
                    break;
            }

        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            NavigationService nav = NavigationService.GetNavigationService(mainFrame);
            nav.Navigated += Nav_Navigated;
        }

        private void Nav_Navigated(object sender, NavigationEventArgs e)
        {
            NavigationCommands.BrowseBack.InputGestures.Clear();
            NavigationCommands.BrowseForward.InputGestures.Clear();
        }

        private void resizeWindowHook(object sender, System.EventArgs e)
        {
            this.Width = 800;
            this.Height = 600;


        }
    }
}
