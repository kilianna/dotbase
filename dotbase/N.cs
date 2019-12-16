using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotBase
{
    public static class N
    {

        public static double doubleParse(string text)
        {
            double result;
            text = text.Trim().Replace(" ", "");
            if (double.TryParse(text, out result)) return result;
            if (text.IndexOf('.') >= 0) return double.Parse(text.Replace('.', ','));
            return double.Parse(text.Replace(',', '.'));
        }

        public static bool doubleTryParse(string text, out double result)
        {
            text = text.Trim().Replace(" ", "");
            if (double.TryParse(text, out result)) return true;
            if (text.IndexOf('.') >= 0) text = text.Replace('.', ',');
            else if (text.IndexOf(',') >= 0) text = text.Replace(',', '.');
            return double.TryParse(text, out result);
        }

    }
}
