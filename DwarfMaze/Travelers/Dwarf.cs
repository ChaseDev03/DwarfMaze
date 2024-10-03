using DwarfMaze.Pathfinding;
using DwarfMaze.Movement;
using System;
using System.Collections.Generic;

namespace DwarfMaze.Travelers
{
    public class Dwarf : ITraveler, IAStar
    {
        #region Initializations

        public void InitializeAStarSeeker(Maze maz)
        {
            aStarSeeker = new AStarSeeker(MazeHandler.GetMaze());
        }

        #endregion

        #region Declarations

        private AStarSeeker aStarSeeker;

        private Point position;

        private ConsoleColor travelerColor = ConsoleColor.Cyan;

        private string travelerName = "Sebevědomý trpaslík";
        private string rotationASCII = "→↓←↑";

        private int currentASCII;
        private int pathStep;

        private List<Point> pathWay;


        public AStarSeeker AStarSeeker { get => aStarSeeker; set => aStarSeeker = value; }

        public Point Position { get => position; set => position = value; }

        public ConsoleColor TravelerColor { get => travelerColor; set => travelerColor = value; }

        public string TravelerName { get => travelerName; set => travelerName = value; }
        public string RotationASCII { get => rotationASCII; set => rotationASCII = value; }

        public bool Changed { get; private set; }

        public bool ReachedEnd { get; private set; }

        #endregion

        #region Class Methods

        public Dwarf(Point pos)
        {
            position = pos;
            currentASCII = rotationASCII[0];
        }

        #endregion

        #region Movement Methods

        public void Step()
        {
            if (ReachedEnd) return;

            GetPathToEnd();
            MoveTowardsEnd();
        }

        //Finds path to the end point using A*
        private void GetPathToEnd()
        {
            if (pathWay == null)
            {
                pathWay = aStarSeeker.FindPath(MazeHandler.GetStartPosition(), MazeHandler.GetEndPosition());
            }
        }

        //Move closer to end by each frame until you're there
        private void MoveTowardsEnd()
        {
            if (pathStep != pathWay.Count - 1)
            {
                pathStep++;

                Point toPoint = pathWay[pathStep];
                Point fromPoint = pathWay[pathStep - 1];

                currentASCII = GetASCIICharacterFromPoints(toPoint, fromPoint);
                position = toPoint;
            }
            else
            {
                ReachedEnd = true;
            }

            Changed = true;
        }
        #endregion

        #region Render Methods

        //Reset Changed and if not in end and should be rendered, render
        public bool DrawTraveler(bool draw)
        {
            Changed = false;

            if (ReachedEnd || !draw) return false;

            Console.ForegroundColor = travelerColor;

            Console.Write(rotationASCII[currentASCII]);

            Console.ResetColor();

            return true;
        }

        //Show only if has changed
        //Clears the line, shows messagee
        public int ShowTravelerMessage(int placeInOrder, int yLine)
        {
            if (!Changed) return 1;

            //Clear line
            Console.Write(new string(' ', 100));
            Console.SetCursorPosition(0, yLine);

            Console.ForegroundColor = travelerColor;

            if (!ReachedEnd)
            {
                Console.WriteLine(placeInOrder + ". " + travelerName + " je na pozici " + position.x + ", " + position.y);

                Console.ResetColor();
                return 1;
            }

            Console.WriteLine(placeInOrder + ". " + travelerName + " je v CÍLi na pozici " + position.x + ", " + position.y);

            Console.ResetColor();

            return 1;
        }

        //Manual way to change the way the dwarf looks
        private int GetASCIICharacterFromPoints(Point toPoint, Point fromPoint)
        {
            if (toPoint.x > fromPoint.x)
            {
                return 0;

            }
            else if (toPoint.x < fromPoint.x)
            {
                return 2;
            }
            else if (toPoint.y > fromPoint.y)
            {
                return 1;
            }
            else return 3;
        }

        #endregion
    }
}
