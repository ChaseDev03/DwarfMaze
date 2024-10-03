using DwarfMaze.Movement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;

namespace DwarfMaze.Rendering
{
    public static class MazeRenderer
    {
        #region Declarations

        private static List<int> previouslyRedrawnRows = new List<int>();

        #endregion

        #region Render Methods

        //Sets console window size and renders the entire maze without travelers
        //Done only at the beginning of the program
        public static void RenderMaze(Maze maze)
        {
            Console.SetWindowSize(maze.MapSize.x + 40, maze.MapSize.y + 10);

            DrawMaze(maze);
        }

        private static void DrawMaze(Maze maze)
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0); //Top left corner
            for (int y = 0; y < maze.MapData.GetLength(1); y++)
            {
                for (int x = 0; x < maze.MapData.GetLength(0); x++)
                {
                    Object mapObject = maze.MapData[x, y];

                    if (mapObject != null)
                    {
                        mapObject.DrawObject();
                    }
                }
                Console.WriteLine();
            }
        }

        private static RowHolder GetRowsToRedraw(List<ITraveler> travelers)
        {
            RowHolder rowHolder = new RowHolder();
            rowHolder.travelerDictionary = new Dictionary<Point, HashSet<ITraveler>>();
            rowHolder.rowsToRedraw = new HashSet<int>();

            foreach (ITraveler traveler in travelers)
            {
                //If traveler changed and his row is not in the list, add it
                if (traveler.Changed && !rowHolder.rowsToRedraw.Contains(traveler.Position.y))
                {
                    rowHolder.rowsToRedraw.Add(traveler.Position.y);
                }

                //Add all travelers and their positions
                if (!rowHolder.travelerDictionary.ContainsKey(traveler.Position))
                {
                    rowHolder.travelerDictionary.Add(traveler.Position, new HashSet<ITraveler>());
                }

                rowHolder.travelerDictionary[traveler.Position].Add(traveler);
            }

            //Make temporary copy as a list
            List<int> tempRedraw = rowHolder.rowsToRedraw.ToList();

            //Add previously redrawn rows in the list to remove ghosts
            foreach (int row in previouslyRedrawnRows)
            {
                if (!rowHolder.rowsToRedraw.Contains(row))
                    rowHolder.rowsToRedraw.Add(row);
            }

            //Save as previously redrawn rows
            previouslyRedrawnRows = tempRedraw;

            return rowHolder;
        }

        public static void DrawTravelers(Maze maze, List<ITraveler> travelers)
        {
            //Get traveler rows and dictionary of positions for quick search, remove duplicates
            RowHolder rowData = GetRowsToRedraw(travelers);
            
            //Redraw rows with travelers
            foreach (int y in rowData.rowsToRedraw)
            {
                //Redraws only changed rows, the rest stays the same
                Console.SetCursorPosition(0, y);

                for (int x = 0; x < maze.MapSize.x; x++)
                {
                    Point currentPosition = new Point(x, y);

                    //Check if traveler should be rendered
                    bool wasTravelerRendered = false;
                    if (rowData.travelerDictionary.ContainsKey(currentPosition))
                    {
                        HashSet<ITraveler> travsOnSpot = rowData.travelerDictionary[currentPosition];

                        //If yes, only first is actually rendered, the rest needs to get their method called to reset their Changed property
                        int i = 0;
                        foreach (ITraveler traveler in rowData.travelerDictionary[currentPosition])
                        {
                            i++;

                            if (i == 1)
                                wasTravelerRendered = traveler.DrawTraveler(true);
                            else
                                wasTravelerRendered = traveler.DrawTraveler(false);
                        }

                        //If any traveler was rendered, continue to next position
                        if (wasTravelerRendered) continue;
                    }

                    //If no traveler was rendered, render map object
                    Object mapObject = maze.MapData[x, y];

                    if (mapObject != null)
                    {
                        maze.MapData[x, y].DrawObject();
                    }
                }
            }
        }

        #endregion

        #region Struct

        private struct RowHolder
        {
            #region Declarations

            public Dictionary<Point, HashSet<ITraveler>> travelerDictionary;

            public HashSet<int> rowsToRedraw;

            #endregion
        }

        #endregion
    }
}
