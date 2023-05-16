using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [SerializeField] private float _tickFrequence = .05f;
    [SerializeField] private float _rotationSpeed;
    //[SerializeField] private bool _useRelativeRotation;

    private WaitForSeconds _delay;

    private void Awake()
    {
        _delay = new WaitForSeconds(_tickFrequence);
    }

    private void OnEnable()
    {
        StartCoroutine(RotateObjectCoroutine());
    }

    private void OnDisable()
    {
        StopCoroutine(RotateObjectCoroutine());
    }

    private IEnumerator RotateObjectCoroutine()
    {
        //Vector3 rotation = _useRelativeRotation ? transform.up : Vector3.up;
        while (true)
        {
            transform.Rotate(Vector3.up * _rotationSpeed);
            yield return _delay;
        }
    }
}
