﻿using QKeyCommon.Keyboard_items;
using QMKCGen.helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMKCGen
{
    class Validations
    {
        public static bool has_valid_key_codes(Keyboard keeb)
        {
            var keycode_dict = Self.get_dict_from_csv("QMKCGen.Resources.key_name_to_kc_mapping.csv");
            var keycode_modifiers_dict = Self.get_dict_from_csv("QMKCGen.Resources.key_name_to_kc_mapping_modifiers_only.csv");
            foreach (var key in keeb.keys)
            {
                foreach(var keycode in key.binding.on_tap)
                {
                    if(!keycode_dict.ContainsKey(keycode) && !(keycode == "+" || keycode == "-"))
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
    }
}
