using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DwarfMaze.Movement
{
    public interface ITraveler
    {
        #region Declarations

        Point Position { get; set; }

        ConsoleColor TravelerColor { get; set; }

        string TravelerName { get; set; }
        string RotationASCII { get; set; }

        bool Changed { get; } //Mostly a render value
        bool ReachedEnd { get; }

        #endregion

        #region Travel Methods

        //Called every frame
        void Step();

        #endregion

        #region Render Methods

        bool DrawTraveler(bool draw);

        /// <summary>
        /// Should return the amount of lines written
        /// </summary>
        /// <param name="placeInOrder"></param>
        /// <returns></returns>
        int ShowTravelerMessage(int placeInOrder, int yLine);

        #endregion
    }
}
