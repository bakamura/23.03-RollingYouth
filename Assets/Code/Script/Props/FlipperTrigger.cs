using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipperTrigger : MonoBehaviour
{
    [SerializeField] private Flipper _flipper;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) _flipper.Bump(other.attachedRigidbody);
    }
}
