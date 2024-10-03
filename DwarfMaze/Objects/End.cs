using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DwarfMaze.Objects
{
    class End : Object
    {
        #region Class Methods

        public End (ObjectType objType)
        {
            objectType = objType;
        }

        #endregion

        #region Duplication Methods

        //Returns itself because it only holds collision information
        public override Object DuplicateObject()
        {
            End newObj = new End(objectType);

            newObj.hasCollision = hasCollision;
            newObj.character = character;

            return newObj;
        }

        #endregion

        #region Render Methods

        //Change color, then render
        public override void DrawObject()
        {
            Console.ForegroundColor = ConsoleColor.Red;

            base.DrawObject();

            Console.ResetColor();
        }

        #endregion
    }
}
