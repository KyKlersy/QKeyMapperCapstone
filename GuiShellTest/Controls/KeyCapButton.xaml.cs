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
        private KeyMacro _onTapMacro;
        private KeyMacro _onHoldMacro;
        private string _onTapMacroName;
        private string _onHoldMacroName;

        public int onTapMacroListID { get; set; }
        public int onHoldMacroListID { get; set; }

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
            }
        }

        public string OnTapMacroName
        {
            get
            {
                return _onTapMacroName;
            }

            set
            {
                if (value != _onTapMacroName)
                {
                    _onTapMacroName = value;
                    onPropertyRaised(nameof(OnTapMacroName));
                    Debug.WriteLine("Selected Macro: " + _onTapMacroName);
                }
            }
        }


        public KeyMacro OnTapMacro
        {
            get
            {
                return _onTapMacro;
            }

            set
            {
                if (value != _onTapMacro)
                {
                    _onTapMacro = value;
                    OnTapMacroName = _onTapMacro.macroName;

                    keyItem.binding.on_tap = _onTapMacro.macroString;

                    onPropertyRaised(nameof(OnTapMacro));

                }
            }
        }
        
        public string OnHoldMacroName
        {
            get
            {
                return _onHoldMacroName;
            }

            set
            {
                if (value != _onHoldMacroName)
                {
                    _onHoldMacroName = value;
                    onPropertyRaised(nameof(OnHoldMacroName));
                    Debug.WriteLine("Selected Macro: " + _onHoldMacroName);
                }
            }
        }

        public KeyMacro OnHoldMacro
        {
            get
            {
                return _onHoldMacro;
            }

            set
            {
                if (value != _onHoldMacro)
                {
                    _onHoldMacro = value;
                    OnHoldMacroName = _onHoldMacro.macroName;

                    keyItem.binding.on_hold = _onHoldMacro.macroString;

                    onPropertyRaised(nameof(OnHoldMacro));

                }
            }
        }

        public void setKeyHoldMacro(List<string> macrostring)
        {
            keyItem.binding.on_hold = macrostring;
        }

        public void setKeyTapMacro(List<string> macrostring)
        {
            keyItem.binding.on_tap = macrostring;
        }

        public qk.Key_items.Key getKeyItem()
        {
            return keyItem;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void onPropertyRaised(String propertyName)
        {
            //If the property on the view has changed, raise new event and construct new event args with the name of the property.
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


        }

    }

}
