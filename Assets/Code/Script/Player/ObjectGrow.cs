using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ObjectGrow : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform _objectToGrow;
    [SerializeField] private Rigidbody _objectPhysics;

    [Header("Variables")]
    [SerializeField] private float _minSize;
    [SerializeField] private float _maxSize;
    [SerializeField, Range(0f, 1f), Tooltip("the minimum size to eat something in percentage")] private float _minRequiredSizeToEatPercentage;
    [SerializeField, Tooltip("the size increase relative to its initial size")] private float _sizeIncreaseFactor = 1f;

    private float _currentSize;
    private float _initialMass;

    public Transform ObjectToGrow => _objectToGrow;
    public Rigidbody ObjectPhysics => _objectPhysics;
    public Action OnObjectGrow;

    private void Awake()
    {
        _currentSize = _objectToGrow.localScale.magnitude;
        _initialMass = _objectPhysics.mass;
    }

    public bool UpdateSize(float objectSize, float objectMass = 0)
    {
        //Debug.Log($"current size {_currentSize} , required size {objectSize * _minRequiredSizeToEatPercentage}");
        if (_currentSize >= objectSize * _minRequiredSizeToEatPercentage)
        {
            float finalVolume = objectSize * .476f;
            if (objectSize < 0)
            {
                _objectToGrow.localScale = new Vector3(
                Mathf.Clamp(_objectToGrow.localScale.x + objectSize, _minSize, _maxSize),
                Mathf.Clamp(_objectToGrow.localScale.y + objectSize, _minSize, _maxSize),
                Mathf.Clamp(_objectToGrow.localScale.z + objectSize, _minSize, _maxSize));
            }
            else
            {                
                _objectToGrow.localScale = new Vector3(
                Mathf.Clamp(_objectToGrow.localScale.x + finalVolume, _minSize, _maxSize),
                Mathf.Clamp(_objectToGrow.localScale.y + finalVolume, _minSize, _maxSize),
                Mathf.Clamp(_objectToGrow.localScale.z + finalVolume, _minSize, _maxSize));
            }
            if (_objectPhysics.mass + objectMass > _initialMass) _objectPhysics.mass += finalVolume * _sizeIncreaseFactor;
            else _objectPhysics.mass = _initialMass;
            _currentSize = _objectToGrow.localScale.magnitude;
            OnObjectGrow?.Invoke();
            return true;
        }
        return false;
    }
}
