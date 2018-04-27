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
    /// Interaction logic for MacroEditorPage.xaml
    /// </summary>
    public partial class MacroEditorPage : Page
    {
        private Stopwatch sTimer = new Stopwatch();
        private List<Key> keyDownSet = new List<Key>();
        private List<Key> keyUpSet = new List<Key>();

        private List<string> keypressOrder = new List<string>();

        private HashSet<Key> modifierKeys = new HashSet<Key>();
        private string lastSeen = "";
        public MacroEditorPage()
        {
            InitializeComponent();
            //InitialiseModifierKeyList();
        }






        //private void startRecordingKeyMacro(object sender, RoutedEventArgs e)
        //{

        //    this.KeyDown += new KeyEventHandler(recordKeyDown);
        //    this.KeyUp += new KeyEventHandler(recordKeyUp);
        //    sTimer.Start();

        //}

        //void recordKeyUp(object sender, KeyEventArgs e)
        //{
        //    Debug.WriteLine("Elapsed ms: " + sTimer.ElapsedMilliseconds);
        //    if ( lastSeen !=e.Key.ToString() || (lastSeen == e.Key.ToString() && sTimer.ElapsedMilliseconds > 100d ))
        //    {

        //        lastSeen = e.Key.ToString();
        //        keypressOrder.Add("-" + lastSeen);
        //        keyUpSet.Add(e.Key);
        //        sTimer.Reset();
        //        sTimer.Start();
        //        Debug.WriteLine("KeyPressed: " + e.Key);
        //    }

        //    if(sTimer.ElapsedMilliseconds > 600d)
        //    {
        //        lastSeen = "";
        //    }


        //}

        //void recordKeyDown(object sender, KeyEventArgs e)
        //{
        //    Debug.WriteLine("Elapsed ms: " + sTimer.ElapsedMilliseconds);
        //    //if (lastSeen != e.Key.ToString() || (lastSeen == e.Key.ToString() && sTimer.ElapsedMilliseconds > 100d))
        //    //{
        //    //    Debug.WriteLine(e.KeyStates);
        //    //    lastSeen = e.Key.ToString();
        //    //    keyDownSet.Add(e.Key);
        //    //    sTimer.Reset();
        //    //    sTimer.Start();
        //    //    Debug.WriteLine("KeyPressed: " + e.Key);
        //    //}

        //    if(lastSeen != e.Key.ToString() || (lastSeen == e.Key.ToString() && sTimer.ElapsedMilliseconds > 500d))
        //    {
        //        lastSeen = e.Key.ToString();
        //        keypressOrder.Add("+" + lastSeen);
        //        keyDownSet.Add(e.Key);
        //        sTimer.Reset();
        //        sTimer.Start();
        //    }

        //    if (sTimer.ElapsedMilliseconds > 600d)
        //    {
        //        lastSeen = "";
        //    }


        //}

        //private string generateMacro()
        //{
        //    Debug.WriteLine("Macro::");
        //    string macroString = "";
        //    int loopCount = (keyDownSet.Count - 1);
        //    for (int i = 0; i <= loopCount; i++)
        //    {
        //        Debug.WriteLine(keyDownSet[i].ToString());//+ " " +i+ " " + (keyDownSet.Count - 1));
        //        //if (i < loopCount && notDuplicateModifierKey(keyDownSet[i], keyDownSet[i + 1]))
        //        //{
        //        //    macroString += " 10ms " + keyDownSet[i];
        //        //}
        //        //else if( i == loopCount)
        //        //{
        //        //    macroString += " 10ms " + keyDownSet[i];
        //        //}

        //    };
        //    Debug.WriteLine("Keyup events");
        //    for(int l = 0; l < (keyUpSet.Count -1); l++)
        //    {
        //        Debug.WriteLine(keyDownSet[l].ToString());

        //    }

        //    Debug.WriteLine("Press order");
        //    keypressOrder.ForEach(p =>
        //    {
        //        Debug.WriteLine("Press" + p);
        //    });


        //    return macroString;
        //}

        //private bool notDuplicateModifierKey(Key keyPress1, Key keyPress2)
        //{

        //    if(_isModifierKey(keyPress1) && _isModifierKey(keyPress2))
        //    {
        //        return (keyPress1.Equals(keyPress2)) ? false : true;
        //    }

        //    return true;
            

        //}

        //private bool _isModifierKey(Key type)
        //{
        //    return modifierKeys.Contains(type);
        //}

        //private void stopRecordingKeyMacro(object sender, RoutedEventArgs e)
        //{
        //    string macro = "";
        //    this.KeyDown -= recordKeyDown;
        //    this.KeyUp -= recordKeyUp;
        //    sTimer.Stop();
        //    //keyDownSet.ForEach(kt => { Debug.WriteLine(kt.ToString()); });
        //    macro = generateMacro();
        //    Debug.WriteLine("Macro generated: " + macro);
        //}

        //private void InitialiseModifierKeyList()
        //{
        //    modifierKeys.Add(Key.LeftAlt);
        //    modifierKeys.Add(Key.LeftCtrl);
        //    modifierKeys.Add(Key.LeftShift);
        //    modifierKeys.Add(Key.RightAlt);
        //    modifierKeys.Add(Key.RightCtrl);
        //    modifierKeys.Add(Key.RightShift);
        //    modifierKeys.Add(Key.CapsLock);
        //}
    }
}
