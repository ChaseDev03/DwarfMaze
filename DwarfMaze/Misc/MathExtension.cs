using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DwarfMaze.Misc
{
    public static class MathExtension
    {

        #region Mathematical Funcitions

        public static double DegreesToRadians(double degrees)
        {
            return Math.PI / 180 * degrees;
        }

        #endregion
    }
}
