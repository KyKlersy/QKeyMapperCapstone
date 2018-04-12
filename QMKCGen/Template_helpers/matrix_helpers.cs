using QKeyCommon.Keyboard_items;
using QKeyCommon.Keyboard_items.Key_items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMKCGen.Template_helpers
{
    class matrix_helpers
    {
        public static Key[,] fill_key_matrix(Keyboard keeb)
        {
            Key[,] key_matrix = new Key[keeb.spec.matrix_spec.rows, keeb.spec.matrix_spec.cols];
            var pin_map = pins_to_index(keeb);
            foreach (var key in keeb.keys)
            {
                key_matrix[pin_map[key.matrix.row], pin_map[key.matrix.col]] = key;
            }
            return key_matrix;
        }

        public static Dictionary<string, int> pins_to_index(Keyboard keeb)
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

        public static string user_friendly(Keyboard keeb)
        {
            string result = "";
            Key[,] key_matrix = fill_key_matrix(keeb);

            for(int i = 0; i < key_matrix.GetLength(0); i++) //row
            {
                result += "    "; //indentation to prettify
                for (int j = 0; j < key_matrix.GetLength(1); j++) //col
                {
                    if (key_matrix[i, j] != null)
                        result += "k" + i.ToString() + j.ToString();
                    if (!(i == key_matrix.GetLength(0) - 1  && j == key_matrix.GetLength(1) - 1))
                        result += ", ";
                }
                result += " \\";
                if (i != key_matrix.GetLength(0) - 1)
                    result += "\n";
            }

            return result;
        }

        public static string with_kc_no(Keyboard keeb)
        {
            string result = "";
            Key[,] key_matrix = fill_key_matrix(keeb);

            for (int i = 0; i < key_matrix.GetLength(0); i++) //row
            {
                result += "    {";
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

        public static string keymap(Keyboard keeb)
        {
            string result = "";
            Key[,] key_matrix = fill_key_matrix(keeb);
            for (int i = 0; i < key_matrix.GetLength(0); i++) //row
            {
                result += "    "; //indentation to prettify
                for (int j = 0; j < key_matrix.GetLength(1); j++) //col
                {
                    if (key_matrix[i, j] != null)
                        result += "KC_" + key_matrix[i, j].binding.on_tap[0].ToUpper(); //only handling single key binding atm
                    if (!(i == key_matrix.GetLength(0) - 1 && j == key_matrix.GetLength(1) - 1))
                        result += ", ";
                }
                result += " \\";
                if (i != key_matrix.GetLength(0) - 1)
                    result += "\n";
            }
            return result;
        }
    }
}
