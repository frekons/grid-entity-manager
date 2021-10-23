using System.Collections.Generic;
using UnityEngine;

public class GridCell
{
    public GridCell(float x, float y, float width, float height) { this.X = x; this.Y = y; this.Width = width; this.Height = height; }


    #region PUBLIC FUNCTIONS
    public void AddEntity(IGridCellEntity entity)
    {
        if (DoesContainEntity(entity))
            return;

        Entities.Add(entity);

        entity.CurrentCell = this;
    }

    public bool RemoveEntity(IGridCellEntity entity)
    {
        return Entities.Remove(entity);
    }

    public bool DoesContainEntity(IGridCellEntity entity)
    {
        return Entities.Contains(entity);
    }

    public bool IsEmpty()
    {
        return Entities.Count <= 0;
    }
    #endregion


    #region OVERRIDES
    public override string ToString()
    {
        return "GridCell Position: " + Position.ToString();
    }
    #endregion


    public float X { get; set; }

    public float Y { get; set; }

    public Vector3 Position { get { return new Vector3(X, 0, Y); } }

    public float Width { get; set; }

    public float Height { get; set; }


    public List<IGridCellEntity> Entities = new List<IGridCellEntity>(10);
}
