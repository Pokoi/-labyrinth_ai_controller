using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.DataStructures;
using UnityEngine;

namespace BreadthFirstSearch
{

    /// <summary>
    /// Breadth first search.
    /// </summary>
    public class BreadthFirstSearch : AbstractPathMind
    {       

        List<Node> open_list;
        List<Locomotion.MoveDirection> movements;
        Color tint_color;
        Color walked_color;

        Node goal_node;

        public override Locomotion.MoveDirection GetNextMove(BoardInfo boardInfo, CellInfo currentPos, CellInfo[] goals)
        {
            if(walked_color == null) walked_color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);

            switch (current_phase)
            {
                case Phases.Searching: SetMovements(Search(ref boardInfo, ref currentPos, ref goals)); break;
                case Phases.PathFound: currentPos.TintCell(walked_color, walked_color); return ReadActions();
                case Phases.NotPathFound: return Locomotion.MoveDirection.None;
            }
            return Locomotion.MoveDirection.None;
        }

        public override void Repath()
        {

        }

        /// <summary>
        /// Searching algorithim
        /// </summary>
        /// <returns>The search.</returns>
        /// <param name="_board">Board.</param>
        /// <param name="initial_position">Initial position.</param>
        /// <param name="goals">Goals.</param>
        private Node Search(ref BoardInfo _board, ref CellInfo initial_position, ref CellInfo[] goals)
        {
            //Generate the random color
            tint_color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);

            //Create the open node list
            open_list = new List<Node>();

            //Add the first node to the list
            open_list.Add(new Node(null, initial_position, Locomotion.MoveDirection.None));

            //Meanwhile the open list has items
            while (open_list.Count > 0)
            {
                //Take out the first item in the list to use it
                var node = open_list[0];
                open_list.RemoveAt(0);

                //Check if it's the goal and return it
                if (IsGoal(node, ref goals)) return node;

                //Expand the children of the node and add it to the list if there arent already in the list
                foreach (var child in node.Expand(ref _board))
                { if (!InList(child)) { open_list.Add(child); child.GetCell.TintCell(tint_color, walked_color);} }
            }

            //If no goal is found return null and updates the phase
            current_phase = Phases.NotPathFound;
            return null;
        }

        private Locomotion.MoveDirection ReadActions()
        {
            Locomotion.MoveDirection movement_to_return;
            movement_to_return = movements[0];
            movements.RemoveAt(0);
            return movement_to_return;
        }

        /// <summary>
        /// Check if a given node is goal
        /// </summary>
        /// <returns><c>true</c>, if its goal, <c>false</c> otherwise.</returns>
        /// <param name="_node">The node to check.</param>
        /// <param name="goals">The goals.</param>
        private bool IsGoal(Node _node, ref CellInfo[] goals)
        {
            int iterator;
            for (iterator = 0; iterator < goals.Length && goals[iterator] != _node.GetCell; iterator++) ;

            return iterator < goals.Length;
        }

        /// <summary>
        /// Set the list of movements to arrive at the goal
        /// </summary>
        /// <param name="_final_node">The final node.</param>
        private void SetMovements(Node _final_node)
        {
            movements = new List<Locomotion.MoveDirection>();
            while (_final_node.GetParent != null)
            {
                movements.Add(_final_node.GetActionNeeded);
                _final_node = _final_node.GetParent;
            }
            movements.Reverse();
            current_phase = Phases.PathFound;
        }

        /// <summary>
        /// Returns if a given node is in the open list
        /// </summary>
        /// <returns><c>true</c>, if was in the list, <c>false</c> otherwise.</returns>
        /// <param name="n">The node</param>
        private bool InList(Node n)
        {
            bool is_in_list = false;

            foreach (Node _n in open_list) if (_n.GetCell == n.GetCell) is_in_list = true;

            return is_in_list;
        }

    }

    /// <summary>
    /// Node class
    /// </summary>
    [System.Serializable]
    public class Node
    {
        /// <summary>
        /// Parent reference of this node.
        /// </summary>
        /// <value>The parent node.</value>
        public Node GetParent { get; }
        /// <summary>
        /// Cell reference of this node.
        /// The status in the laberinthim
        /// </summary>
        /// <value>The state.</value>
        public CellInfo GetCell { get; }
        /// <summary>
        /// Reference of what movement is needed to arrived at this node cell
        /// </summary>
        /// <value>The action needed.</value>
        public Locomotion.MoveDirection GetActionNeeded { get; }

        /// <summary>
        /// Builder method of the node
        /// </summary>
        /// <param name="_parent">Parent.</param>
        /// <param name="_cell">Cell.</param>
        /// <param name="_action">Action.</param>
        public Node(Node _parent, CellInfo _cell, Locomotion.MoveDirection _action)
        {
            GetParent = _parent;
            GetCell = _cell;
            GetActionNeeded = _action;
        }

        /// <summary>
        /// Expands a node.
        /// </summary>
        /// <returns>The node walkable neighbours nodes.</returns>
        /// <param name="_b">The board info.</param>
        public List<Node> Expand(ref BoardInfo _b)
        {
            CellInfo[] neighbours = GetCell.WalkableNeighbours(_b);
            List<Node> neighbours_nodes = new List<Node>();

            for (int iterator = 0; iterator < neighbours.Length; iterator++)
            {
                if (neighbours[iterator] != null)
                {
                    switch (iterator)
                    {
                        case 0: neighbours_nodes.Add(new Node(this, neighbours[iterator], Locomotion.MoveDirection.Up)); break;
                        case 1: neighbours_nodes.Add(new Node(this, neighbours[iterator], Locomotion.MoveDirection.Right)); break;
                        case 2: neighbours_nodes.Add(new Node(this, neighbours[iterator], Locomotion.MoveDirection.Down)); break;
                        case 3: neighbours_nodes.Add(new Node(this, neighbours[iterator], Locomotion.MoveDirection.Left)); break;
                    }
                }
            }

            return neighbours_nodes;
        }

    }
}
