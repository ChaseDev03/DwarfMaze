

namespace DwarfMaze
{
    public struct Point
    {
        #region Declarations

        public int x;
        public int y;

        #endregion

        #region Struct Methods

        public Point(int xPos, int yPos)
        {
            x = xPos;
            y = yPos;
        }

        #endregion

        #region Operator Methods

        public static bool operator ==(Point a, Point b)
        {
            return a.x == b.x && a.y == b.y;
        }

        public static bool operator !=(Point a, Point b)
        {
            return a.x != b.x || a.y != b.y;
        }

        public static Point operator +(Point a, Point b)
        {
            a.x = a.x + b.x;
            a.y = a.y + b.y;

            return a;
        }

        public static Point operator -(Point a, Point b)
        {
            a.x = a.x - b.x;
            a.y = a.y - b.y;

            return a;
        }

        public static Point operator *(Point a, Point b)
        {
            a.x = a.x * b.x;
            a.y = a.y * b.y;

            return a;
        }

        public static Point operator /(Point a, Point b)
        {
            a.x = a.x / b.x;
            a.y = a.y / b.y;

            return a;
        }

        #endregion
    }
}
