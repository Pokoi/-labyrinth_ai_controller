using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts.DataStructures
{
    public class CellInfo : ICloneable
    {
        public PlaceableItem ItemInCell { get; set; }

        public int RowId { get; private set; }
        public int ColumnId { get; private set; }

        public string CellId { get { return "" + this.ColumnId + "," + this.RowId; } }
        public float WalkCost { get; private set; }
        public bool Walkable { get; private set; }

        GameObject cell_gameobject;

        public CellInfo(int col, int row)
        {
            this.WalkCost = 1.0f;
            this.Walkable = true;
            this.RowId = row;
            this.ColumnId = col;
            this.ItemInCell = null;
        }

        
        public Vector2 GetPosition
        {
            get
            {
                return new Vector2(this.ColumnId, this.RowId);
            }
        }

        public void ChangeToNoWalkable()
        {
            this.Walkable = false;
            this.WalkCost = float.MaxValue;
        }

        public object Clone()
        {
            var result = new CellInfo(this.ColumnId, this.RowId)
            {
                Walkable = this.Walkable,
                WalkCost = this.WalkCost
            };
            if (this.ItemInCell != null)
                ItemInCell = (PlaceableItem)this.ItemInCell.Clone();


            return result;
        }

        public GameObject CreateGameObject(BoardManager boardManager)
        {
            var tile = boardManager.floorTile;
            if (!this.Walkable) tile = boardManager.wallTile;
            cell_gameobject =  GameObject.Instantiate(tile, new Vector3(this.ColumnId, this.RowId, 0f),
                Quaternion.identity, boardManager.transform);
            if (ItemInCell != null)
            {
                var itGO = this.ItemInCell.CreateGameObject(boardManager, cell_gameobject.transform);
                itGO.GetComponent<ItemLogic>().Type = this.ItemInCell.Type;
                itGO.GetComponent<ItemLogic>().PlaceableItem = this.ItemInCell;

            }
            return cell_gameobject;
        }

        public void TintCell(Color color, Color walked) { if(cell_gameobject.GetComponentInChildren<SpriteRenderer>().color !=  walked) cell_gameobject.GetComponentInChildren<SpriteRenderer>().color = color; }

        public CellInfo[] WalkableNeighbours(BoardInfo board)
        {
            var neighbours = new CellInfo[4];
            //UP
            neighbours[0] = (this.RowId < (board.NumRows -1) && board.CellInfos[this.ColumnId, this.RowId + 1].Walkable)
                ? board.CellInfos[this.ColumnId, this.RowId + 1]
                : null;
            //Right
            neighbours[1] = (this.ColumnId < (board.NumColumns -1) && board.CellInfos[this.ColumnId+1, this.RowId].Walkable)
                ? board.CellInfos[this.ColumnId+1, this.RowId]
                : null;
            //Down
            neighbours[2] = (this.RowId > 0 && board.CellInfos[this.ColumnId, this.RowId - 1].Walkable)
                ? board.CellInfos[this.ColumnId, this.RowId - 1]
                : null;
            //Left
            neighbours[3] = (this.ColumnId > 0 && board.CellInfos[this.ColumnId - 1, this.RowId].Walkable)
                ? board.CellInfos[this.ColumnId - 1, this.RowId]
                : null;
            return neighbours;
        }
    }
}