using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatibleProp : MonoBehaviour
{
    [SerializeField] private float _massIncrease;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponentInParent<ObjectGrow>().UpdateSize(transform.localScale.x * transform.localScale.y * transform.localScale.z, _massIncrease);
            Destroy(gameObject);
        }
    }
}
