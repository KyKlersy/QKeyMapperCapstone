using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMKCGen.helpers
{
    /*
     * Helper class to declutter code and put meaningful replacements
     * in the stead of plain text
     */
    class Syntax
    {
        public static string indent(int amount)
        {
            return new string(' ', amount * 4);
        }

        public static string switch_case(int i)
        {
            return "case(" + i + "):";
        }

        public static string key_macro(int i)
        {
            return "M(" + i.ToString() + ")";
        }

        public static string key_mod_tap(string s)
        {
            return "MT(" + s + ")";
        }

        public static string key_down(string s)
        {
            return "D(" + s + ")";
        }

        public static string key_up(string s)
        {
            return "U(" + s + ")";
        }

        public static string key_tap(string s)
        {
            return "T(" + s + ")";
        }

        public static string key_wait(int i)
        {
            return "W(" + i + ")";
        }
        
        public static string macro(string s)
        {
            return "return MACRO(" + s + ", END);";
        }
    }
}
