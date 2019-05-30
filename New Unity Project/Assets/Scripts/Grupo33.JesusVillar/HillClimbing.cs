using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;
using Assets.Scripts.DataStructures;

namespace HillClimbing
{
    public class HillClimbing : AbstractPathMind
    {
        public List<Node> open_list;
        
        float g;


        public override Locomotion.MoveDirection GetNextMove(BoardInfo boardInfo, CellInfo currentPos, CellInfo[] goals)
        {
            open_list = new List<Node>();
            Node node = new Node(null, currentPos, Locomotion.MoveDirection.None);

            CellInfo _goal = boardInfo.Enemies.Count > 0 ? GetClosestEnemy(boardInfo.Enemies, node).CurrentPosition() : goals[0];

            
            open_list.Add(node);

            while (open_list.Count > 0)
            {
                
                foreach (var child in node.Expand(ref boardInfo))    open_list.Add(child);
                foreach (var _n in open_list)                        _n.UpdateValues(_goal, g);

                //Get the node in the open_list with the lowest H value
                foreach (Node _n in open_list) if (node == null || _n.GetH < node.GetH) node = _n;

                Debug.Log(node.GetCell.Walkable);
                return node.GetActionNeeded;
            }

            return Locomotion.MoveDirection.None; 
        }

        public override void Repath()
        {
            
        }

        private EnemyBehaviour GetClosestEnemy(List<EnemyBehaviour> _enemies, Node current_node)
        {
            EnemyBehaviour _closest = null;
            foreach (EnemyBehaviour _e in _enemies) if (_closest == null || current_node.CalculateEuclideanGeometry(_e.CurrentPosition().ColumnId, _e.CurrentPosition().RowId) < current_node.CalculateEuclideanGeometry(_closest.CurrentPosition().ColumnId, _closest.CurrentPosition().RowId)) _closest = _e;
            return _closest;
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

