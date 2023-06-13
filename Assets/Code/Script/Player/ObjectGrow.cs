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
    [SerializeField] private float _massIncreaseFactor = 1f;

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

    public bool UpdateSize(float objectSize, float objectMass = 0, bool checkForMinimalSize = true)
    {
        //Debug.Log($"current size {_currentSize} , required size {objectSize * _minRequiredSizeToEatPercentage}");
        if (checkForMinimalSize)
        {
            if (_currentSize >= objectSize * _minRequiredSizeToEatPercentage)
            {
                ChangeSizeAndMass(objectSize, objectMass);
                return true;
            }
            return false;
        }
        else
        {
            ChangeSizeAndMass(objectSize, objectMass);
            return true;
        }
    }

    private void ChangeSizeAndMass(float objectSize, float objectMass)
    {
        float finalVolume = Mathf.Pow(objectSize + Mathf.Pow(_objectToGrow.localScale.x, 3), (float)1 / 3);
        if (objectSize < 0)
        {
            _objectToGrow.localScale = new Vector3(
            Mathf.Clamp(_objectToGrow.localScale.x + objectSize, _minSize, _maxSize),
            Mathf.Clamp(_objectToGrow.localScale.y + objectSize, _minSize, _maxSize),
            Mathf.Clamp(_objectToGrow.localScale.z + objectSize, _minSize, _maxSize));
            _objectPhysics.mass = Mathf.Clamp(_objectPhysics.mass + objectMass, _initialMass, float.MaxValue);
        }
        else
        {
            _objectToGrow.localScale = new Vector3(
            Mathf.Clamp(finalVolume, _minSize, _maxSize),
            Mathf.Clamp(finalVolume, _minSize, _maxSize),
            Mathf.Clamp(finalVolume, _minSize, _maxSize));
            _objectPhysics.mass += finalVolume * _massIncreaseFactor;
        }
        //if (_objectPhysics.mass + objectMass > _initialMass) _objectPhysics.mass += finalVolume * _sizeIncreaseFactor;
        //else _objectPhysics.mass = _initialMass;
        //_objectPhysics.mass += objectMass;        
        _currentSize = _objectToGrow.localScale.magnitude;
        OnObjectGrow?.Invoke();
    }
}
