using DwarfMaze.Factories;

namespace DwarfMaze
{
    class Initializator
    {
        #region Class Methods

        //This is where the program starts and initializes things that need to be done beforehand
        static void Main(string[] args)
        {
            ObjectFactory.InitializeObjectPrefabs();
            DwarfFactory.InitializeDwarfCreation();

            Maze.InitializeObjectModification();
            
            MazeHandler.InitializeMaze();
            MazeHandler.RunMaze();
        }

        #endregion
    }
}
