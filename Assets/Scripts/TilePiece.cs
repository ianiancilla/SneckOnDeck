using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SnakeGame.GridSpace 
{
    public class TilePiece
    {
        private Transform transform;
        private GridPosition gridPosition;

        public TilePiece(Transform transform)
        {
            this.transform = transform;
            this.gridPosition = Grid.WorldPosToGridPos(transform.position);
        }


        public override string ToString() => $"Tilepiece: {transform.gameObject.name} ({gridPosition})";
        
        public bool IsColliding (TilePiece other)
        {
            return this.gridPosition == other.GetGridPosition();
        }
        public bool IsColliding (GridPosition other)
        {
            return this.gridPosition == other;
        }

        public Transform GetTransform() => transform;
        public GridPosition GetGridPosition() => gridPosition;
        public void MoveTileToGridPos(GridPosition gridPosition)
        {
            this.gridPosition = gridPosition;
            this.transform.position = Grid.GridPosToWorldPos(gridPosition);
        }
    }
}
