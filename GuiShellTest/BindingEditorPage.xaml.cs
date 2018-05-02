using GuiShellTest.Controls;
using GuiShellTest.Models;
using GuiShellTest.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using QK = QKeyCommon.Keyboard_items;
using QKeyCommon;

namespace QKeyMapper
{
    /// <summary>
    /// Interaction logic for Page2.xaml
    /// </summary>
    public partial class BindingEditorPage : Page
    {
        private bindingEditorModel model;
        private MainWindow mainWindow;
        private QK.Keyboard keeb;

        public BindingEditorPage()
        {
            InitializeComponent();
            model = new bindingEditorModel();
            DataContext = model;
            OnHoldMacroTextBlock.DataContext = null;
        }

        public BindingEditorPage(MainWindow mainWindow) {
            InitializeComponent();
            model = mainWindow.bindingeditormodel;
            this.mainWindow = mainWindow;
            DataContext = model;

            Loaded += loadJson;

        }

        private QK.Keyboard get_keyboard_json_from_path(string path)
        {
            string json_raw = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<QK.Keyboard>(json_raw);
        }


        private void createNewMacroButton_Click(object sender, RoutedEventArgs e)
        {

            NavigationService.Navigate(mainWindow.macroEditorPage);
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
                    //open model here.
                    kcb.keyButton.Click += selectKeyView;
                    keyBoardGridPicker.Children.Add(kcb);
                Grid.SetRow(kcb, row);
                Grid.SetColumn(kcb, col);
            }
        }

        private void selectKeyView(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            KeyCapButton data = (KeyCapButton)btn.DataContext;

            OnTapMacroTextBlock.DataContext = data.DataContext;
            OnHoldMacroTextBlock.DataContext = data.DataContext;

        }

        private void loadJson(object sender, RoutedEventArgs e)
        {
            if(mainWindow.keyboardinfomodel.SelectedJsonLayout == null)
            {

               // return;
            }

            //store the path to the json file
            var json_path = mainWindow.keyboardinfomodel.SelectedJsonLayout.layoutPath;
            //var json_path = @"C:\Users\Kyle\Documents\Visual Studio 2015\Projects\GuiShellTest\GuiShellTest\Resources\JsonDefaultLayouts\6ball_no_macro.json";
            //create keyboard oject and initialize with the contents within the json file
            try
            {
                keeb = get_keyboard_json_from_path(json_path);
                //create the grid for the binding editor
                GridCreate(keeb.ui_desc.rows, keeb.ui_desc.cols);
                //generate the json specified keycaps
                generateKeyCaps(keeb);
            }
            catch (Exception dse)
            {
                Debug.WriteLine("Error Deserializing: " + dse);
            }
        }

        private void goBackToHome(object sender, RoutedEventArgs e)
        {
            //KeyBoardInfoPage keyboardInfoPage = new KeyBoardInfoPage();
            Loaded -= loadJson;
            NavigationService.Navigate(mainWindow.keyboardInfoPage);
        }

        private void beginFlashingButton_Click(object sender, RoutedEventArgs e)
        {

            string jsonPath = "";
            try
            {
                keeb.keys.Clear();
                keeb.keys = getKeyData();
                string output = JsonConvert.SerializeObject(keeb, Formatting.Indented); //Serialize the List and add to output string
                System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"\" + keeb.desc.product_name + ".json", output); //Save file
                Console.WriteLine("Go this address to open Json File:" + AppDomain.CurrentDomain.BaseDirectory);     //File path
                jsonPath = (AppDomain.CurrentDomain.BaseDirectory + @"\" + keeb.desc.product_name + ".json");
 
            }
            catch
            {
                MessageBox.Show("An error occured while serializing the object");
                return;
            }

            try
            {
                var solution_dir = new DirectoryInfo(System.AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName;
                var qmk_cgen = System.IO.Path.Combine(solution_dir, "QMKCGen", "bin", "debug", "QMKCGen.exe");
                string process_output = Exec.launch(qmk_cgen, jsonPath);
                if (process_output != string.Empty) 
                {
                    MessageBox.Show(process_output);
                }
                Debug.WriteLine("[me]QMKCGen.exe exited with code: " + process_output);
            }
            catch (Exception err)
            {
                MessageBox.Show("Error launching flashing utility\n" + 
                                "Error Message:" + err.Message
                );
                return;
            }
        }

        private void SetOnTapMacroModel(object sender, RoutedEventArgs e)
        {
            int selected = 0;
            if (OnTapMacroTextBlock.DataContext is KeyCapButton)
            {
                try
                {
                    selected = tryGetSelectionOnlyOne();
                    KeyCapButton dc = (KeyCapButton)OnTapMacroTextBlock.DataContext;

                    if (selected == 0)
                    {
                        //OnTapMacroTextBlock.Text = model.SelectedKeyMacroEditor.macroName;
                        dc.OnTapMacro = model.SelectedKeyMacroEditor;
                        dc.onTapMacroListID = 0;
                        clearMacroComboBox();
                    }
                    else
                    {
                        //OnTapMacroTextBlock.Text = model.SelectedKeySingleMacro.macroName;
                        dc.OnTapMacro = model.SelectedKeySingleMacro;
                        dc.onTapMacroListID = 1;
                        clearSingleKeyComboBox();
                    }

                }
                catch(Exception err)
                {
                    MessageBox.Show("Error: " + err.Message);

                    clearMacroComboBox();
                    clearSingleKeyComboBox();
                }
            }
            else
            {
                MessageBox.Show("You must first select a key before trying to set its binding.");
            }
            
        }

        private void clearSelectedComboBoxes(object sender, RoutedEventArgs e)
        {

            clearMacroComboBox();
            clearSingleKeyComboBox();

            if (OnTapMacroTextBlock.DataContext is KeyCapButton)
            {
                KeyCapButton dc = (KeyCapButton)OnTapMacroTextBlock.DataContext;
                dc.OnTapMacroName = "";
                dc.OnHoldMacroName = "";
            }
            else
            {
                MessageBox.Show("You must first select a key before trying to clear its binding.");
            }
        }

        private void SetOnHoldMacro(object sender, RoutedEventArgs e)
        {
            int selected = 0;
            if (OnTapMacroTextBlock.DataContext is KeyCapButton)
            {
                try
                {
                    selected = tryGetSelectionOnlyOne();
                    KeyCapButton dc = (KeyCapButton)OnTapMacroTextBlock.DataContext;

                    if(selected == 0)
                    {
                        //OnHoldMacroTextBlock.Text = model.SelectedKeyMacroEditor.macroName;

                        validateOnHoldMacro(model.SelectedKeyMacroEditor);

                        dc.OnHoldMacro = model.SelectedKeyMacroEditor;
                        dc.onHoldMacroListID = 0;
                        clearMacroComboBox();

                    }
                    else
                    {
                        //OnHoldMacroTextBlock.Text = model.SelectedKeySingleMacro.macroName;

                        validateOnHoldMacro(model.SelectedKeySingleMacro);

                        dc.OnHoldMacro = model.SelectedKeySingleMacro;
                        dc.onHoldMacroListID = 1;
                        clearSingleKeyComboBox();
                    }

                }
                catch (Exception err)
                {
                    MessageBox.Show("Error: " + err.Message);

                    clearMacroComboBox();
                    clearSingleKeyComboBox();

                }
            }
            else
            {
                MessageBox.Show("You must first select a key before trying to set its binding.");
            }
        }

        private int tryGetSelectionOnlyOne()
        {
            if((singleKeyChoiceComboBox.SelectedIndex == -1 && macroKeyChoiceComboBox.SelectedIndex != -1) ^ (macroKeyChoiceComboBox.SelectedIndex == -1 && singleKeyChoiceComboBox.SelectedIndex != -1))
            {
                Debug.WriteLine("Exclusive or slected");
                if(singleKeyChoiceComboBox.SelectedIndex == -1)
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                throw new Exception("Only one combo box choice for a macro allowed.");
            }

        }

        private List<QK.Key_items.Key> getKeyData()
        {

            List<QK.Key_items.Key> keyData = new List<QK.Key_items.Key>();

            foreach (UIElement control in keyBoardGridPicker.Children)
            {
                if (control is KeyCapButton)
                {
                    keyData.Add(((KeyCapButton)control).getKeyItem());
                }
            }

            return keyData;
        }

        private void validateOnHoldMacro(KeyMacro macroSelected)
        {

            foreach(var modkey in model.validHoldMacro)
            {
                if (modkey.macroString[1].Equals(macroSelected.macroString[1]))
                {
                    return;
                }
            }

            throw new Exception("Invalid on hold macro must start with a modifier key.");

        }


        private void clearMacroComboBox()
        {
            macroKeyChoiceComboBox.SelectedIndex = -1;
            macroKeyChoiceComboBox.Text = "";
        }

        private void clearSingleKeyComboBox()
        {
            singleKeyChoiceComboBox.SelectedIndex = -1;
            singleKeyChoiceComboBox.Text = "";
        }
    }
}
