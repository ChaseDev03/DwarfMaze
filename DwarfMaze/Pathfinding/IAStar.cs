using DwarfMaze.Movement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DwarfMaze.Pathfinding
{
    //Interface for objects using A* pathfinding
    public interface IAStar
    {
        #region Initializations

        void InitializeAStarSeeker(Maze maz);

        #endregion

        #region Declarations

        AStarSeeker AStarSeeker { get; set; }

        #endregion
    }
}
