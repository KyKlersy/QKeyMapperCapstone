using QKeyCommon.Keyboard_items;
using QKeyCommon.Keyboard_items.Key_items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QMKCGen.helpers
{
    class macro_helpers
    {
        public static string macros(Keyboard keeb)
        {
            string result = "";
            var keys_with_macros = keeb.keys.Where(key => key.binding.is_macro());
            int macro_index = 0;
            foreach (var key in keys_with_macros)
            {
                result += Syntax.indent(3) + "case(" + macro_index + "):\n";
                result += Syntax.indent(4) + "return MACRO(" + parseMacro(key.binding) + ", END);\n";
                macro_index++;
            }
            return result;
        }

        private static string parseMacro(Binding binding)
        {
            string result = "";
            Dictionary<string, string> keycode_dict = Self.get_dict_from_csv("QMKCGen.Resources.key_name_to_kc_mapping.csv");
            List<List<string>> kcdirection_or_time = tokenizeMacro(binding);
            foreach (var elem in kcdirection_or_time)
            {
                switch (elem.Count())
                {
                    case 1:
                        result += "W(" + elem[0].Substring(0, elem[0].Length - 2) + ")";
                        break;
                    case 2:
                        if (elem[0] == "+")
                        {
                            result += "D(";
                        }
                        else if (elem[0] == "-")
                        {
                            result += "U(";
                        }
                        else
                        {
                            string msg = "Characters other than + or - in front of a keycode is not allowed";
                            Console.WriteLine(msg);
                            throw new ArgumentException(msg);
                        }
                        result += keycode_dict[elem[1]].Substring(3) + ")";
                        break;
                    default:
                        throw new ArgumentException("invalid keycode combinations");
                }
                if (elem != kcdirection_or_time.Last()) { result += ", "; }
            }
            return result;
        }

        //parses the macro string into atomic components
        //for example:
        //["+", " K", "10ms", "-", "K"] -> [["+", "K"], ["10ms"], ["-", "K"]]
        private static List<List<string>> tokenizeMacro(Binding binding)
        {
            Regex time_regex = new Regex(@"[1-9][0-9]*ms");
            List<List<string>> kcdirection_or_time = new List<List<string>>();
            for (int i = 0; i < binding.on_tap.Count(); i++)
            {
                try
                {
                    if (time_regex.IsMatch(binding.on_tap[i]))//it's a time
                    {
                        List<string> temp = new List<string>();
                        temp.Add(binding.on_tap[i]);
                        kcdirection_or_time.Add(temp);
                    }
                    else
                    {
                        List<string> temp = new List<string>();
                        temp.Add(binding.on_tap[i]);
                        temp.Add(binding.on_tap[i + 1]);
                        kcdirection_or_time.Add(temp);
                        i++;
                    }
                }
                catch
                {
                    Console.WriteLine("One of the key macros is ill-formed");
                    throw;
                }
            }
            return kcdirection_or_time;
        }
    }
}
