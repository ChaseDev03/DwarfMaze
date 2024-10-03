using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DwarfMaze.Objects
{
    class YellowSpace : Object
    {
        #region Class Methods

        public YellowSpace(ObjectType objType)
        {
            objectType = objType;
        }

        #endregion

        #region Duplication Methods

        //Returns itself because it only holds collision information
        public override Object DuplicateObject()
        {
            return this;
        }

        #endregion

        #region Render Methods

        //Change color, then render
        public override void DrawObject()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;

            base.DrawObject();

            Console.ResetColor();
        }

        #endregion
    }
}
