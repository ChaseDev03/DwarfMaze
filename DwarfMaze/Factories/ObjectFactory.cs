using DwarfMaze.Objects;
using System;
using System.Collections.Generic;
using System.IO;

namespace DwarfMaze.Factories
{
    public static class ObjectFactory
    {
        #region Initializations

        //Creates an object (basically a factory) of each object as there's a DuplicateObject() method in Object that handles duplication differently each time.
        public static void InitializeObjectPrefabs()
        {
            objectPrefabs.Add(' ', CreateEmptySpace());
            objectPrefabs.Add('#', CreateWall());
            objectPrefabs.Add('S', CreateStart());
            objectPrefabs.Add('F', CreateEnd());
            objectPrefabs.Add('B', CreateBlueSpace());
            objectPrefabs.Add('Y', CreateYellowSpace());
        }

        #endregion

        #region Declarations

        private static Dictionary<char, Object> objectPrefabs = new Dictionary<char, Object>();

        #endregion

        #region Main Methods

        //Duplicates object
        public static Object CreateObject(char objID)
        {
            return objectPrefabs[objID].DuplicateObject();
        }

        #endregion

        #region Creation Methods

        //A maze wall. Its sole purpose is to be collidable
        private static Object CreateWall()
        {
            Wall result = new Wall(ObjectType.Wall);

            result.HasCollision = true;

            result.ChangeCharacter('#');

            return result;
        }

        //The starting point. Non-collidable
        private static Object CreateStart()
        {
            Start result = new Start(ObjectType.Start);

            result.HasCollision = false;
            
            result.ChangeCharacter('S');

            return result;
        }

        //The finish point. Non-collidable
        private static Object CreateEnd()
        {
            End result = new End(ObjectType.End);

            result.HasCollision = false;

            result.ChangeCharacter('F');

            return result;
        }

        //Empty space. Just so the Traveler (dwarf) knows he can move there
        private static Object CreateEmptySpace()
        {
            EmptySpace result = new EmptySpace(ObjectType.EmptySpace);

            result.HasCollision = false;

            result.ChangeCharacter(' ');

            return result;
        }

        //Colored empty space. Non-collidable. Used for A* pathfinding debugging. Good to keep in in case anything needs to be changed
        private static Object CreateBlueSpace()
        {
            BlueSpace result = new BlueSpace(ObjectType.EmptySpace);

            result.HasCollision = false;

            result.ChangeCharacter('#');

            return result;
        }

        //Colored empty space. Non-collidable. Used for A* pathfinding debugging. Good to keep in in case anything needs to be changed
        private static Object CreateYellowSpace()
        {
            YellowSpace result = new YellowSpace(ObjectType.EmptySpace);

            result.HasCollision = false;

            result.ChangeCharacter('#');

            return result;
        }

        #endregion
    }
}
