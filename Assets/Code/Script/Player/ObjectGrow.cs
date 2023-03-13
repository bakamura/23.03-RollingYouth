using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private float _meshExtentsFactor;

    private void Awake()
    {
        _meshExtentsFactor = _objectToGrow.GetComponent<MeshFilter>().sharedMesh.bounds.extents.magnitude;
        //_currentSize = _objectToGrow.localScale.magnitude * _meshExtentsFactor;
        _currentSize = _objectToGrow.localScale.magnitude;
        _sizeIncrease = _objectToGrow.localScale.x * (1f + _sizeIncreaseFactor);
    }

    public void UpdateSize(float objectSize, float objectMass = 0)
    {
        //Debug.Log($"current size {_currentSize} , required size {objectSize * _minRequiredSizeToEatPercentage}");
        if (_currentSize >= objectSize * _minRequiredSizeToEatPercentage || objectSize < 0 || objectMass < 0)
        {
            //increase size relative from the base size of the object that will grow (the GameObject with this script)
            Vector3 newSize = new Vector3(
                Mathf.Clamp(_objectToGrow.localScale.x + _sizeIncrease, _minSize, _maxSize),
                Mathf.Clamp(_objectToGrow.localScale.y + _sizeIncrease, _minSize, _maxSize),
                Mathf.Clamp(_objectToGrow.localScale.z + _sizeIncrease, _minSize, _maxSize));
            //increases size relative to the object eaten size
            //Vector3 newSize = new Vector3(
            //    Mathf.Clamp(_objectToGrow.localScale.x + objectSize, _minSize, _maxSize),
            //    Mathf.Clamp(_objectToGrow.localScale.y + objectSize, _minSize, _maxSize),
            //    Mathf.Clamp(_objectToGrow.localScale.z + objectSize, _minSize, _maxSize));
            _objectToGrow.localScale = newSize;
            if (_objectPhysics) _objectPhysics.mass += objectMass;
            _currentSize = _objectToGrow.localScale.magnitude;
        }
    }
}
