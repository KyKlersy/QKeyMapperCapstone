using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using qk = QKeyCommon.Keyboard_items;

namespace GuiShellTest.Models
{
    class layoutEditorModel : INotifyPropertyChanged
    {
        private qk.Keyboard keyboard;

        public layoutEditorModel()
        {
            keyboard = new qk.Keyboard();
        }

        public string layoutname
        {
            get
            {
                return keyboard.desc.product_name;
            }

            set
            {
                keyboard.desc.product_name = value;
                Debug.WriteLine("Layoutname: " + keyboard.desc.product_name);
                onPropertyRaised("layoutname");
            }
        }

        public string diodeDirection
        {
            get
            {
                return keyboard.spec.diode_direction;
            }

            set
            {
                keyboard.spec.diode_direction = value;
                Debug.WriteLine("Diode direction selected: " + keyboard.spec.diode_direction);
                onPropertyRaised("diodeDirection");
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
