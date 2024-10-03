using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DwarfMaze.Objects
{
    class Wall : Object
    {
        #region Class Methods

        public Wall(ObjectType objType)
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
    }
}
