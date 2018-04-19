using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using qk = QKeyCommon.Keyboard_items;
namespace StubRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            DeserialzeJson dj = new DeserialzeJson();
            qk.Keyboard kb;

            kb = dj.getKeyboardDeserialization();

            foreach(var key in kb.keys)
            {
                Debug.WriteLine("Key matrix pins: Row: " + key.matrix.row + " Col: " + key.matrix.col);
            }
            
        }
    }
}
