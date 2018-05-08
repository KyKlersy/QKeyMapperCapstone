using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using qk = QKeyCommon.Keyboard_items;
using GuiShellTest.Models;
using System.Reflection;
using System.IO;

namespace GuiShellTest.ViewModels
{
    public class layoutEditorModel : INotifyPropertyChanged
    {
        public List<DiodeDirection> SupportedDiodeDirections { get; set; }
        private DiodeDirection _selectedDiodeDirection;

        public List<MatrixPin> SupportedPins { get; set; }
        private MatrixPin _selectedMatrixPin;
        private MatrixPin _selectedKeyCapPin;

        private string rowMatrixPins = "";
        private string colMatrixPins = "";

        public List<MatrixPin> SelectedKeyboardRowPins { get; set; }
        public List<MatrixPin> SelectedKeyboardColPins { get; set; }

        private string _productName;

        public layoutEditorModel()
        {
            SupportedPins = new List<MatrixPin>();
            SelectedKeyboardRowPins = new List<MatrixPin>();
            SelectedKeyboardColPins = new List<MatrixPin>();

            SupportedDiodeDirections = new List<DiodeDirection>()
            {
                new DiodeDirection("Column to Row", "COL2ROW"),
                new DiodeDirection("Row to Column", "ROW2COL"),
                new DiodeDirection("None", "")
            };

            loadSupportedPins();

        }

        /* Public binding property for layout name in layouteditor page */
        public string layoutname
        {
            get
            {
                return _productName;
            }

            set
            {
                _productName = value;

                onPropertyRaised(nameof(layoutname));
            }
        }

        /* Public binding property for layout editor page selected diode direction combobox item */
        public DiodeDirection SelectedDiodeDirection
        {
            get
            {
                return _selectedDiodeDirection;
            }

            set
            {
                if (value != _selectedDiodeDirection)
                {
                    _selectedDiodeDirection = value;
                    onPropertyRaised(nameof(SelectedDiodeDirection));

                }

            }
        }

        /* Public binding property for showing the selected keyboard matrix row */
        public string KeyboardMatrixRow
        {
            get
            {
                return rowMatrixPins;

            }
            set
            {
                string pins = "";
                SelectedKeyboardRowPins.ForEach(p =>
                {
                    pins += p.pinName + ",";
                });

                pins.TrimEnd(',');

                rowMatrixPins = pins;
                onPropertyRaised(nameof(KeyboardMatrixRow));
            }
        }

        /* Public binding property for selected matrix pin object from combobox selection */
        public MatrixPin SelectedMatrixPin
        {
            get
            {
                return _selectedMatrixPin;
            }
            set
            {
                if (value != _selectedMatrixPin)
                {
                    _selectedMatrixPin = value;

                    onPropertyRaised(nameof(SelectedMatrixPin));

                }
            }
        }

        public MatrixPin SelectedKeyCapPin
        {
            get
            {
                return _selectedKeyCapPin;
            }
            set
            {
                if (value != _selectedKeyCapPin)
                {
                    _selectedKeyCapPin = value;

                    onPropertyRaised(nameof(SelectedKeyCapPin));

                }
            }
        }

        /* Public binding property for showing the string representation of the keyboard matrix col */
        public string KeyboardMatrixCol
        {
            get
            {
                return colMatrixPins;

            }
            set
            {
                string pins = "";
                SelectedKeyboardColPins.ForEach(p =>
                {
                    pins += p.pinName + ",";
                });

                pins.TrimEnd(',');

                colMatrixPins = pins;
                onPropertyRaised(nameof(KeyboardMatrixCol));
            }
        }


        /* Event changed handler for updating property binding on change. */
        public event PropertyChangedEventHandler PropertyChanged;
        private void onPropertyRaised(String propertyName)
        {
            //If the property on the view has changed, raise new event and construct new event args with the name of the property.
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /* Loads the supported pins into the combo box data list used in the binding view. */
        private void loadSupportedPins()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "GuiShellTest.Resources.supportedPins.csv";
            MatrixPin mp;
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                while (!reader.EndOfStream)
                {
                    var readLine = reader.ReadLine();
                    var tokens = readLine.Split(',');
                    mp = new MatrixPin(tokens[0]);
                    SupportedPins.Add(mp);
                }
            }
        }

        public void reset()
        {

            _selectedDiodeDirection = null;


            _selectedMatrixPin = null;

            rowMatrixPins = "";
            colMatrixPins = "";

            SelectedKeyboardRowPins.Clear();
            SelectedKeyboardColPins.Clear();

            _productName = "";

        }
    }
}
