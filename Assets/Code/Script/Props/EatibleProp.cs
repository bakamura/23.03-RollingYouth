using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatibleProp : MonoBehaviour
{
    [SerializeField, Tooltip("with the current calc for player mass, this value is not being used")] private float _massIncrease;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponentInParent<ObjectGrow>().UpdateSize(transform.localScale.magnitude, _massIncrease))
        {            
            Destroy(gameObject);
        }
    }
}
