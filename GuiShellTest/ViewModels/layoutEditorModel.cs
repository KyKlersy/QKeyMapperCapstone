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

        private string _productName;

        public layoutEditorModel()
        {
            SupportedPins = new List<MatrixPin>();

            SupportedDiodeDirections = new List<DiodeDirection>()
            {
                new DiodeDirection("Column to Row", "COL2ROW"),
                new DiodeDirection("Row to Column", "ROW2COL"),
                new DiodeDirection("None", "")
            };

            loadSupportedPins();                    

        }

        public string layoutname
        {
            get
            {
                return _productName;
            }

            set
            {
                _productName = value;
                Debug.WriteLine("Layoutname: " + _productName);
                onPropertyRaised(nameof(layoutname));
            }
        }

        public DiodeDirection SelectedDiodeDirection
        {
            get
            {
                return _selectedDiodeDirection;
            }

            set
            {
                if(value != _selectedDiodeDirection)
                {
                    _selectedDiodeDirection = value;
                    onPropertyRaised(nameof(SelectedDiodeDirection));
                    Debug.WriteLine("Diode Direction: " + _selectedDiodeDirection);
                }
               
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void onPropertyRaised(String propertyName)
        {
            //If the property on the view has changed, raise new event and construct new event args with the name of the property.
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


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

    }
}
