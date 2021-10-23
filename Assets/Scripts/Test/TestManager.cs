using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager : MonoBehaviour
{
    public GameObject EntityPrefab;

    public int SpawnEntityCount = 1000;

    // Start is called before the first frame update
    void Awake()
    {
        for (int i = 0; i < SpawnEntityCount; ++i)
        {
            Instantiate(EntityPrefab).name = $"Entity - {i}";
        }
    }

    //

    public static GridManager Grid = new GridManager(8, 8);
}
