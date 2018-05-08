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
using System.Text.RegularExpressions;

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

        /* Border drop handler for draggin a key onto the grid */
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

        /* Keycap drag event */
        private void keyRect_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Image img = (Image)sender;
            DataObject dataObj = new DataObject(img);
            DragDrop.DoDragDrop(img, dataObj, DragDropEffects.Move);
        }


      
        /* keycap event oppener */
        private void PopOpenSelector(object sender, RoutedEventArgs e)
        {


            keyDataForm.Visibility = Visibility.Visible;
            keyboardMatrixPanel.Visibility = Visibility.Hidden;
            visualControlPanel.Visibility = Visibility.Hidden;

            Button btn = (Button)sender;
            KeyCapButton data = (KeyCapButton)btn.DataContext;

            keyDataForm.DataContext = data;

        }

        /* User button event for creating the json layout */
        private void createJson_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                string KeyboardLayoutName = layoutNameTextbox.Text;     //Layout Name

                validateLayoutName(KeyboardLayoutName);

                qk.Keyboard keeb = new qk.Keyboard();
                List<QKeyCommon.Keyboard_items.Keyboard> JsonInfo = new List<QKeyCommon.Keyboard_items.Keyboard>();  //List Contains Json Info
                List<qk.Key_items.Key> KeyItems = getKeyData();  //List Contains Json Info using GetKeyData()



                if (mainWindow.layouteditormodel.SelectedDiodeDirection != null)
                {
                    keeb.spec.diode_direction = mainWindow.layouteditormodel.SelectedDiodeDirection.diodeValue;
                }
                else
                {
                    throw new Exception("No diode direction selected. [Define Keyboard Matrix]");
                }
                        

                keeb.ui_desc.rows = int.Parse(gridRow.Text);
                keeb.ui_desc.cols = int.Parse(gridColumn.Text);
                keeb.desc.product_name = model.layoutname;

                if(mainWindow.keyboardinfomodel.SelectedMicroProc != null)
                {
                    keeb.spec.avrdude.partno = mainWindow.keyboardinfomodel.SelectedMicroProc.mpCode;
                    keeb.spec.avrdude.partno_verbose = mainWindow.keyboardinfomodel.SelectedMicroProc.mpName;
                }
                else
                {
                    throw new Exception("We are confused as to how you got here without selecting a microprocessor.");
                }
                

                if(KeyItems.Count <= 0)
                {
                    throw new Exception("Keyboard should have keys added.");
                }

                keeb.keys = KeyItems;

                if(model.SelectedKeyboardRowPins.Count() <= 0 || model.SelectedKeyboardColPins.Count() <= 0)
                {
                    throw new Exception("Keyboard must have valid keyboard matrix size defined. Atleast 1 pin row/col [Define Keyboard Matrix]");
                }

                keeb.spec.matrix_spec.rows = model.SelectedKeyboardRowPins.Count();
                keeb.spec.matrix_spec.row_pins = model.KeyboardMatrixRow.TrimEnd(',').Split(',').ToList();

                keeb.spec.matrix_spec.cols = model.SelectedKeyboardColPins.Count();
                keeb.spec.matrix_spec.col_pins = model.KeyboardMatrixCol.TrimEnd(',').Split(',').ToList();

                //string jsonPath = (mainWindow.approot + KeyboardLayoutName + ".json");
                string output = JsonConvert.SerializeObject(keeb, Formatting.Indented); //Serialize the List and add to output string
                var userTemplatePath = System.IO.Path.Combine(mainWindow.userTemplatesFolderPath, KeyboardLayoutName + ".json");
                try
                {
                    System.IO.File.WriteAllText(userTemplatePath, output); //Save file
                }
                catch
                {
                    throw new Exception("Invalid Folder Name.");
                }


                mainWindow.keyboardinfomodel.SelectedJsonLayout.layoutPath = userTemplatePath;
                mainWindow.keyboardinfomodel.JsonLayouts.Add(new JsonTemplateLayout(keeb.desc.product_name, userTemplatePath));
                NavigationService.Navigate(mainWindow.bindingEditorPage);

            }
            catch(Exception err)
            {
                MessageBox.Show("An error occured while serializing the object: " + err.Message);
            }

        }


        /* Creates and returns a dropable border ui element */
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

        /* Creates the visual grid layout based on the size specified */
        private void CreateGrid(object sender, RoutedEventArgs e)
        {
            try
            {
                int RowIterations = int.Parse(gridRow.Text);
                int ColomnIterations = int.Parse(gridColumn.Text);

                if (visualEditorGrid.Children.Count != 0)
                {
                    visualEditorGrid.Children.Clear();
                    //gridRow.Clear();
                    //gridColumn.Clear();
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

        /* Retrive key data list from each of the keycap objects */
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

                    if(kcb.SelectedMatrixPinRow != null && kcb.SelectedMatrixPinCol != null)
                    {
                        key.matrix.row = kcb.SelectedMatrixPinRow;
                        key.matrix.col = kcb.SelectedMatrixPinCol;

                        keyData.Add(key);
                    }
                    else
                    {
                        throw new Exception("Key's should have their pin values set. [Click on the key to access pin data]");
                    }
                }
            }


            return keyData;
        }

        /* Popopen event for keyboard matrix panel */
        private void PopOpenKeyboardMatrixPanel(object sender, RoutedEventArgs e)
        {
            keyboardMatrixPanel.Visibility = Visibility.Visible;
            keyDataForm.Visibility = Visibility.Hidden;
            visualControlPanel.Visibility = Visibility.Hidden;

        }

        /* Click event for adding keyboard matrix row pin */
        private void addKeyboardRowMatrixPin(object sender, RoutedEventArgs e)
        {
            try
            {

                if (model.SelectedMatrixPin != null)
                {
                    
                    model.SelectedKeyboardRowPins.Add(model.SelectedMatrixPin);
                    model.KeyboardMatrixRow = "";

                    pinSelectionComboBox.SelectedIndex = -1;
                    pinSelectionComboBox.Text = "";
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

        /* Click event for adding column matrix pin to collection */
        private void addKeyboardColMatrixPin(object sender, RoutedEventArgs e)
        {
            try
            {

                if (model.SelectedMatrixPin != null)
                {
                    model.SelectedKeyboardColPins.Add(model.SelectedMatrixPin);
                    model.KeyboardMatrixCol = "";

                    pinSelectionComboBox.SelectedIndex = -1;
                    pinSelectionComboBox.Text = "";
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

        private void validateLayoutName(string layoutName)
        {
            if (layoutName.Length > 0 && layoutName.Length <= 100)
            {
                System.IO.DirectoryInfo di = null;
                System.IO.FileInfo fi = null;
                try
                {
                    di = new System.IO.DirectoryInfo(mainWindow.userTemplatesFolderPath + layoutName);
                }
                catch
                {
                    throw new Exception("Invalid Name layout name. \nMust not be empty. \nMust be a valid Windows Folder / File Name.");
                }


                fi = new System.IO.FileInfo(System.IO.Path.Combine(mainWindow.userTemplatesFolderPath, layoutName + ".json"));

                if(fi.Exists)
                {
                    throw new Exception("Template name already exists. \nTemplate names must be unique.");
                }

                return;         

            }

            throw new Exception("Invalid Name layout name. \nMust not be empty. \nMust be a valid Windows Folder Name.");

        }

        private void clearMatrixColPins(object sender, RoutedEventArgs e)
        {
            cleanSelectedPinComboBox();
            cleanMatrixColPins();
        }

        private void cleanMatrixColPins()
        {
            colPinKeyboardMatrix.Text = "";
            model.SelectedKeyboardColPins.Clear();
            model.KeyboardMatrixCol = "";
        }


        private void clearMatrixRowPins(object sender, RoutedEventArgs e)
        {
            cleanSelectedPinComboBox();
            cleanMatrixRowPins();
        }

        private void cleanMatrixRowPins()
        {
            rowPinKeyboardMatrix.Text = "";
            model.SelectedKeyboardRowPins.Clear();
            model.KeyboardMatrixRow = "";
        }

        private void cleanSelectedPinComboBox()
        {
            pinSelectionComboBox.SelectedIndex = -1;
            pinSelectionComboBox.Text = "";
        }

        private void cleanLayoutEditor()
        {
            layoutNameTextbox.Text = "";
            gridRow.Text = "";
            gridColumn.Text = "";
            visualEditorGrid.Children.Clear();
        }


        public void reset()
        {
            cleanLayoutEditor();
            cleanSelectedPinComboBox();
            cleanMatrixColPins();
            cleanMatrixRowPins();
        }

        private void addKeyPinRow(object sender, RoutedEventArgs e)
        {
            try
            {

                if (model.SupportedPins != null)
                {
                    KeyCapButton kcb = (KeyCapButton)keyDataForm.DataContext;
                    kcb.SelectedMatrixPinRow = ((MatrixPin)keycapMatrixPinSelection.SelectedItem).pinName;
                    keycapMatrixPinSelection.SelectedItem = null;
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

        private void clearKeycapPinRow(object sender, RoutedEventArgs e)
        {
            try
            {

                if (model.SupportedPins != null)
                {
                    KeyCapButton kcb = (KeyCapButton)keyDataForm.DataContext;
                    kcb.SelectedMatrixPinRow = null;
                }
                else
                {
                    throw new Exception("A pin must be selected first before trying to clear it.");
                }
            }
            catch (Exception err)
            {
                MessageBox.Show("Error: " + err.Message);
            }

        }

        private void addKeyPinCol(object sender, RoutedEventArgs e)
        {
            try
            {

                if (model.SupportedPins != null)
                {
                    KeyCapButton kcb = (KeyCapButton)keyDataForm.DataContext;
                    kcb.SelectedMatrixPinCol = ((MatrixPin)keycapMatrixPinSelection.SelectedItem).pinName;
                    keycapMatrixPinSelection.SelectedItem = null;
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

        private void clearKeycapPinCol(object sender, RoutedEventArgs e)
        {
            try
            {

                if (model.SupportedPins != null)
                {
                    KeyCapButton kcb = (KeyCapButton)keyDataForm.DataContext;
                    kcb.SelectedMatrixPinCol = null;
                }
                else
                {
                    throw new Exception("A pin must be selected first before trying to clear it.");
                }
            }
            catch (Exception err)
            {
                MessageBox.Show("Error: " + err.Message);
            }

        }

    }
}
