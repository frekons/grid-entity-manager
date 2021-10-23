using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour, IGridCellEntity
{
    [SerializeField]
    private float _speed = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        var randomPos = Random.insideUnitSphere;

        randomPos.y = 0;

        transform.SetPositionAndRotation(randomPos * 500.0f, Random.rotation);

        TestManager.Grid.AddEntity(this);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += _speed * Time.deltaTime * transform.forward;

        TestManager.Grid.UpdateEntity(this);

        var entities = TestManager.Grid.GetEntitiesInArea(transform.position, 8f);

        foreach (var entity in entities)
        {
            var renderer = entity.gameObject.GetComponent<Renderer>();

            if (renderer)
                renderer.material.SetColor("_Color", entities.Count > 1 ? Color.red : Color.white);
        }
    }

    //

    public Vector3 GetPosition()
    {
        return transform.position; 
    }

    public GridCell CurrentCell { get; set; }

}
