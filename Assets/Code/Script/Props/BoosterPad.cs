using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterPad : MonoBehaviour
{
    [SerializeField] private float _speedBoost;
    private void OnTriggerEnter(Collider other)
    {
        PlayerComponents playerComponents = other.GetComponent<PlayerComponents>();
        if(playerComponents)
        {
            playerComponents.PlayerRigidbody.AddForce(transform.forward * _speedBoost, ForceMode.Impulse);
        }
    }
}
