using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using qk = QKeyCommon.Keyboard_items;

namespace GuiShellTest.Controls
{
    /// <summary>
    /// Interaction logic for KeyCapButton.xaml
    /// </summary>
    public partial class KeyCapButton : UserControl, INotifyPropertyChanged
    {
        private qk.Key_items.Key keyItem;
        public KeyCapButton()
        {
            InitializeComponent();

            keyItem = new qk.Key_items.Key();
            DataContext = this;

        }

        public string text {
            get
            {
               return keyItem.graphics.text;
            }

            set
            {
                keyItem.graphics.text = value;
                onPropertyRaised("text");
            }
        }

        public string matrixrow
        {
            get
            {
                return keyItem.matrix.row;
            }

            set
            {
                keyItem.matrix.row = value;
                onPropertyRaised("matrixrow");
            }
        }

        public string matrixcol
        {
            get
            {
                return keyItem.matrix.col;
            }

            set
            {
                keyItem.matrix.col = value;
                Debug.WriteLine("Prop update: " + keyItem.matrix.col);
                onPropertyRaised("matrixcol");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void onPropertyRaised(String propertyName)
        {
            //If the property on the view has changed, raise new event and construct new event args with the name of the property.
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


        }

    }

}
