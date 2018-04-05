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


namespace QKeyMapper
{
    //omercan
    public partial class LayoutEditorPage : Page
    {


        public LayoutEditorPage()
        {
            InitializeComponent();
           
           
        }

        //Kyles` function to create grid passing only row and coloumn recieved from textboxes
        private void GridCreate(int row,int coloumn)
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
            if(e.OriginalSource is Border)
            {
                Border targetBorder = (Border)e.OriginalSource;

                targetBorder.Child = new KeyCapButton();
            }
        }

        qk.Keyboard keeb = new qk.Keyboard();

        private void keyRect_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Image img = (Image)sender;
            DataObject dataObj = new DataObject(img);
            DragDrop.DoDragDrop(img, dataObj, DragDropEffects.Move);
        }

        private void CreateGrid_Click(object sender, RoutedEventArgs e)
        {


        }


        private void createJson_Click(object sender, RoutedEventArgs e)
        {
            List<QKeyCommon.Keyboard_items.Keyboard> JsonInfo = new List<QKeyCommon.Keyboard_items.Keyboard>(1);  //List Contains Json Info


            //foreach (var item in LayoutEditorPage.)
            //{
            //    var key = new qk.Key_items.Key();
            //    keeb.spec.matrix_spec.col_pins.Add("F0");
            //    key.matrix.col = item.col;
            //    key.matrix.row = item.row;
            //    keeb.keys.Add(key);
            //}

            JsonInfo.Add(keeb);

            string output = JsonConvert.SerializeObject(JsonInfo, Formatting.Indented); //Serialize the List and add to output string
            string KeyboardLayoutName = layoutNameTextbox.Text;     //Layout Name
            System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"\" + KeyboardLayoutName + ".json", output); //Save file
            Console.WriteLine("Go this address to open Json File:" + AppDomain.CurrentDomain.BaseDirectory);     //File path
            MessageBox.Show("Keyboard Layout created in a Json file");



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

                if(visualEditorGrid.Children.Count != 0)
                {
                    visualEditorGrid.Children.Clear();
                }

                GridCreate(RowIterations, ColomnIterations); // Make grid great again
            }
            catch {
                Console.WriteLine("not valid input");
            }
}

       
    }
}

/*  
public class KeyboardLayout
{

    public string KeyName { get; set; }
    public int Row { get; set; }
    public int Coloumn { get; set; }



    public override string ToString()
    {
        return "keyname:" + this.KeyName + " ROW: " + this.Row + " Coloumn:" + this.Coloumn;
        private void addRowButton_Click(object sender, RoutedEventArgs e)
        {
            RowDefinition rd = new RowDefinition();
            rd.Height = new GridLength(1.0, GridUnitType.Star);
            visualEditorGrid.RowDefinitions.Add(rd);



            var newRowCount = (visualEditorGrid.ColumnDefinitions.Count - 1);
            BrushConverter bc = new BrushConverter();
            UIElement borderElement = null;

            for(int newGridItems = 0; newGridItems <= newRowCount; newGridItems++)
            {
                Debug.WriteLine("Row count " + (visualEditorGrid.RowDefinitions.Count - 1));
                Debug.WriteLine("Column count " + (newGridItems));

                borderElement = dropableBorder((newRowCount + 1), newGridItems, bc);
                Grid.SetRow(borderElement, newRowCount);
                Grid.SetColumn(borderElement, newGridItems);
                Debug.WriteLine("Row: " + newRowCount + "Col: " + newGridItems);
                visualEditorGrid.Children.Add(borderElement);
            }

            //Debug.WriteLine("New Row added: " + (visualEditorGrid.RowDefinitions.Count - 1));
        }
    }
}*/


        // Create Grid Kyles` way...
/*
       BrushConverter bc = new BrushConverter();
       UIElement dropBorder = null;


       for (int defaultGridSize = 0; defaultGridSize < 10; defaultGridSize++)
       {
           RowDefinition rd = new RowDefinition();
           rd.Height = new GridLength(1.0, GridUnitType.Star);
           visualEditorGrid.RowDefinitions.Add(rd);

           ColumnDefinition cd = new ColumnDefinition();
           cd.Width = new GridLength(1.0, GridUnitType.Star);
           visualEditorGrid.ColumnDefinitions.Add(cd);


       }

       for (int i = 0; i < 10; i++)
       {


           /*Border rbd = new Border();
           rbd.Background = Brushes.Transparent;
           rbd.BorderBrush = (Brush)bc.ConvertFromString("#8D8D8D");
           rbd.BorderThickness = new Thickness(1.0);
           Grid.SetRow(rbd, i);
           visualEditorGrid.Children.Add(rbd); //

           for (int j = 0; j < 10; j++)
           {

               dropBorder = dropableBorder(i, j, bc);


               Grid.SetRow(dropBorder, i);
               Grid.SetColumn(dropBorder, j);
               Debug.WriteLine("Row: " + i + "Col: " + j);
               visualEditorGrid.Children.Add(dropBorder);

           }

       } 
   */
