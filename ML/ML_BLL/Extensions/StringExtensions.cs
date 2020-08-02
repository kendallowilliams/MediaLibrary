using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MediaLibraryBLL.Extensions
{
    public static class StringExtensions
    {
        public static bool Contains(this string str, string strToFind, StringComparison comparer)
        {
            return (str?.IndexOf(strToFind, comparer) ?? -1) != -1;
        }
    }
}
