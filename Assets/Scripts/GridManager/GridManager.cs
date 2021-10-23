using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public partial class GridManager
{
    public GridManager(float cellWidth, float cellHeight)
    {
        this.CellWidth = cellWidth;

        this.CellHeight = cellHeight;

        _gridDictionary = new Dictionary<Vector3, GridCell>(new GridDictionaryComparer());
    }

    public Vector3 GetGridPosition(Vector3 position)
    {
        /* for performance purposes */
        position.x -= position.x % CellWidth;

        position.y = 0;

        position.z -= position.z % CellHeight;

        return position;

        //return new Vector3(position.x - (position.x % CellWidth), 0, position.z - (position.z % CellHeight));
    }

    public bool DoesGridExist(Vector3 position)
    {
        var gridPosition = GetGridPosition(position);

        return _gridDictionary.ContainsKey(gridPosition);
    }

    public bool DoesGridExist(Vector3 position, out Vector3 gridPosition)
    {
        gridPosition = GetGridPosition(position);

        return _gridDictionary.ContainsKey(gridPosition);
    }

    public GridCell GetCell(Vector3 position)
    {
        if (!DoesGridExist(position, out var gridPosition))
            return default;

        return _gridDictionary[gridPosition];
    }

    public GridCell GetCell(Vector3 position, out Vector3 gridPosition)
    {
        if (!DoesGridExist(position, out gridPosition))
            return default;

        return _gridDictionary[gridPosition];
    }

    public GridCell AddOrGetCell(Vector3 position)
    {
        var existingCell = GetCell(position, out var gridPosition);

        if (existingCell != default)
            return existingCell;

        var newCell = new GridCell(gridPosition.x, gridPosition.z, CellWidth, CellHeight);

        //Debug.Log("Position: " + position + ", gridPosition: " + gridPosition);

        if (!_gridDictionary.ContainsKey(gridPosition))
        {
            _gridDictionary.Add(gridPosition, newCell);

            return newCell;
        }
        else
        {
            Debug.LogError($"[GridManager] AddOrGetCell, dictionary contains key already.");

            return default;
        }
    }

    public bool AddEntity(IGridCellEntity entity)
    {
        var cell = AddOrGetCell(entity.GetPosition());

        if (cell != default)
        {
            cell.AddEntity(entity);

            return true;
        }
        else
        {
            Debug.LogError($"[GridManager] Error: AddOrGetCell returned default!");
        }

        return false;
    }

    public void UpdateEntity(IGridCellEntity entity)
    {
        var cell = AddOrGetCell(entity.GetPosition());

        if (cell != default)
        {
            /* entity changed cell, so update entity */
            if (entity.CurrentCell != cell)
            {
                RemoveEntity(entity);

                AddEntity(entity);
  
            } /* else don't update because already in cell */
        }
        else
        {
            Debug.LogError($"[GridManager] UpdateEntity, GetCell returned default!");
        }
    }

    public bool RemoveEntity(IGridCellEntity entity)
    {
        var cell = entity.CurrentCell;

        if (cell != default)
        {
            var removed = cell.RemoveEntity(entity);

            /* if there is no entity in this cell, remove this cell */
            if (removed && cell.IsEmpty())
            {
                _gridDictionary.Remove(cell.Position);
            }

            return removed;
        }

        return false;
    }


    public List<IGridCellEntity> GetEntitiesInArea(Vector3 position, float radius)
    {
        List<IGridCellEntity> entityList = new List<IGridCellEntity>();

        Dictionary<int, bool> processedCells = new Dictionary<int, bool>();

        var radiusSquare = radius * radius;

        var cellWidth = CellWidth;

        var cellHeight = CellHeight;

        /* if radius is greater than CellSize then we need to check further more cell */
        var widthStep = Mathf.CeilToInt(radius / cellWidth);

        var heightStep = Mathf.CeilToInt(radius / cellHeight);

        /* we create a square for loop to look up entities in this area */
        for (int i = -widthStep; i <= widthStep; ++i)
        {
            for (int j = -heightStep; j <= heightStep; ++j)
            {
                var cell = GetCell(position + new Vector3(cellWidth * i, 0, cellHeight * j));

                /* if there is cell (means there are entities in it) and if this cell is not processed before in this loop */
                if (cell != default && !processedCells.ContainsKey(cell.GetHashCode()))
                {
                    /* add entities which is closer than radius to position */
                    var entityCount = cell.Entities.Count;

                    for (int z = 0; z < entityCount; ++z)
                    {
                        var entity = cell.Entities[z];

                        var distanceSqr = (entity.GetPosition() - position).sqrMagnitude;

                        if (distanceSqr <= radiusSquare)
                        {
                            entityList.Add(entity);
                        }
                    }

                    /* add to the processedCells, so don't process this cell again */
                    processedCells.Add(cell.GetHashCode(), true);
                }
            }
        }

        return entityList;
    }


    public void Print()
    {
        var lines = _gridDictionary.Select(kvp => kvp.Key + " - " + kvp.Value.ToString());

        var str = string.Join(Environment.NewLine, lines);

        str = str.Insert(0, "\n");

        Debug.Log(str);
    }

    //

    public float CellWidth;

    public float CellHeight;

    /* calling getter causes some performance loss */

    //public float CellWidth { get; set; }

    //public float CellHeight { get; set; }


    //

    private readonly Dictionary<Vector3, GridCell> _gridDictionary;
}
