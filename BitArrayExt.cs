using Gridcore.Win32;
using System;
using System.Collections;
using System.Linq;
using System.Reflection;

namespace Gridcore {
    public static class BitArrayExt {
        // получение массива интов
        private static readonly FieldInfo bitArrayMArrayField =
            typeof(BitArray).GetField("m_array", BindingFlags.NonPublic | BindingFlags.Instance);

        private static int[] GetArray(this BitArray self) {
            return (int[]) bitArrayMArrayField.GetValue(self);
        }
        
        public static bool BitwiseEquals(this BitArray lhs, BitArray rhs) {
            
            return lhs.GetArray().SequenceEqual(rhs.GetArray());
        }

        public static string ToKeyString(this BitArray self) {
            var value = "";
            var intValue = 0;

            foreach (bool key in self) {
                if (key && Enum.IsDefined(typeof(VK), intValue)) {
                    if (value != "")
                        value += " ";
                    value += (VK) intValue;
                }
                ++intValue;
            }

            return value;
        }
    }
}
