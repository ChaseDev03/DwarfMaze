using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DwarfMaze
{
    public class Object
    {
        #region Initializations

        public virtual void ChangeCharacter(char newChar)
        {
            character = newChar;
        }

        #endregion

        #region Declarations

        protected ObjectType objectType;

        protected bool hasCollision;

        protected char character;


        public virtual ObjectType ObjectType { get => objectType; }

        public bool HasCollision { get => hasCollision; set => hasCollision = value; }

        public char Character { get => character; }

        #endregion

        #region Duplication Methods

        //Copy object with default settings
        public virtual Object DuplicateObject()
        {
            Object newObj = new Object();

            newObj.objectType = objectType;
            newObj.hasCollision = hasCollision;
            newObj.character = character;

            return newObj;
        }

        #endregion

        #region Render Methods

        public virtual void DrawObject()
        {
            Console.Write(character);
        }

        #endregion
    }
}
