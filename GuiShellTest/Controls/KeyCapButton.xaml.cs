using GuiShellTest.Models;
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
        private MatrixPin _selectedRowMatrixPin;
        private MatrixPin _selectedColMatrixPin;

        public KeyCapButton()
        {
            InitializeComponent();

            keyItem = new qk.Key_items.Key();
            DataContext = this;
        }

        public KeyCapButton(qk.Key_items.Key key)
        {
            InitializeComponent();
            keyItem = key;
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

        public MatrixPin matrixrow
        {
            get
            {
                return _selectedRowMatrixPin;
            }

            set
            {
                if (value != _selectedRowMatrixPin)
                {
                    _selectedRowMatrixPin = value;
                    onPropertyRaised(nameof(matrixrow));
                    keyItem.matrix.row = _selectedRowMatrixPin.pinName;
                }

                //    _selectedRowMatrixPin = value;
                //onPropertyRaised("matrixrow");
            }
        }

        public MatrixPin matrixcol
        {
            get
            {
                return _selectedColMatrixPin;
            }

            set
            {

                if (value != _selectedColMatrixPin)
                {
                    _selectedColMatrixPin = value;
                    onPropertyRaised(nameof(matrixcol));
                    keyItem.matrix.col = _selectedColMatrixPin.pinName;
                }

                //keyItem.matrix.col = value;
                //Debug.WriteLine("Prop update: " + keyItem.matrix.col);
                //onPropertyRaised("matrixcol");
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
