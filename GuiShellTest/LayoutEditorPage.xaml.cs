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
using GuiShellTest.Models;

namespace QKeyMapper
{

    public partial class LayoutEditorPage : Page
    {


        private MainWindow mainWindow;
        private layoutEditorModel model;

    
        public void ChangeButtonColor() { }

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
            keyboardMatrixPanel.Visibility = Visibility.Hidden;
            visualControlPanel.Visibility = Visibility.Hidden;

            Button btn = (Button)sender;
            KeyCapButton data = (KeyCapButton)btn.DataContext;

            keyDataForm.DataContext = data;

        }


        private void createJson_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string KeyboardLayoutName = layoutNameTextbox.Text;     //Layout Name
                qk.Keyboard keeb = new qk.Keyboard();
                List<QKeyCommon.Keyboard_items.Keyboard> JsonInfo = new List<QKeyCommon.Keyboard_items.Keyboard>();  //List Contains Json Info
                List<qk.Key_items.Key> KeyItems = getKeyData();  //List Contains Json Info using GetKeyData()

                if (KeyboardLayoutName.Length > 0 && KeyboardLayoutName.Length <= 100)
                {
                    if (mainWindow.layouteditormodel.SelectedDiodeDirection.diodeValue != null & mainWindow.layouteditormodel.SelectedDiodeDirection.diodeValue != "")
                        keeb.spec.diode_direction = mainWindow.layouteditormodel.SelectedDiodeDirection.diodeValue;

                    keeb.ui_desc.rows = int.Parse(gridRow.Text);
                    keeb.ui_desc.cols = int.Parse(gridColumn.Text);
                    keeb.desc.product_name = model.layoutname;
                    keeb.spec.avrdude.partno = mainWindow.keyboardinfomodel.SelectedMicroProc.mpCode;
                    keeb.spec.avrdude.partno_verbose = mainWindow.keyboardinfomodel.SelectedMicroProc.mpName;
                    keeb.keys = KeyItems;

                    keeb.spec.matrix_spec.rows = model.SelectedKeyboardRowPins.Count();
                    keeb.spec.matrix_spec.row_pins = model.KeyboardMatrixRow.TrimEnd(',').Split(',').ToList();

                    keeb.spec.matrix_spec.cols = model.SelectedKeyboardColPins.Count();
                    keeb.spec.matrix_spec.col_pins = model.KeyboardMatrixCol.TrimEnd(',').Split(',').ToList();

                    string output = JsonConvert.SerializeObject(keeb, Formatting.Indented); //Serialize the List and add to output string
                    System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"\" + KeyboardLayoutName + ".json", output); //Save file

                    string jsonPath = (AppDomain.CurrentDomain.BaseDirectory + @"\" + KeyboardLayoutName + ".json");
                    mainWindow.keyboardinfomodel.SelectedJsonLayout.layoutPath = jsonPath;
                    mainWindow.keyboardinfomodel.JsonLayouts.Add(new JsonTemplateLayout(keeb.desc.product_name, jsonPath));
                    NavigationService.Navigate(mainWindow.bindingEditorPage);

                }
            }
            catch { MessageBox.Show("An error occured while serializing the object"); }

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
                int RowIterations = int.Parse(gridRow.Text);
                int ColomnIterations = int.Parse(gridColumn.Text);

                if (visualEditorGrid.Children.Count != 0)
                {
                    visualEditorGrid.Children.Clear();
                    gridRow.Clear();
                    gridColumn.Clear();
                }

                if ( RowIterations > 100 || RowIterations <= 0 || ColomnIterations > 100 || ColomnIterations <= 0)
                {
                    MessageBox.Show("Row and Column values should be 0-100");
                }
                else
                {
                    GridCreate(RowIterations, ColomnIterations); // Make grid great again
                }

            }
            catch {
                MessageBox.Show("Not a valid integer input");
                visualEditorGrid.Children.Clear();
                gridRow.Clear();
                gridColumn.Clear();
            }
        }
    

        private void closeKeyDataForm(object sender, RoutedEventArgs e)
        {
            keyboardMatrixPanel.Visibility = Visibility.Hidden;
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

                    key.matrix.row = kcb.matrixrow.pinName;
                    key.matrix.col = kcb.matrixcol.pinName;

                    keyData.Add(key);
                }
            }


            return keyData;
        }

        private void PopOpenKeyboardMatrixPanel(object sender, RoutedEventArgs e)
        {
            keyboardMatrixPanel.Visibility = Visibility.Visible;
            keyDataForm.Visibility = Visibility.Hidden;
            visualControlPanel.Visibility = Visibility.Hidden;

        }

        private void addKeyboardRowMatrixPin(object sender, RoutedEventArgs e)
        {
            try
            {

                if (model.SelectedMatrixPin != null)
                {
                    model.SelectedKeyboardRowPins.Add(model.SelectedMatrixPin);
                    model.KeyboardMatrixRow = "";
                }
                else
                {
                    throw new Exception("A pin must be selected first before trying to add it.");
                }
            }
            catch(Exception err)
            {
                MessageBox.Show("Error: " + err.Message);
            }
            
        }

        private void addKeyboardColMatrixPin(object sender, RoutedEventArgs e)
        {
            try
            {

                if (model.SelectedMatrixPin != null)
                {
                    model.SelectedKeyboardColPins.Add(model.SelectedMatrixPin);
                    model.KeyboardMatrixCol = "";
                }
                else
                {
                    throw new Exception("A pin must be selected first before trying to add it.");
                }
            }
            catch (Exception err)
            {
                MessageBox.Show("Error: " + err.Message);
            }

        }

        private void clearMatrixColPins(object sender, RoutedEventArgs e)
        {

            colPinKeyboardMatrix.Text = "";
            model.SelectedKeyboardColPins.Clear();
            model.KeyboardMatrixCol = "";

        }

        private void clearMatrixRowPins(object sender, RoutedEventArgs e)
        {
            rowPinKeyboardMatrix.Text = "";
            model.SelectedKeyboardRowPins.Clear();
            model.KeyboardMatrixRow = "";
        }
    }
}
