using System.Collections.Generic;
using UnityEngine;

public partial class GridManager
{
    class GridDictionaryComparer : IEqualityComparer<Vector3>
    {
        public bool Equals(Vector3 x, Vector3 y)
        {
            return x == y;
        }

        public int GetHashCode(Vector3 obj)
        {
            return obj.GetHashCode();
        }
    }
}
