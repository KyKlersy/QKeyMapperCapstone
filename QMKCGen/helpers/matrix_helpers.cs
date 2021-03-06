﻿using QKeyCommon.Keyboard_items;
using QKeyCommon.Keyboard_items.Key_items;
using QMKCGen.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QMKCGen.helpers
{
    /*
     * A helper class that wraps all functions that deal with unrolling the keyboard matrix
     * public methods here are mounted as handlebars helpers
     * private methods are are used by the public methods to accomplish a task
     */
    class matrix_helpers
    {
        /*
         * Generates a 2D matrix of keys that corresponds directly to the keyboard matrix
         * Throws ArgumentException if a pin in Keyboard.keys is not contained in either
         * Keyboard.spec.matrix_spec.row_pins and Keyboard.spec.matrix_spec.col_pins
         */
        private static Key[,] fill_key_matrix(Keyboard keeb)
        {
            if (keeb.spec.matrix_spec.rows != keeb.spec.matrix_spec.row_pins.Count()
            || keeb.spec.matrix_spec.cols != keeb.spec.matrix_spec.col_pins.Count())
                throw new ArgumentException("The number of pins specified doesn't match the lists");
            Key[,] key_matrix = new Key[keeb.spec.matrix_spec.rows, keeb.spec.matrix_spec.cols];
            var pin_map = pins_to_index(keeb);
            foreach (var key in keeb.keys)
            {
                if (!pin_map.ContainsKey(key.matrix.col) ||
                   !pin_map.ContainsKey(key.matrix.row))
                {
                    throw new ArgumentException("That pin isn't in row_pins or col_pins");
                }
                key_matrix[pin_map[key.matrix.row], pin_map[key.matrix.col]] = key;
            }
            return key_matrix;
        }

        /*
         * Generates a map that converts pin names into their respective indices
         */
        private static Dictionary<string, int> pins_to_index(Keyboard keeb)
        {
            var row_pins = keeb.spec.matrix_spec.row_pins;
            var col_pins = keeb.spec.matrix_spec.col_pins;
            //make sure that row_pins and col_pins are all distinct to themselves and each other
            if (row_pins.Intersect(col_pins).Count() != 0
            ||  row_pins.Distinct().Count() != row_pins.Count()
            ||  col_pins.Distinct().Count() != col_pins.Count())
            {
                throw new ArgumentException("row_pins and col_pins are not distinct and/or mutually distinct");
            }

            var result = new Dictionary<string, int>();
            for(int i = 0; i < row_pins.Count(); i++)
            {
                result[row_pins[i]] = i;
            }
            for (int i = 0; i < col_pins.Count(); i++)
            {
                result[col_pins[i]] = i;
            }

            return result;
        }

        /*
         * Handlebars helper that generates the C code associated with
         * the keyboard matrix displayed in a user_friendly manner
         */
        public static string user_friendly(Keyboard keeb)
        {
            string result = "";
            Key[,] key_matrix = fill_key_matrix(keeb);

            for(int i = 0; i < key_matrix.GetLength(0); i++) //row
            {
                result += Syntax.indent(1); 
                for (int j = 0; j < key_matrix.GetLength(1); j++) //col
                {
                    if (key_matrix[i, j] != null)
                        result += "k" + i.ToString() + j.ToString();
                    if (!(i == key_matrix.GetLength(0) - 1  && j == key_matrix.GetLength(1) - 1))
                        result += ", ";
                }
                result += @" \";
                if (i != key_matrix.GetLength(0) - 1)
                    result += "\n";
            }

            return result;
        }

        /*
         * Handlebars helper that generates the C code associated with
         * the keyboard matrix displayed in the manner required with QMK
         */
        public static string with_kc_no(Keyboard keeb)
        {
            string result = "";
            Key[,] key_matrix = fill_key_matrix(keeb);

            for (int i = 0; i < key_matrix.GetLength(0); i++) //row
            {
                result += Syntax.indent(1) + "{";
                for (int j = 0; j < key_matrix.GetLength(1); j++) //col
                {
                    if (key_matrix[i, j] != null)
                        result += "k" + i.ToString() + j.ToString();
                    else
                        result += "KC_NO";

                    if (j != key_matrix.GetLength(1) - 1)
                        result += ", ";
                }
                result += "}, \\";
                if (i != key_matrix.GetLength(0) - 1)
                    result += "\n";
            }

            return result;
        }

        /*
         * Handlebars helper that generates the C code associated with
         * the keyboard matrix's corresponding keycodes and macros
         */
        public static string keymap(Keyboard keeb)
        {
            var keycode_dict = Self.get_dict_from_csv("QMKCGen.Resources.key_name_to_kc_mapping.csv");
            string result = "";
            Key[,] key_matrix = fill_key_matrix(keeb);
            int macro_index = 0;
            for (int i = 0; i < key_matrix.GetLength(0); i++) //row
            {
                result += Syntax.indent(1);
                for (int j = 0; j < key_matrix.GetLength(1); j++) //col
                {
                    if (key_matrix[i, j] != null)
                    {
                        if (key_matrix[i, j].binding.is_macro())
                        {
                            result += Syntax.key_macro(macro_index);
                            macro_index++;
                        }
                        else if (key_matrix[i, j].binding.is_multifunction())
                        {
                            //key is not a macro
                            result += Syntax.key_mod_tap(
                                unroll_on_hold(key_matrix[i, j].binding) + ", " +
                                keycode_dict[key_matrix[i, j].binding.on_tap[0]]
                            );
                        }
                        else if (key_matrix[i, j].binding.on_tap.Count() == 1)
                        {
                            result += keycode_dict[key_matrix[i, j].binding.on_tap[0]];
                        }
                        else
                        {
                            result += "KC_NO";
                        }
                    }
                    if (!(i == key_matrix.GetLength(0) - 1 && j == key_matrix.GetLength(1) - 1))
                        result += ", ";
                }
                result += " \\";
                if (i != key_matrix.GetLength(0) - 1)
                    result += "\n";
            }
            return result;
        }

        /*
         * Unrols a series of on_hold commands contained in Binding
         * and returns an '|' separated string of the commands
         * as required by QMK 
         * Throws ArgumentException if Binding.on_hold contains an invalid value
         */ 
        private static string unroll_on_hold(Binding binding)
        {
            var keycode_dict = Self.get_dict_from_csv("QMKCGen.Resources.supportedOnHoldMacroModifiers.csv");
            string result = "";
            foreach(var keycode in binding.on_hold)
            {
                if (!keycode_dict.ContainsKey(keycode))
                    throw new ArgumentException("Invalid keycodes in on_hold binding");
                result += keycode_dict[keycode];
                if (keycode != binding.on_hold.Last())
                    result += " | ";
            }
            return result;
        }
    }
}
