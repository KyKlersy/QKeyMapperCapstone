using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Web.Script.Serialization;



namespace QKeyMapper
{
    /* REPLACED WITH CHRIS`S CLASSES

    public class Rootobject
    {
        public string keyname { get; set; }
        public Rootobject( string key ,string width, int row, int column, string text, object[] on_tap, int Matrixrow, int Matrixcolumn)
        {
            this.keyname = key;
            new Graphics(width, row, column, text);
            new Binding(on_tap);
            new Matrix(Matrixrow, Matrixcolumn);

        }

        public Key key { get; set; }
    }

    public class Key
    {
        public Graphics graphics { get; set; }
        public Binding binding { get; set; }
        public Matrix matrix { get; set; }
    }

    public class Graphics
    {
        public Graphics(string width, int row, int column, string text)
        {
            this.width = width;
            this.row = row;
            this.column = column;
            this.text = text;
        }

        public string width { get; set; }
        public int row { get; set; }
        public int column { get; set; }
        public string text { get; set; }
    }

    public class Binding
    {
        public Binding(object[] on_tap)
        {
            this.on_tap = on_tap;
        }

        public object[] on_tap { get; set; }
    }

    public class Matrix
    {
        public Matrix(int row, int column)
        {
            this.row = row;
            this.column = column;
        }

        public int row { get; set; }
        public int column { get; set; }
    }
    List<Rootobject> JsonInfo = new List<Rootobject>(1);  //List Contains Json Info


    */




    /// <summary>
    /// Interaction logic for Page4.xaml
    /// </summary>
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

        private void keyRect_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Rectangle rec = (Rectangle)sender;
            DataObject dataObj = new DataObject(rec);
            DragDrop.DoDragDrop(rec, dataObj, DragDropEffects.Move);
            // Console.WriteLine(Grid.GetColumn(rec));
            //Console.WriteLine(Grid.GetRow(rec));
      


        }




        private void createJson_Click(object sender, RoutedEventArgs e)
        {
            CreateLayout createLayout = new CreateLayout();
            createLayout.CreateNewLayout(layoutNameTextbox.Text);
            //JsonInfo.Add(new Rootobject(key: "key1", width: "1u", row: 0, column: 0, text: "", on_tap: null , Matrixrow: 1 , Matrixcolumn: 1 ));

            JavaScriptSerializer ser = new JavaScriptSerializer(); // Serializer 
            //string output = ser.Serialize(JsonInfo);                 //Serialize the List and add to output string
            string KeyboardLayoutName = layoutNameTextbox.Text;     //Layout Name
         //   System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"\" + KeyboardLayoutName + ".json", output); //Save file
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
