﻿using GuiShellTest.Models;
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
    /// This is a custom control for modeling the behavior of a key object with its data members tied to it.
    /// </summary>
    public partial class KeyCapButton : UserControl, INotifyPropertyChanged
    {
        private qk.Key_items.Key keyItem;
        private MatrixPin _selectedRowMatrixPin;
        private MatrixPin _selectedColMatrixPin;
        private string _selectedRowText;
        private string _selectedColText;

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

        /* Public binding property for keycap text */
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

        /* Public binding property for showing string representation of the pin selected */
        public string SelectedMatrixPinRow
        {
            get
            {
                return _selectedRowText;
            }
            set
            {
                if (value != _selectedRowText)
                {
                    _selectedRowText = value;

                    if(value != null)
                        keyItem.matrix.row = _selectedRowText;

                    Debug.WriteLine("Selected Matrix Row Pin: " + keyItem.matrix.row);
                    onPropertyRaised(nameof(SelectedMatrixPinRow));
                }
            }
        }

        /* Public binding property for showing string representation of the pin selected */
        public string  SelectedMatrixPinCol
        {
            get
            {
                return _selectedColText;
            }
            set
            {
                if (value != _selectedColText)
                {
                    _selectedColText = value;

                    if(value != null)
                        keyItem.matrix.col = _selectedColText;


                    onPropertyRaised(nameof(SelectedMatrixPinCol));
                }
            }
        }   

        /* Public binding property for showing the string representation of the macro selected. */
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
                }
            }
        }

        /* Public binding property for selected on tap macro object from selected combobox */
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
        
        /* Public binding property for displaying the selected on hold macro string */
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

                }
            }
        }

        /* Public binding property to hold the selected on hold macro object from combobox choice */
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

                    keyItem.binding.on_hold = new List<string>(_onHoldMacro.macroName.Split('\n'));

                    onPropertyRaised(nameof(OnHoldMacro));

                }
            }
        }

        /* Setter call for setting hold macro string from data object to keyboard json object */
        public void setKeyHoldMacro(List<string> macrostring)
        {
            keyItem.binding.on_hold = macrostring;
        }

        /* Setter call for setting tap macro strng from data object to json object */
        public void setKeyTapMacro(List<string> macrostring)
        {
            keyItem.binding.on_tap = macrostring;
        }

        public qk.Key_items.Key getKeyItem()
        {
            return keyItem;
        }


        /* Property changed event handler called using data binding */
        public event PropertyChangedEventHandler PropertyChanged;
        private void onPropertyRaised(String propertyName)
        {
            //If the property on the view has changed, raise new event and construct new event args with the name of the property.
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }

    }

}
