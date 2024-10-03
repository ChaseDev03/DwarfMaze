using DwarfMaze.Custom_Timer;
using DwarfMaze.Movement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DwarfMaze.Travelers
{
    public class IntergalacticDwarf : ITraveler
    {
        #region Declarations

        private Point position;

        private ConsoleColor travelerColor = ConsoleColor.DarkCyan;

        private CustomTimer walker;

        private string travelerName = "Intergalactic Dwarf";
        private string rotationASCII = "→";

        private int timeWaited;
        private int timeToWait = 10;

        private bool renderAtEnd = true;


        public Point Position { get => position; set => position = value; }

        public ConsoleColor TravelerColor { get => travelerColor; set => travelerColor = value; }

        public string TravelerName { get => travelerName; set => travelerName = value; }
        public string RotationASCII { get => rotationASCII; set => rotationASCII = value; }

        public bool Changed { get; set; }

        public bool ReachedEnd { get; set; }

        #endregion

        #region Class Methods

        public IntergalacticDwarf(Point pos, CustomTimer walkr, int minTime, int maxTime)
        {
            position = pos;

            //Initialize random time until the dwarf teleports to end
            Random r = new Random();
            timeToWait = r.Next(minTime, maxTime);

            walker = walkr;

            Changed = true;
        }

        #endregion

        #region Movement Methods

        public void Step()
        {
            if (ReachedEnd) return;

            Changed = true;

            //Add time based on how long it takes before new frame is called
            timeWaited += walker.MillisecondsToPass;

            if (timeWaited >= timeToWait)
            {
                position = MazeHandler.GetEndPosition();
                ReachedEnd = true;
            }
        }

        #endregion

        #region Rendering Methods

        public bool DrawTraveler(bool draw)
        {
            Changed = false;

            if (!draw) return false;

            //If reached end, first render him on the spot, stop rendering him next frame, so you can see him there at least for a frame
            if (ReachedEnd)
            {
                if (!renderAtEnd) return false;
                else if (renderAtEnd)
                {
                    Changed = true;
                    renderAtEnd = false;
                }
            }

            Console.ForegroundColor = travelerColor;

            Console.Write(rotationASCII);

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
                Console.WriteLine(placeInOrder + ". " + travelerName + " has trouble finding his black hole at position " + position.x + ", " + position.y);

                Console.ResetColor();
                return 1;
            }

            Console.WriteLine(placeInOrder + ". " + travelerName + " teleported to the FINISH at position " + position.x + ", " + position.y);

            Console.ResetColor();

            return 1;
        }

        #endregion
    }
}
