using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotBase
{
    class SqlQueryUtils
    {
        public static String normalize(String input)
        {
            return input.Replace(",", ".");
        }
    }
}
