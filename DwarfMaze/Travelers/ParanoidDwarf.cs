using DwarfMaze.Misc;
using DwarfMaze.Movement;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DwarfMaze.Travelers
{
    internal class ParanoidDwarf : ITraveler
    {
        #region Declarations

        private Point position;

        private ConsoleColor travelerColor = ConsoleColor.Blue;

        private string travelerName = "Paranoidní trpaslík";
        private string rotationASCII = "→↓←↑";

        private int degreesToRotate = -90;
        private int currentASCII;

        public Point Position { get => position; set => position = value; }

        public ConsoleColor TravelerColor { get => travelerColor; set => travelerColor = value; }

        public string TravelerName { get => travelerName; set => travelerName = value; }
        public string RotationASCII { get => rotationASCII; set => rotationASCII = value; }

        public int DegreesToRotate { get => degreesToRotate; set => degreesToRotate = value; }

        public bool Changed { get; private set; }

        public bool ReachedEnd { get; private set; }

        #endregion

        #region Class Methods

        public ParanoidDwarf(Point pos)
        {
            position = pos;
            currentASCII = 0;
        }

        #endregion

        #region Movement Methods

        public void Step()
        {
            if (ReachedEnd) return;

            //First checks if he can go to the side
            if (!CheckAround(degreesToRotate))
            {
                //Second he checks if he can go forward
                if (!CheckAround(0))
                {
                    //If neither, turn around to the side
                    ChangeASCII(currentASCII + (degreesToRotate / -90));
                }
            }

            CheckIfReachedEnd();
        }

        private bool CheckAround(int plusDeg)
        {
            //Convert where to check into a vector
            int deg = currentASCII * 90 + plusDeg;
            Point side = new Point((int)Math.Cos(MathExtension.DegreesToRadians(deg)), (int)Math.Sin(MathExtension.DegreesToRadians(deg)));

            //If it's non-collidable in there, move to that point
            Point toPos = side + position;
            if (MazeHandler.IsPointInMazeNonCollidable(toPos))
            {
                position = toPos;
                //look that way
                ChangeASCII(deg / 90);
                Changed = true;

                return true;
            }

            return false;
        }

        //Circle around when turning around
        private void ChangeASCII(int asciiIndex)
        {
            if (asciiIndex >= rotationASCII.Length) asciiIndex = 0;
            else if (asciiIndex < 0) asciiIndex = rotationASCII.Length - 1;

            currentASCII = asciiIndex;

            Changed = true;
        }

        private void CheckIfReachedEnd()
        {
            if (position == MazeHandler.GetEndPosition())
            {
                ReachedEnd = true;
                Changed = true;
            }
        }

        #endregion

        #region Render Methods

        public bool DrawTraveler(bool draw)
        {
            Changed = false;

            if (ReachedEnd || !draw) return false;

            Console.ForegroundColor = travelerColor;

            Console.Write(rotationASCII[currentASCII]);

            Console.ResetColor();

            return true;
        }

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

            Console.WriteLine(placeInOrder + ". " + travelerName + " je v CÍLI na pozici " + position.x + ", " + position.y);

            Console.ResetColor();

            return 1;
        }

        #endregion
    }
}
