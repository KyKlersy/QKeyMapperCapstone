using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Web.Script.Serialization;
using qk = QKeyCommon.Keyboard_items;


namespace QKeyMapper
{
    //
    public partial class LayoutEditorPage : Page
    {
        

        public LayoutEditorPage()
        {
            InitializeComponent();

            BrushConverter bc = new BrushConverter();

            for (int i = 0; i < 10; i++)
            {
                RowDefinition rd = new RowDefinition();
                rd.Height = new GridLength(1.0, GridUnitType.Star);
                visualEditorGrid.RowDefinitions.Add(rd);

                /*Border rbd = new Border();
                rbd.Background = Brushes.Transparent;
                rbd.BorderBrush = (Brush)bc.ConvertFromString("#8D8D8D");
                rbd.BorderThickness = new Thickness(1.0);
                Grid.SetRow(rbd, i);
                visualEditorGrid.Children.Add(rbd);*/

                for (int j = 0; j < 10; j++)
                {
                    ColumnDefinition cd = new ColumnDefinition();
                    cd.Width = new GridLength(1.0, GridUnitType.Star);
                    visualEditorGrid.ColumnDefinitions.Add(cd);

                    Border cbd = new Border();
                    cbd.Background = Brushes.Transparent;
                    cbd.BorderBrush = (Brush)bc.ConvertFromString("#8D8D8D");
                    cbd.BorderThickness = new Thickness(1.0);
                    cbd.Name = "cell" + i + "_" + j;
                    cbd.MinWidth = 75;
                    cbd.MinHeight = 75;
                    cbd.HorizontalAlignment = HorizontalAlignment.Stretch;
                    cbd.VerticalAlignment = VerticalAlignment.Stretch;
                    cbd.AddHandler(Border.DropEvent, new DragEventHandler(Border_Drop));

                    Grid.SetRow(cbd, i);
                    Grid.SetColumn(cbd, j);
                    visualEditorGrid.Children.Add(cbd);






                }

            }
        }


        private void Border_Drop(object sender, DragEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine(e.OriginalSource.ToString());
            Border targetBorder = (Border)e.OriginalSource;
            Rectangle sentRect = (Rectangle)e.Data.GetData(typeof(Rectangle));
            Rectangle copyRect = new Rectangle();
            copyRect.Fill = sentRect.Fill;
            targetBorder.Child = copyRect;

        }
        qk.Keyboard keeb = new qk.Keyboard();

        private void keyRect_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Rectangle rec = (Rectangle)sender;
            DataObject dataObj = new DataObject(rec);
            DragDrop.DoDragDrop(rec, dataObj, DragDropEffects.Move);
            // Console.WriteLine(Grid.GetColumn(rec));
            //Console.WriteLine(Grid.GetRow(rec));
           // keeb.keys.Add(new qk.Key_items.Key() );


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
            JavaScriptSerializer ser = new JavaScriptSerializer(); // Serializer 
            string output = ser.Serialize(JsonInfo);                 //Serialize the List and add to output string
            string KeyboardLayoutName = layoutNameTextbox.Text;     //Layout Name
            System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"\" + KeyboardLayoutName + ".json", output); //Save file
            Console.WriteLine("Go this address to open Json File:" + AppDomain.CurrentDomain.BaseDirectory);     //File path
            MessageBox.Show("Keyboard Layout created in a Json file");



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
    }
}





*/


