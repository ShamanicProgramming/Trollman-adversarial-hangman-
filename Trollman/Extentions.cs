using System;
using System.Collections.Generic;

namespace Trollman
{
    internal static class Extentions
    {
        public static List<int> AllIndexesOf(this string str, char value)
        {
            List<int> indexes = new List<int>();
            for (int index = 0; ; index += 1)
            {
                index = str.IndexOf(value, index);
                if (index == -1)
                    return indexes;
                indexes.Add(index);
            }
        }
    }
}
