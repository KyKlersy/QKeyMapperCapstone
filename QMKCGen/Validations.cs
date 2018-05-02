using QKeyCommon.Keyboard_items;
using QMKCGen.helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QMKCGen
{
    class Validations
    {
        public static bool has_valid_key_codes(Keyboard keeb)
        {
            Regex time_regex = new Regex(@"[1-9][0-9]*ms");
            var keycode_dict = Self.get_dict_from_csv("QMKCGen.Resources.key_name_to_kc_mapping.csv");
            var keycode_modifiers_dict = Self.get_dict_from_csv("QMKCGen.Resources.key_name_to_kc_mapping_modifiers_only.csv");
            foreach (var key in keeb.keys)
            {
                foreach(var keycode in key.binding.on_tap)
                {
                    if(!keycode_dict.ContainsKey(keycode) && !(keycode == "+" || keycode == "-" || time_regex.IsMatch(keycode)))
                    {
                        return false;
                    }
                }
                foreach (var keycode in key.binding.on_hold)
                {
                    if(!keycode_modifiers_dict.ContainsKey(keycode))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static bool is_valid_filename(string subject)
        {
            if(string.IsNullOrEmpty(subject))
            {
                return false;
            }
            //if subject contains any invalid file name characters
            if(subject.Any(x => System.IO.Path.GetInvalidFileNameChars().Any(y => y == x)))
            {
                return false;
            }
            return true;
        }

        public static bool ensure_critical_values(Keyboard keeb)
        {
            try
            {
                if(keeb.desc.product_name == null)
                    return false;
                if(keeb.spec.matrix_spec.col_pins == null ||
                   keeb.spec.matrix_spec.row_pins == null)
                    return false;
                if(keeb.spec.avrdude.partno == null ||
                   keeb.spec.avrdude.partno_verbose == null)
                    return false;
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
