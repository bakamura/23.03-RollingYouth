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
    [SerializeField, Range(0f, 1f), Tooltip("the size increase relative to its initial size")] private float _sizeIncreaseFactor = .1f;

    private float _currentSize;
    private float _sizeIncrease;
    private Vector3 _initialSize;
    private float _initialMass;

    //public Vector3 InitialSize => _initialSize;
    //public float InitialMass => _initialMass;
    public Transform ObjectToGrow => _objectToGrow;
    public Rigidbody ObjectPhysics => _objectPhysics;

    private void Awake()
    {
        _currentSize = _objectToGrow.localScale.magnitude;
        _initialSize = _objectToGrow.localScale;
        _initialMass = _objectPhysics.mass;
        _sizeIncrease = _objectToGrow.localScale.x * (1f + _sizeIncreaseFactor);
    }

    public void UpdateSize(float objectSize, float objectMass = 0)
    {
        //Debug.Log($"current size {_currentSize} , required size {objectSize * _minRequiredSizeToEatPercentage}");
        if (_currentSize >= objectSize * _minRequiredSizeToEatPercentage)
        {
            if (objectSize < 0)
            {
                _objectToGrow.localScale = new Vector3(
                Mathf.Clamp(_objectToGrow.localScale.x + objectSize, _minSize, _maxSize),
                Mathf.Clamp(_objectToGrow.localScale.y + objectSize, _minSize, _maxSize),
                Mathf.Clamp(_objectToGrow.localScale.z + objectSize, _minSize, _maxSize));
            }
            else
            {
                //increase size relative from the base size of the object that will grow (the GameObject with this script)
                _objectToGrow.localScale = new Vector3(
                Mathf.Clamp(_objectToGrow.localScale.x + _sizeIncrease, _minSize, _maxSize),
                Mathf.Clamp(_objectToGrow.localScale.y + _sizeIncrease, _minSize, _maxSize),
                Mathf.Clamp(_objectToGrow.localScale.z + _sizeIncrease, _minSize, _maxSize));
                //increases size relative to the object eaten size
                //Vector3 newSize = new Vector3(
                //    Mathf.Clamp(_objectToGrow.localScale.x + objectSize, _minSize, _maxSize),
                //    Mathf.Clamp(_objectToGrow.localScale.y + objectSize, _minSize, _maxSize),
                //    Mathf.Clamp(_objectToGrow.localScale.z + objectSize, _minSize, _maxSize));
            }
            if (_objectPhysics.mass + objectMass > _initialMass) _objectPhysics.mass += objectMass;
            else _objectPhysics.mass = _initialMass;
            _currentSize = _objectToGrow.localScale.magnitude;
        }
    }
}
