using System;

namespace DwarfMaze.Objects
{
    class BlueSpace : Object
    {
        #region Class Methods

        public BlueSpace(ObjectType objType)
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
            Console.ForegroundColor = ConsoleColor.Blue;

            base.DrawObject();

            Console.ResetColor();
        }

        #endregion
    }
}
