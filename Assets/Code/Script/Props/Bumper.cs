using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper : MonoBehaviour
{
    [SerializeField] private float _force;
    [SerializeField] private float _massLoss;
    [SerializeField] private bool _loseMass;
    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb = collision.rigidbody;
        Vector3 direction = collision.GetContact(0).normal;
        rb.AddForce(-new Vector3(direction.x, 0, direction.z) * _force, ForceMode.Acceleration);
        if (_loseMass) rb.mass -= _massLoss;
    }
}
