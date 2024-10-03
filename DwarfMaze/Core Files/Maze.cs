using DwarfMaze.Core_Files;
using DwarfMaze.Factories;
using System;
using System.Collections.Generic;

namespace DwarfMaze
{
    public class Maze
    {
        #region Initializations

        //When loading maze, some objects require extra work. This handles the methods get initialized
        public static void InitializeObjectModification()
        {
            objectModificationMethod.Add('S', ModifyStart);
            objectModificationMethod.Add('F', ModifyEnd);
        }

        //mData is loaded from the 'Maze.dat' file
        private void LoadMapData(string[] mData)
        {
            ChangeMapSize(mData);

            //Load map content
            for (int y = 0; y < mData.Length; y++)
            {
                string yRow = mData[y];

                for (int x = 0; x < yRow.Length; x++)
                {
                    char xChar = yRow[x];

                    //Get object for each maze tile
                    Object newObj = ObjectFactory.CreateObject(xChar);

                    //If further work has to be done on the object, call the necessary method. This is because some objects don't actually duplicate, but return themselves
                    //to save memory, because they're mostly static
                    if (objectModificationMethod.ContainsKey(xChar)) objectModificationMethod[xChar](this, newObj, new Point(x, y));

                    //Save object
                    mapData[x, y] = newObj;
                }
            }

            //Error handling
            if (!DoesStartExist()) ErrorHandler.ThrowError("The map you chose to load has no Start assigned to it.");
            else if (!DoesEndExist()) ErrorHandler.ThrowError("The map you chose to load has no End assigned to it.");
        }

        //Initializes maze size. This resets the map data as well
        private void ChangeMapSize(string[] mData)
        {
            if (mData.Length == 0) ErrorHandler.ThrowError("The map you chose to load has no data.");

            int x = mData[0].Length;
            int y = mData.Length;

            //If the map is not rectangular, throw error
            int i = 1;
            foreach (string row in mData)
            {
                if (row.Length != x) ErrorHandler.ThrowError($"The map you chose to load does not have a rectangular shape. Make sure row {i} is the same length as the other rows");
                i++;
            }

            //Initialize sizes
            mapSize = (x, y);
            mapData = new Object[x, y];
        }

        #endregion

        #region Declarations

        private static Dictionary<char, Action<Maze, Object, Point>> objectModificationMethod = new Dictionary<char, Action<Maze, Object, Point>> ();


        private Object[,] mapData;

        private List<Point> neighborIncrements;

        private (int x, int y) mapSize;
        

        private EndWrapper startHolder;
        private EndWrapper endHolder;


        public (int x, int y) MapSize { get => mapSize; }

        public Object[,] MapData { get => mapData; }

        #endregion

        #region Class Methods

        //Constructs the Maze object.
        public Maze(string[] data)
        {
            //Determines what's node/tile is considered a default node/tile neighbor
            //P - current tile
            //1 - default neighbor
            //010
            //1P1
            //010
            neighborIncrements = new List<Point>() { new Point(1, 0), new Point(-1, 0), new Point(0, 1), new Point(0, -1) };

            LoadMapData(data);
        }

        #endregion

        #region Getter Methods

        public Point GetStartPosition()
        {
            return startHolder.position;
        }

        public Point GetEndPosition()
        {
            return endHolder.position;
        }

        public bool DoesStartExist()
        {
            return startHolder.end != null;
        }

        public bool DoesEndExist()
        {
            return endHolder.end != null;
        }


        public bool IsPointNonNonCollidable(Point pos)
        {
            //Is point in bounds and is non-collidable?
            if (pos.x >= 0 && pos.x < mapData.GetLength(0) && pos.y >= 0 && pos.y < mapData.GetLength(1) && !mapData[pos.x, pos.y].HasCollision)
                return true;

            return false;
        }

        //Get non-collidable default neighbor positions around current position
        public List<Point> GetAdjacentNonCollidablePoints(Point pos)
        {
            List<Point> result = new List<Point>();

            foreach (Point p in neighborIncrements)
            {
                Point nP = pos + p;

                if (IsPointNonNonCollidable(nP)) result.Add(nP);
            }

            return result;
        }

        //Get non-collidable neighbor positions set by 'neighborIncrements' around current position
        public List<Point> GetAdjacentNonCollidablePoints(Point pos, List<Point> neighborIncrements)
        {
            List<Point> result = new List<Point>();

            foreach (Point p in neighborIncrements)
            {
                Point nP = pos + p;

                if (IsPointNonNonCollidable(nP)) result.Add(nP);
            }

            return result;
        }

        //Get non-collidable  neighbor directions set by 'neighborIncrements' around current position
        public HashSet<Point> GetAdjacentNonCollidableSides(Point pos, List<Point> neightIncrements)
        {
            HashSet<Point> result = new HashSet<Point>();
            List<Point> freePointAround = GetAdjacentNonCollidablePoints(pos, neightIncrements);

            foreach (Point side in neightIncrements)
            {
                if (freePointAround.Contains(side + pos))
                    result.Add(side);
            }

            return result;
        }


        #endregion

        #region Static - Object Modification - Methods

        //Save start and if there are two starts, throw error
        private static void ModifyStart(Maze maze, Object start, Point pos)
        {
            if (maze.startHolder.end != null)
                ErrorHandler.ThrowError("Only one start 'S' can be on map.\nPress enter to terminate program");

            maze.startHolder.end = start;
            maze.startHolder.position = pos;
        }

        //Save end and if there aer two ends, throw error
        private static void ModifyEnd(Maze maze, Object end, Point pos)
        {
            if (maze.endHolder.end != null)
                ErrorHandler.ThrowError("Only one finish 'F' can be on map.\nPress enter to terminate program");

            maze.endHolder.end = end;
            maze.endHolder.position = pos;
        }

        #endregion

        #region Structs

        //Holder for start and end
        private struct EndWrapper
        {
            #region Declarations

            public Point position;
            
            public Object end;

            #endregion
        }

        #endregion
    }
}
