using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DwarfMaze.Objects
{
    class Start : Object
    {

        #region Class Methods

        public Start(ObjectType objType)
        {
            objectType = objType;
        }

        #endregion

        #region Duplication Methods

        public override Object DuplicateObject()
        {
            Start newObj = new Start(objectType);

            newObj.hasCollision = hasCollision;
            newObj.character = character;

            return newObj;
        }

        #endregion

        #region Render Methods

        //Change color, then render
        public override void DrawObject()
        {
            Console.ForegroundColor = ConsoleColor.Green;

            base.DrawObject();

            Console.ResetColor();
        }

        #endregion
    }
}
