using GuiShellTest.Controls;
using GuiShellTest.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using QK = QKeyCommon.Keyboard_items;

namespace QKeyMapper
{
    /// <summary>
    /// Interaction logic for Page2.xaml
    /// </summary>
    public partial class BindingEditorPage : Page
    {
        private bindingEditorModel model;
        private MainWindow mainWindow;

        public BindingEditorPage()
        {
            InitializeComponent();
            model = new bindingEditorModel();
            DataContext = model;
        }

        public BindingEditorPage(MainWindow mainWindow) {
            InitializeComponent();
            model = new bindingEditorModel();
            this.mainWindow = mainWindow;
            DataContext = model;

            Loaded += loadJson;

        }

        private QK.Keyboard get_keyboard_json_from_path(string path)
        {
            string json_raw = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<QK.Keyboard>(json_raw);
        }

        private void beginFlashingButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void createNewMacroButton_Click(object sender, RoutedEventArgs e)
        {
            //KeyBoardInfoPage keyboardInfoPage = new KeyBoardInfoPage();
            Loaded -= loadJson;
            NavigationService.Navigate(mainWindow.keyboardInfoPage);
        }

        private void GridCreate(int row, int coloumn)
        {
            BrushConverter bc = new BrushConverter();


            for (int defaultGridSize = 0; defaultGridSize < row; defaultGridSize++)
            {
                RowDefinition rd = new RowDefinition();
                rd.Height = new GridLength(0, GridUnitType.Star);
                keyBoardGridPicker.RowDefinitions.Add(rd);
            }

            for (int defaultGridSize = 0; defaultGridSize < coloumn; defaultGridSize++)
            {
                ColumnDefinition cd = new ColumnDefinition();
                cd.Width = new GridLength(0, GridUnitType.Star);
                keyBoardGridPicker.ColumnDefinitions.Add(cd);
            }

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < coloumn; j++)
                {
                    UIElement dropzone = dropableBorder(row, coloumn, bc);
                    Grid.SetRow(dropzone, i);
                    Grid.SetColumn(dropzone, j);
                    keyBoardGridPicker.Children.Add(dropzone);
                    dropzone = null;
                }
            }
        }

        public UIElement dropableBorder(int rowNum, int colNum, BrushConverter bc)
        {
            Border cbd = new Border();
            cbd.Background = Brushes.Transparent;
            cbd.BorderBrush = (Brush)bc.ConvertFromString("#8D8D8D");
            cbd.BorderThickness = new Thickness(1.0);
            cbd.Name = "cell" + rowNum + "_" + colNum;
            cbd.MinWidth = 75;
            cbd.MinHeight = 75;
            cbd.HorizontalAlignment = HorizontalAlignment.Stretch;
            cbd.VerticalAlignment = VerticalAlignment.Stretch;
            cbd.AddHandler(Border.DropEvent, new DragEventHandler(Border_Drop));

            return cbd;
        }

        private void Border_Drop(object sender, DragEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine(e.OriginalSource.ToString());
            if (e.OriginalSource is Border)
            {
                Border targetBorder = (Border)e.OriginalSource;
                KeyCapButton kcb = new KeyCapButton();

                //kcb.keyButton.Click += PopOpenSelector;

                targetBorder.Child = kcb;
            }
        }

        private void generateKeyCaps(QK.Keyboard kc) {
            foreach (var key in kc.keys) {
                int row = key.graphics.row;
                int col = key.graphics.col;
                KeyCapButton kcb = new KeyCapButton(key);
                if (key.matrix.row == null && key.matrix.col == null)
                {/*skip*/}
                else
                    keyBoardGridPicker.Children.Add(kcb);
                Grid.SetRow(kcb, row);
                Grid.SetColumn(kcb, col);
            }
        }

        private void loadJson(object sender, RoutedEventArgs e)
        {
            //store the path to the json file
            var json_path = mainWindow.keyboardinfomodel.SelectedJsonLayout.layoutPath;
            //create keyboard oject and initialize with the contents within the json file
            try
            {
                QK.Keyboard current_layout = get_keyboard_json_from_path(json_path);
                //create the grid for the binding editor
                GridCreate(current_layout.ui_desc.rows, current_layout.ui_desc.cols);
                //generate the json specified keycaps
                generateKeyCaps(current_layout);
            }
            catch (Exception dse)
            {
                Debug.WriteLine("Error Deserializing: " + dse);
            }
        }

    }
}
