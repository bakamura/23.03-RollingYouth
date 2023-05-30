using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChangePlayerSpeed : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)] private float _velocityReduction;
    [SerializeField] private UnityEvent _onTriggerEnter;

    private void OnTriggerEnter(Collider other)
    {
        PlayerComponents playerComponents = other.GetComponent<PlayerComponents>();
        if (playerComponents)
        {
            playerComponents.PlayerRigidbody.velocity = playerComponents.PlayerRigidbody.velocity * (1f - _velocityReduction);
            _onTriggerEnter?.Invoke();
        }
    }
}
