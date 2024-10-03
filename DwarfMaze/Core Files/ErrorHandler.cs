using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DwarfMaze.Core_Files
{
    public static class ErrorHandler
    {
        #region Error Methods

        //Show error, wait till user presses enter, then exit program
        public static void ThrowError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();

            Console.ReadLine();
            Environment.Exit(0);
        }

        #endregion
    }
}
