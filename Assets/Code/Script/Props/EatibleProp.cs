using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatibleProp : MonoBehaviour
{
    [SerializeField] private float _massIncrease;
    private MeshFilter _meshFilter;
    private void Awake()
    {
        _meshFilter = GetComponent<MeshFilter>();        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.GetComponent<ObjectGrow>().UpdateSize(_meshFilter.mesh.bounds.extents.magnitude * transform.localScale.magnitude, _massIncrease);
            Destroy(gameObject);
        }
    }
}
