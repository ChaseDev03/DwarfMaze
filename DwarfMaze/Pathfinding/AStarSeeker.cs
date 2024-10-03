using DwarfMaze.Factories;
using System;
using System.Collections.Generic;

namespace DwarfMaze.Pathfinding
{
    public class AStarSeeker
    {
        #region Declarations

        private Maze maze;
        private Dictionary<Point, Node> nodes = new Dictionary<Point, Node>();

        private Node startNode;
        private Node endNode;


        //Debug variable
        private bool showProcess = false;

        #endregion

        #region Class Methods

        public AStarSeeker(Maze maz)
        {
            maze = maz;
        }

        #endregion

        #region Pathfinding Methods

        //Start of A* algorithm
        public List<Point> FindPath(Point startPos, Point endPos)
        {
            startNode = new Node(startPos);
            endNode = new Node(endPos);

            //Add start and end node
            nodes.Add(startPos, startNode);
            nodes.Add(endPos, endNode);

            //Start search
            List<Point> path = new List<Point>();
            bool foundPath = Search(startNode);

            if (foundPath)
            {
                //Start from the end, get parent, move to the parent, repeat until you reach the start
                Node node = endNode;
                while (node.ParentNode != null)
                {
                    if (showProcess) maze.MapData[node.Position.x, node.Position.y] = ObjectFactory.CreateObject('Y');

                    path.Add(node.Position);
                    node = node.ParentNode;
                }
                //Reverse to start from the start
                path.Reverse();
            }
            return path;
        }

        private bool Search(Node currentNode)
        {
            //Close node
            currentNode.State = NodeState.Closed;
            //Get unchecked or open neighbors
            List<Node> nextNodes = GetAdjacentNodes(currentNode);
            //Sort by lowest F cost
            nextNodes.Sort((node1, node2) => node1.FCost.CompareTo(node2.FCost));

            foreach (Node nextNode in nextNodes)
            {
                //End search if we're at the end
                if (nextNode.Position == endNode.Position)
                {
                    return true;
                }
                else
                {
                    //Otherwise recur
                    if (Search(nextNode))
                        return true;
                }
            }

            return false;
        }

        //Gets appropriate neighbor nodes
        private List<Node> GetAdjacentNodes(Node fromNode)
        {
            List<Node> resultNodes = new List<Node>();
            //Get all non-collidable neighbors
            List<Point> neighboringNodes = maze.GetAdjacentNonCollidablePoints(fromNode.Position);

            foreach (Point p in neighboringNodes)
            {
                Node node;

                //Get node if it's been created, otherwise instantiate a new one with default parameters
                if (nodes.ContainsKey(p)) node = nodes[p];
                else
                {
                    node = new Node(p);
                    node.CalculateGCost(startNode); //How far it is from the starting point
                    node.CalculateHCost(endNode.Position); //Heuristic for how far it is from the end point

                    nodes.Add(p, node);
                }

                //If it's closed, it's done
                if (node.State == NodeState.Closed)
                    continue;

                //If it's open, check if it could potentially be good to move to its neighbor
                if (node.State == NodeState.Open)
                {
                    double traversalCost = Node.GetTraversalCost(node.Position, node.ParentNode.Position);
                    double gTemp = fromNode.GCost + traversalCost;
                    if (gTemp < node.GCost)
                    {
                        //If it is, change its parent to the current node
                        node.ParentNode = fromNode;
                        resultNodes.Add(node);
                    }
                }
                else
                {
                    if (showProcess) maze.MapData[node.Position.x, node.Position.y] = ObjectFactory.CreateObject('B');
                    
                    //If it's unchecked, open it for consideration
                    node.ParentNode = fromNode;
                    node.State = NodeState.Open;
                    resultNodes.Add(node);
                }
            }

            return resultNodes;
        }

        #endregion
    }
}
