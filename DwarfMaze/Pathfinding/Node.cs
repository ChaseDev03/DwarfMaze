using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DwarfMaze.Pathfinding
{
    public class Node
    {
        #region Declarations

        private Node parentNode = null;

        private Point position;

        private NodeState nodeState = NodeState.Unchecked;

        private double gCost;
        private double hCost;


        public Node ParentNode { get => parentNode; set => parentNode = value; }

        public NodeState State { get => nodeState; set => nodeState = value; }

        public Point Position { get => position; }

        public double GCost { get => gCost; }
        public double HCost { get => hCost; }
        public double FCost { get => gCost + hCost; }

        #endregion

        #region Class Methods

        public Node(Point pos)
        {
            position = pos;
            gCost = 1;
        }

        #endregion

        #region Calculation Methods

        //Calculates distance
        public static double GetTraversalCost(Point fromPoint, Point toPoint)
        {
            return Math.Sqrt((fromPoint.x - toPoint.x) * (fromPoint.x - toPoint.x) + (fromPoint.y - toPoint.y) * (fromPoint.y - toPoint.y));
        }

        public void CalculateGCost(Node otherNode)
        {
            gCost = otherNode.GCost + GetTraversalCost(position, otherNode.position);
        }

        public void CalculateHCost(Point endPos)
        {
            //Manhattan approach
            hCost = Math.Abs(position.x - endPos.x) + Math.Abs(position.y - endPos.y);
        }

        #endregion
    }
}
