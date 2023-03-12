using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _initialDesiredLocation;
    private float _targetInitialDistance;
    private float _meshMagnitude;
#if UNITY_EDITOR
    [SerializeField] private bool _debugDraw;
    [SerializeField] private Color _color;
#endif
    private void Awake()
    {
        if (_target)
        {
            _targetInitialDistance = Vector3.Distance(_target.position + _initialDesiredLocation, _target.position);
            _meshMagnitude = _target.GetComponentInChildren<MeshFilter>().mesh.bounds.extents.magnitude;
        }
    }
    private void Update()
    {
        transform.position = _target.position + (_target.localScale.magnitude * _meshMagnitude + _targetInitialDistance) * -transform.forward; 
    }
    
#if UNITY_EDITOR
    public void RepositionCam()
    {
        transform.position = _target.position + _initialDesiredLocation;
        transform.LookAt(_target.position, Vector3.up);
    }
    private void OnDrawGizmosSelected()
    {
        if (_debugDraw && _target)
        {
            Gizmos.color = _color;
            Gizmos.DrawLine(_target.position, _initialDesiredLocation + _target.position);
            Gizmos.DrawSphere(_initialDesiredLocation + _target.position, .5f);
        }
    }
#endif
}
