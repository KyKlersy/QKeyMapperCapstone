using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace QKeyMapper
{
    /// <summary>
    /// Interaction logic for Page4.xaml
    /// </summary>
    public partial class LayoutEditorPage : Page
    {
        public LayoutEditorPage()
        {
            InitializeComponent();

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
                visualEditorGrid.Children.Add(rbd);*/

                for (int j = 0; j < 10; j++)
                {

                    dropBorder = dropableBorder(i,j,bc);


                    Grid.SetRow(dropBorder, i);
                    Grid.SetColumn(dropBorder, j);
                    Debug.WriteLine("Row: " + i + "Col: " + j);
                    visualEditorGrid.Children.Add(dropBorder);
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
    }
}
