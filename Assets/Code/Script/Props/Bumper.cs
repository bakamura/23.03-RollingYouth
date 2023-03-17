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
        rb.AddForce(-collision.GetContact(0).normal * _force, ForceMode.Acceleration);
        if (_loseMass) rb.mass -= _massLoss;
    }
}
