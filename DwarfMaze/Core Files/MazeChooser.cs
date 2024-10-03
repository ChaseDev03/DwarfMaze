using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DwarfMaze.Core_Files
{
    public static class MazeChooser
    {
        #region Maze Choosing

        public static string GetMazeToLoad()
        {
            FileInfo[] mazes = GetMazeFileInfo();

            while (true)
            {
                ShowMazeOptions(mazes);

                //If got a validated input, return path to chosen maze
                int res;
                if (int.TryParse(Console.ReadLine(), out res) && res >= 0 && res < mazes.Length)
                {
                    Console.Clear();
                    return mazes[res].FullName;
                }

                //If not, show error and loop
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid input.");
                Console.ResetColor();
            }
        }

        private static FileInfo[] GetMazeFileInfo()
        {
            FileInfo[] availMazes = new DirectoryInfo(Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\Data\\").GetFiles();

            //If no, show error and end the program
            if (availMazes.Length == 0) ErrorHandler.ThrowError("No mazes available in the Data folder.");

            return availMazes;
        }

        #endregion

        #region Show Methods

        private static void ShowMazeOptions(FileInfo[] mazes)
        {
            Console.WriteLine("Choose a maze to load:");
            for (int i = 0; i < mazes.Length; i++)
            {
                Console.WriteLine(i + $" - {mazes[i].Name}");
            }
        }

        #endregion
    }
}
