using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProvisGames.PolygonClipper
{
    internal class UMath
    {
        public static bool IsZero(double value)
        {
            return Math.Abs(value) < double.Epsilon;
        }
    }
}
