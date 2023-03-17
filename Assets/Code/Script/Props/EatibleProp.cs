using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatibleProp : MonoBehaviour
{
    [SerializeField] private float _massIncrease;
    //private MeshFilter _meshFilter;
    //private void Awake()
    //{
    //    _meshFilter = GetComponent<MeshFilter>();        
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //collision.transform.GetComponent<ObjectGrow>().UpdateSize(_meshFilter.mesh.bounds.extents.magnitude * transform.localScale.magnitude, _massIncrease);
            other.GetComponentInParent<ObjectGrow>().UpdateSize(transform.localScale.magnitude, _massIncrease);
            Destroy(gameObject);
        }
    }
}
