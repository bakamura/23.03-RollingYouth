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
    [SerializeField, Range(0f, 1f)] private float _sizeIncreaseFactor = 1f;// a good value for this is .7f

    private float _currentSize;
    private Mesh _mesh;

    private void Awake()
    {
        _mesh = _objectToGrow.GetComponent<MeshFilter>().sharedMesh;
        _currentSize = _objectToGrow.localScale.magnitude * _mesh.bounds.extents.magnitude;
    }

    public void UpdateSize(float objectSize, float objectMass = 0)
    {
        //Debug.Log($"current size {_currentSize} , required size {objectSize * _minRequiredSizeToEatPercentage}");
        if (_currentSize >= objectSize * _minRequiredSizeToEatPercentage || objectSize < 0 || objectMass < 0)
        {
            Vector3 newSize = new Vector3(
                Mathf.Clamp(_objectToGrow.localScale.x + objectSize, _minSize, _maxSize),
                Mathf.Clamp(_objectToGrow.localScale.y + objectSize, _minSize, _maxSize),
                Mathf.Clamp(_objectToGrow.localScale.z + objectSize, _minSize, _maxSize));
            _objectToGrow.localScale = newSize * _sizeIncreaseFactor;
            if (_objectPhysics) _objectPhysics.mass += objectMass;
            _currentSize = _objectToGrow.localScale.magnitude * _mesh.bounds.extents.magnitude;
        }
    }
}
