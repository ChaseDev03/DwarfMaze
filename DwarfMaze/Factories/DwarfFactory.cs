using DwarfMaze.Travelers;
using DwarfMaze.Custom_Timer;
using DwarfMaze.Movement;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Timers;

namespace DwarfMaze.Factories
{
    public static class DwarfFactory
    {
        #region Initializations

        //Assigns IDs to each dwarf creation method
        public static void InitializeDwarfCreation()
        {
            dwarfCreationMethods.Add("Dwarf", CreatePathfindingDwarf);
            dwarfCreationMethods.Add("Left Paranoid Dwarf", CreateLeftParanoidDwarf);
            dwarfCreationMethods.Add("Right Paranoid Dwarf", CreateRightParanoidDwarf);
            dwarfCreationMethods.Add("Dwarf Time Traveler", CreateIntergalacticDwarf);
        }

        //Either instantly instantiates a traveler (dwarf) or holds his spawn settings until its his time to spawn
        public static void AddDwarfToQueue(DwarfSpawner spawner, int timeTillSpawn)
        {
            if (timeTillSpawn > 0)
            {
                spawner.timeTillSpawn = timeTillSpawn;
                dwarfSpawners.Add(spawner);
                
                return;
            }

            MazeHandler.WelcomeTraveler(dwarfCreationMethods[spawner.dwarfID](spawner.spawnPoint));
        }

        //Gets the main timer and adds a method to its event handler that handles counting the time until a Traveler should spawn
        public static void InitializeCustomTimer(CustomTimer timer)
        {
            walker = timer;
            walker.ElapsedEvent += AddTimeElapsed;
        }

        #endregion

        #region Declarations

        private static Dictionary<string, Func<Point, ITraveler>> dwarfCreationMethods = new Dictionary<string, Func<Point, ITraveler>>();

        private static List<DwarfSpawner> dwarfSpawners = new List<DwarfSpawner>();

        private static CustomTimer walker;

        #endregion

        #region Creation Methods

        //Dwarf that finds a straight path from the start to the end. Uses A* pathfinding algorithm
        private static ITraveler CreatePathfindingDwarf(Point spawnPoint)
        {
            Dwarf dwarf = new Dwarf(spawnPoint);

            dwarf.InitializeAStarSeeker(MazeHandler.GetMaze());

            return dwarf;
        }

        //Dwarf that turns to his left if it's possible for him
        private static ITraveler CreateLeftParanoidDwarf(Point spawnPoint)
        {
            return new ParanoidDwarf(spawnPoint);
        }

        //Dwarf that turns to his right if it's possible for him
        private static ITraveler CreateRightParanoidDwarf(Point spawnPoint)
        {
            ParanoidDwarf dwarf = new ParanoidDwarf(spawnPoint);

            dwarf.DegreesToRotate = 90;

            dwarf.TravelerColor = ConsoleColor.Magenta;

            return dwarf;
        }

        //Dwarf, the teleporter
        private static ITraveler CreateIntergalacticDwarf(Point spawnPoint)
        {
            return new IntergalacticDwarf(spawnPoint, MazeHandler.Walker, 5000, 15000);
        }

        #endregion

        #region Getter Methods

        public static int GetDwarfsWaitingToSpawn()
        {
            return dwarfSpawners.Count;
        }

        #endregion

        #region Time Handling

        //Handles DwarfSpawner time till spawn. Gets called every main CustomTimer's (walker's) set time
        public static void AddTimeElapsed(object s, ElapsedEventArgs args)
        {
            for (int i = 0; i < dwarfSpawners.Count; i++)
            {
                DwarfSpawner spawner = dwarfSpawners[i];

                //Adds timer's MillisecondsToPass amount to each DwarfSpawner that's still waiting to spawn. This means the accuracy is dependent on the timer's settings.
                spawner.elapsedTime += walker.MillisecondsToPass;

                //If the time waited has reached the time till spawn, spawn Traveler
                if (spawner.elapsedTime >= spawner.timeTillSpawn)
                {
                    MazeHandler.WelcomeTraveler(dwarfCreationMethods[spawner.dwarfID](spawner.spawnPoint));
                    //Removes current DwarfSpawner
                    dwarfSpawners.RemoveAt(i);
                    //Substract i by 1 to not miss any other DwarfSpawners
                    i--;

                    continue;
                }

                //Save changes
                dwarfSpawners[i] = spawner;
            }
        }

        #endregion

        #region Dwarf Spawner

        //A holder for the dwarf settings
        public struct DwarfSpawner
        {
            #region Declarations

            public string dwarfID;

            public Point spawnPoint;

            public int timeTillSpawn;
            public int elapsedTime;

            #endregion
        }

        #endregion
    }
}
