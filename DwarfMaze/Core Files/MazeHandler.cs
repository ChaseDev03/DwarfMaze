using DwarfMaze.Core_Files;
using DwarfMaze.Custom_Timer;
using DwarfMaze.Factories;
using DwarfMaze.Movement;
using DwarfMaze.Rendering;
using System;
using System.Collections.Generic;
using System.IO;

using System.Timers;

namespace DwarfMaze
{
    public static class MazeHandler
    {
        #region Initializations

        public static void InitializeMaze()
        {
            //Let user choose a maze
            string mazeDataPath = MazeChooser.GetMazeToLoad();

            //Not using a Stream, because File.ReadAllLines() does use a Stream anyway, while also should on a lower-level, which could potentially be faster
            maze = new Maze(File.ReadAllLines(mazeDataPath));
        }

        //Wrapper class for initializing Travelers that will spawn
        private static void SpawnTravelers()
        {
            SpawnTraveler("Right Paranoid Dwarf", 5000);
            SpawnTraveler("Left Paranoid Dwarf", 10000);
            SpawnTraveler("Dwarf Time Traveler", 15000);
            SpawnTraveler("Dwarf", 20000);
            
        }

        #endregion

        #region Declarations

        private static Maze maze;
        private static CustomTimer walker;

        private static List<ITraveler> travelers = new List<ITraveler>();


        public static CustomTimer Walker { get => walker; set => walker = value; }

        #endregion

        #region Main Methods

        //Start of program after all's been initialized
        public static void RunMaze()
        {
            MazeRenderer.RenderMaze(maze);

            StartWalking(); //"Framerate" simulation
            SpawnTravelers();

            Console.ReadKey();
        }

        #endregion

        #region Timer Methods

        //Starts "framerate" simulation
        private static void StartWalking()
        {
            //Program updates every 100ms
            walker = new CustomTimer(100);

            //First Step() then dwarf spawning to avoid dwarf making a move as soon as they spawn
            walker.ElapsedEvent += Step;
            DwarfFactory.InitializeCustomTimer(walker);

            //Loop indefinitely
            walker.AutoReset = true;

            walker.Start();
        }

        //Update
        private static void Step(object s, ElapsedEventArgs e)
        {
            //Make travelers do their stuff
            foreach (ITraveler traveler in travelers)
            {
                traveler.Step();
            }

            //Show what traveler's have to say
            ShowTravelerMessages();

            //Render travelers
            RenderTravelers();

            //End of program
            if (CheckIfTravelersReachedEnd())
            {
                walker.AutoReset = false;

                Console.SetCursorPosition(0, maze.MapSize.y + travelers.Count);
                Console.WriteLine("Všichni trpaslíci jsou v cíli.\nStiskněte jakékoli tlačítko pro ukončení programu.");
            }
        }

        #endregion

        #region Render Methods

        private static void RenderTravelers()
        {
            MazeRenderer.DrawTravelers(maze, travelers);
        }

        //Each traveler has his own message position
        private static void ShowTravelerMessages()
        {
            int increment = 0;
            for (int i = 0; i < travelers.Count; i++)
            {
                ITraveler traveler = travelers[i];
                int cursorY = maze.MapSize.y + increment;

                Console.SetCursorPosition(0, cursorY);
                increment += traveler.ShowTravelerMessage(i + 1, cursorY);
            }
        }

        #endregion

        #region Getter Methods

        public static Point GetStartPosition()
        {
            return maze.GetStartPosition();
        }

        public static Point GetEndPosition()
        {
            return maze.GetEndPosition();
        }

        public static bool IsPointInMazeNonCollidable(Point pos)
        {
            return maze.IsPointNonNonCollidable(pos);
        }

        //Sides = directions
        public static HashSet<Point> GetAdjacentNonCollidableSides(Point pos, List<Point> neighborIncrement)
        {
            return maze.GetAdjacentNonCollidableSides(pos, neighborIncrement);
        }

        public static Maze GetMaze()
        {
            return maze;
        }

        #endregion

        #region Traveler Methods

        //Initializes DwarfSpawner and adds it to DwarfFactory queue
        private static void SpawnTraveler(string dwarfID, int timeTillSpawn)
        {
            DwarfFactory.DwarfSpawner spawner = new DwarfFactory.DwarfSpawner()
            {
                dwarfID = dwarfID,
                spawnPoint = maze.GetStartPosition()
            };

            DwarfFactory.AddDwarfToQueue(spawner, timeTillSpawn);
        }

        //Check if all spawned and waiting for get spawned travelers reached the end
        public static bool CheckIfTravelersReachedEnd()
        {
            if (DwarfFactory.GetDwarfsWaitingToSpawn() > 0) return false;

            foreach (ITraveler traveler in travelers)
            {
                if (!traveler.ReachedEnd || traveler.Changed) return false;
            }

            return true;
        }

        //Register traveler
        public static void WelcomeTraveler(ITraveler dwarf)
        {
            travelers.Add(dwarf);
        }

        #endregion
    }
}
