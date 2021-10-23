using UnityEngine;

public interface IGridCellEntity
{
    public GridCell CurrentCell { get; set; }

    public GameObject gameObject { get; } /* MonoBehaviour implements this, so we can access gameObject from interface */

    public abstract Vector3 GetPosition();
}