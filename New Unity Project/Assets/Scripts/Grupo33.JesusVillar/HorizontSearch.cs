using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;
using Assets.Scripts.DataStructures;

namespace HorizontSearch
{

    public class HorizontSearch : AbstractPathMind
    {

        public List<Node> open_list;
        int horizont_k = 3;
        float g;


        public override Locomotion.MoveDirection GetNextMove(BoardInfo boardInfo, CellInfo currentPos, CellInfo[] goals)
        {
            
            open_list = new List<Node>();
            Node node = new Node(null, currentPos, Locomotion.MoveDirection.None);
            open_list.Add(node);

            CellInfo _goal = boardInfo.Enemies.Count > 0 ? GetClosestEnemy(boardInfo.Enemies, node).CurrentPosition() : goals[0];

            Debug.Log(_goal);

            while (GetCurrentDepth(ref node) < horizont_k)
            {
                node = open_list[0];
                open_list.RemoveAt(0);
                if (IsGoal(node, ref goals)) return node.GetActionNeeded;

                foreach (var child in node.Expand(ref boardInfo)) if (!InList(child)) open_list.Add(child);
                foreach (var _n in open_list) _n.UpdateValues(_goal, g);
                g = node.GetG;

                horizont_k++;
                              
            }

            //Get the node in the open_list with the lowest H value
            foreach (Node _n in open_list) if (node == null ||_n.GetH < node.GetH) node = _n;

            return FirstMovement(node);

        }

        public override void Repath()
        {
            
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
        private Locomotion.MoveDirection FirstMovement (Node _final_node)
        {
            var movements = new List<Locomotion.MoveDirection>();
            while (_final_node.GetParent != null)
            {
                movements.Add(_final_node.GetActionNeeded);
                _final_node = _final_node.GetParent;
            }
            movements.Reverse();
            return movements[0];
        }

        private EnemyBehaviour GetClosestEnemy(List<EnemyBehaviour> _enemies, Node current_node)
        {
            EnemyBehaviour _closest = null;
            foreach (EnemyBehaviour _e in _enemies) if (_closest == null || current_node.CalculateEuclideanGeometry(_e.CurrentPosition().ColumnId, _e.CurrentPosition().RowId) < current_node.CalculateEuclideanGeometry(_closest.CurrentPosition().ColumnId, _closest.CurrentPosition().RowId)) _closest = _e;
            Debug.Log(_closest.name);
            return _closest;
        }

        private int GetCurrentDepth(ref Node node)
        {
            int iterator = 0;

            while (node.GetParent != null)
            {
                node = node.GetParent;
                iterator++;
            }

            return iterator;
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
        public Node GetParent { get; set; }
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

        public float GetG { get; private set; }
        public float GetH { get; private set; }
        public float GetF { get; private set; }

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

        public void UpdateValues(CellInfo end, float _g)
        {
            GetG = this.GetCell.WalkCost + _g;
            GetH = CalculateEuclideanGeometry(end.ColumnId, end.RowId);
            GetF = GetG + GetH;
        }


        public float CalculateEuclideanGeometry(float final_column, float final_row)
        {
            return Mathf.Abs(this.GetCell.ColumnId - final_column) + Mathf.Abs(this.GetCell.RowId - final_row);
        }

    }
}
