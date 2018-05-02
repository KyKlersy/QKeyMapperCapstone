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
    /*
     * A helper class that wraps all functions that deal with unrolling key macros
     * public methods here are mounted as handlebars helpers
     * private methods are are used by the public methods to accomplish a task
     */
    class macro_helpers
    {
        /*
         * Handlebars helper that generates the C code associated with key macros
         */
        public static string macros(Keyboard keeb)
        {
            string result = "";
            var keys_with_macros = keeb.keys.Where(key => key.binding.is_macro());
            int macro_index = 0;
            foreach (var key in keys_with_macros)
            {
                result += Syntax.indent(3) + Syntax.switch_case(macro_index) + "\n";
                result += Syntax.indent(4) + Syntax.macro(parse_macro(key.binding));
                macro_index++;
                if (key != keys_with_macros.Last())
                    result += "\n";
            }
            return result;
        }

        /*
         * Parses a wait time string returns a string
         * of C code that corresponds to that wait time
         * if the wait time is greater than 255, it is unrolled into several wait commands
         */
        private static string parse_time(string time_string)
        {
            string result = "";
            Regex time_regex = new Regex(@"[1-9][0-9]*ms");
            if (!time_regex.IsMatch(time_string))
            {
                string msg = "Wait time specification is invalid";
                Console.WriteLine(msg);
                throw new ArgumentException(msg);
            }
            int time = int.Parse(time_string.Substring(0, time_string.Count() - 2)); //strip "ms"
            if (time < 0)
            {
                throw new ArgumentException("No negative numbers in time markers");
            }

            while(time > 255)
            {
                result += Syntax.key_wait(255) + ", ";
                time -= 255;
            }
            if(time > 0)
            {
                result += Syntax.key_wait(time);
            }

            return result;
        }

        /*
         * Parses the macro contained in binding and returns a string
         * of C code that corresponds to that macro
         */ 
        private static string parse_macro(Binding binding)
        {
            string result = "";
            var time_regex = new Regex(@"[1-9][0-9]*ms");
            var keycode_dict = Self.get_dict_from_csv("QMKCGen.Resources.key_name_to_kc_mapping.csv");
            var kcdirection_or_time = tokenize_macro(binding);
            foreach (var elem in kcdirection_or_time)
            {
                switch (elem.Count())
                {
                    case 1:
                        if (time_regex.IsMatch(elem.First()))
                        {
                            result += parse_time(elem.First());
                        }
                        else
                        {
                            result += Syntax.key_tap(keycode_dict[elem.First()].Substring(3));
                        }
                        break;
                    case 2:
                        if (elem[0] == "+")
                        {
                            result += Syntax.key_down(keycode_dict[elem[1]].Substring(3));
                        }
                        else if (elem[0] == "-")
                        {
                            result += Syntax.key_up(keycode_dict[elem[1]].Substring(3));
                        }
                        else
                        {
                            string msg = "Characters other than + or - in front of a keycode is not allowed";
                            Console.WriteLine(msg);
                            throw new ArgumentException(msg);
                        }
                        break;
                    default:
                        throw new ArgumentException("invalid keycode combinations");
                }
                if (elem != kcdirection_or_time.Last()) { result += ", "; }
            }
            return result;
        }

        //tokenizes the macro string into atomic components
        //for example:
        //["+", " K", "10ms", "-", "K", "Q"] -> [["+", "K"], ["10ms"], ["-", "K"], ["Q"]]
        private static List<List<string>> tokenize_macro(Binding binding)
        {
            var keycode_dict = Self.get_dict_from_csv("QMKCGen.Resources.key_name_to_kc_mapping.csv");
            var time_regex = new Regex(@"[1-9][0-9]*ms");
            var kcdirection_or_time = new List<List<string>>();
            for (int i = 0; i < binding.on_tap.Count(); i++)
            {
                try
                {
                    if (time_regex.IsMatch(binding.on_tap[i]))//it's a time
                    {
                        var temp = new List<string>();
                        temp.Add(binding.on_tap[i]);
                        kcdirection_or_time.Add(temp);
                    }
                    else if(binding.on_tap[i] == "+" || binding.on_tap[i] == "-")
                    {
                        var temp = new List<string>();
                        temp.Add(binding.on_tap[i]);
                        if(!keycode_dict.ContainsKey(binding.on_tap[i + 1]))
                        {
                            throw new ArgumentException("invalid keycode combinations, '+'/'-' must be followed by a valid key code");
                        }
                        temp.Add(binding.on_tap[i + 1]);
                        kcdirection_or_time.Add(temp);
                        i++;
                    }
                    else
                    {
                        var temp = new List<string>();
                        temp.Add(binding.on_tap[i]);
                        kcdirection_or_time.Add(temp);
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
