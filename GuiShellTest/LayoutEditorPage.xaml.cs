using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Web.Script.Serialization;
using qk = QKeyCommon.Keyboard_items;
using Newtonsoft.Json;
using GuiShellTest.Controls;
using GuiShellTest.ViewModels;
using System.ComponentModel;
namespace QKeyMapper
{

    public partial class LayoutEditorPage : Page
    {


        private MainWindow mainWindow;
        private layoutEditorModel model;

    
        public void ChangeButtonColor() {


          
        }

        public LayoutEditorPage()
        {
        

            InitializeComponent();
            model = new layoutEditorModel();
            DataContext = model;
            keyDataForm.DataContext = null;

          
        }

        public LayoutEditorPage(MainWindow mainwindow)
        {
            mainWindow = mainwindow;
            InitializeComponent();
            model = mainwindow.layouteditormodel;
            DataContext = model;
            keyDataForm.DataContext = null;

        }

        //Kyles` function to create grid passing only row and coloumn recieved from textboxes
        private void GridCreate(int row, int coloumn)
        {
            BrushConverter bc = new BrushConverter();


            for (int defaultGridSize = 0; defaultGridSize < row; defaultGridSize++)
            {
                RowDefinition rd = new RowDefinition();
                rd.Height = new GridLength(0, GridUnitType.Star);
                visualEditorGrid.RowDefinitions.Add(rd);

            }

            for (int defaultGridSize = 0; defaultGridSize < coloumn; defaultGridSize++)
            {
                ColumnDefinition cd = new ColumnDefinition();
                cd.Width = new GridLength(0, GridUnitType.Star);
                visualEditorGrid.ColumnDefinitions.Add(cd);
            }

            for (int i = 0; i < row; i++)
            {

                for (int j = 0; j < coloumn; j++)
                {

                    UIElement dropzone = dropableBorder(row, coloumn, bc);
                    Grid.SetRow(dropzone, i);
                    Grid.SetColumn(dropzone, j);
                    visualEditorGrid.Children.Add(dropzone);
                    dropzone = null;

                }
            }

        }


        private void Border_Drop(object sender, DragEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine(e.OriginalSource.ToString());
            if (e.OriginalSource is Border)
            {
                Border targetBorder = (Border)e.OriginalSource;
                KeyCapButton kcb = new KeyCapButton();

                kcb.keyButton.Click += PopOpenSelector;

                targetBorder.Child = kcb;
            }
        }


        private void keyRect_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Image img = (Image)sender;
            DataObject dataObj = new DataObject(img);
            DragDrop.DoDragDrop(img, dataObj, DragDropEffects.Move);
        }


        private void PopOpenSelector(object sender, RoutedEventArgs e)
        {
            keyDataForm.Visibility = Visibility.Visible;
            visualControlPanel.Visibility = Visibility.Hidden;

            Button btn = (Button)sender;
            KeyCapButton data = (KeyCapButton)btn.DataContext;

            keyDataForm.DataContext = data;

        }


        private void createJson_Click(object sender, RoutedEventArgs e)
        {

            string KeyboardLayoutName = layoutNameTextbox.Text;     //Layout Name
           
            qk.Keyboard keeb = new qk.Keyboard();
            List<QKeyCommon.Keyboard_items.Keyboard> JsonInfo = new List<QKeyCommon.Keyboard_items.Keyboard>(1);  //List Contains Json Info
            List<qk.Key_items.Key> KeyItems = getKeyData();  //List Contains Json Info using GetKeyData()
            try
            {
                if (mainWindow.layouteditormodel.SelectedDiodeDirection.diodeValue != null & mainWindow.layouteditormodel.SelectedDiodeDirection.diodeValue != "")
                    keeb.spec.diode_direction = mainWindow.layouteditormodel.SelectedDiodeDirection.diodeValue;
                    //Debug.WriteLine(mainWindow.layouteditormodel.SelectedDiodeDirection.diodeValue);
                    //    { DiodeLabel.Visibility = Visibility.Visible; }
                    //    keeb.spec.diode_direction = mainWindow.layouteditormodel.SelectedDiodeDirection.diodeValue;

                keeb.spec.avrdude.partno = mainWindow.keyboardinfomodel.SelectedMicroProc.mpCode;
                keeb.spec.avrdude.partno = mainWindow.keyboardinfomodel.SelectedMicroProc.mpName;
                keeb.keys = KeyItems;
                JsonInfo.Add(keeb);
                string output = JsonConvert.SerializeObject(JsonInfo, Formatting.Indented); //Serialize the List and add to output string
                System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"\" + KeyboardLayoutName + ".json", output); //Save file
                Console.WriteLine("Go this address to open Json File:" + AppDomain.CurrentDomain.BaseDirectory);     //File path
                mainWindow.keyboardinfomodel.SelectedJsonLayout.layoutPath = (AppDomain.CurrentDomain.BaseDirectory + @"\" + KeyboardLayoutName + ".json");
                NavigationService.Navigate(new BindingEditorPage(mainWindow));
                //MessageBox.Show("Keyboard Layout created in a Json file");
            }
            catch { MessageBox.Show("An error occured while serializing the object"); }




            /* Commented out your prior code, to hook some things while testing::
                You can retrieve the information selected from the first two combo boxe's like below by calling into the keyboardinfomodel
                see keyboardinfomodel class under viewmodels 
                SelectedMicroProc , SelectedJsonLayout are both class properties, see their data model classes in the folder models.

                with the way this is setup you should be able to just call into the two class properties above into their class object members to get the data
                you need to add serialize out a complete Keyboard.cs

                use the function getKeyData, returns a list of qk.Key_Items.Key. The line below is debugging it just prints out the list contents for key info.
                You can test it by creating a grid, adding a key, clicking the key, adding information. It will print the console the matrix row/col entered for the key.
                it will return empty nulls for parts of the grid that no key exists, this should serialize out nicely.

          

            Debug.WriteLine("Selected microproc: " + mainWindow.keyboardinfomodel.SelectedMicroProc.mpName);


            List<qk.Key_items.Key> keyD = getKeyData();
            keyD.ForEach(qd =>
            {
                Debug.WriteLine("mrow: " + qd.matrix.row + " mcol: " + qd.matrix.col);
            });
               */
        }

        private static List<qk.Key_items.Key> GetKeyItems(List<qk.Key_items.Key> KeyItems)
        {
            return KeyItems;
        }

        private void removeRowButton_Click(object sender, RoutedEventArgs e)
        {
            var lastrow = (visualEditorGrid.RowDefinitions.Count - 1);
            var lastcol = (visualEditorGrid.ColumnDefinitions.Count - 1);

            UIElement element = null;
            
            for (var col = lastcol; col >= 0; col--)
            {

                element = visualEditorGrid.Children.Cast<UIElement>()
                   .First(i => Grid.GetRow(i) == lastrow && Grid.GetColumn(i) == col);
                visualEditorGrid.Children.Remove(element);

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

        private void CreateGrid(object sender, RoutedEventArgs e)
        {
            try
            {
                int RowIterations = int.Parse(Row.Text);
                int ColomnIterations = int.Parse(Column.Text);
                if ((visualEditorGrid.Children.Count != 0)|| RowIterations > 100 || RowIterations <= 0 || ColomnIterations > 100 || ColomnIterations <= 0)
                {
                    MessageBox.Show("Row and Column values should be 0-100");
                    visualEditorGrid.Children.Clear();
                    Row.Clear();
                    Column.Clear();
                }

                else
                {
                    GridCreate(RowIterations, ColomnIterations); // Make grid great again
                }
            }
            catch {
                Console.WriteLine("Not a valid input");
                visualEditorGrid.Children.Clear();
                Row.Clear();
                Column.Clear();
            }
        }
    

        private void closeKeyDataForm(object sender, RoutedEventArgs e)
        {

            keyDataForm.Visibility = Visibility.Hidden;
            visualControlPanel.Visibility = Visibility.Visible;

        }

        private List<qk.Key_items.Key> getKeyData()
        {

            List<qk.Key_items.Key> keyData = new List<qk.Key_items.Key>();
            Border bd;
            

            foreach (UIElement control in visualEditorGrid.Children)
            {
                if (control is Border && ((Border)control).Child != null)
                {
                    bd = (Border)control;
                    KeyCapButton kcb = (KeyCapButton)bd.Child;
                    qk.Key_items.Key key = new qk.Key_items.Key();

                    key.graphics.row = Grid.GetRow(control);
                    key.graphics.col = Grid.GetColumn(control);
                    key.graphics.text = kcb.text;

                    key.matrix.row = kcb.matrixrow;
                    key.matrix.col = kcb.matrixcol;

                    keyData.Add( key );
           
                }
                else
                {
                    qk.Key_items.Key key = new qk.Key_items.Key();
                    keyData.Add(key);
                }
            }


            return keyData;
        }
    }
}
