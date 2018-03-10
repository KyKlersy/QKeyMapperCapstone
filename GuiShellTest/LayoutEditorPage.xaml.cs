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
using System.Windows.Navigation;
using System.Windows.Shapes;

public class JsonInfo {
    private string keyname;
    private string text;
    private string width;
    private string row;
    private string column;
    private string on_tap;

    public JsonInfo(string keyname, string text, string width, string row, string column, string on_tap)
    {
        this.keyname = keyname;
        this.text = text;
        this.width = width;
        this.row = row;
        this.column = column;
        this.on_tap = on_tap;
    }

    public string Width { get => width; set => width = value; }
    public string Row { get => row; set => row = value; }
    public string Column { get => column; set => column = value; }
    public string Text { get => text; set => text = value; }
    public string On_tap { get => on_tap; set => on_tap = value; }

    public override string ToString() => String.Format(        // Same as Json format
"{\"" + keyname + "\":{" + System.Environment.NewLine +

"\"" + "graphics" + "\":{" + System.Environment.NewLine +

"\"" + "width" + "\":" + width + "," + Environment.NewLine +
"\"" + "row" + "\":" + row + "," + Environment.NewLine +
"\"" + "column" + "\":" + column + "," + Environment.NewLine +
"\"" + "text" + "\":" + "\"" + text + "\"" + "}," + Environment.NewLine +

"\"" + "binding" + "\":" + Environment.NewLine +
"\"" + "on_tap" + "\":" + "[" + on_tap + "] }," + Environment.NewLine +

"\"" + "matrix" + "\":" + Environment.NewLine +
"\"" + "row" + "\":" + row + "," + Environment.NewLine +
"\"" + "column" + "\":" + column + "} };" + Environment.NewLine);
}



namespace QKeyMapper
{

    /// <summary>
    /// Interaction logic for Page4.xaml
    /// </summary>
    public partial class LayoutEditorPage : Page
    {
        List<JsonInfo> info = new List<JsonInfo>(1);   //Create List for JsonInfo class
        

        public LayoutEditorPage()
        {
            InitializeComponent();

            BrushConverter bc = new BrushConverter();

            for(int i = 0; i < 10; i++)
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
           
            

        }

        private void createJson_Click(object sender, RoutedEventArgs e)
        {
            JsonInfo x = new JsonInfo("sad1", "2we", "3", "4", "w5", "e6");
            info.Add(x);
            // File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"\" + layoutNameTextbox.Text +".json", info.ToString());

            //System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"\" + layoutNameTextbox.Text + ".json", info.ToString());
            System.Console.WriteLine(AppDomain.CurrentDomain.BaseDirectory);
            
           // System.Console.WriteLine(x);
            info.ForEach(Console.WriteLine);


            //TO BE CONTINUE

            MessageBox.Show("Object is written to json file");

        }

     
    }
}
