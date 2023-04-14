using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] private Transform _target;
    [SerializeField] private Transform _cameraPosition;
    [SerializeField] private Transform _cameraRotation;
    [SerializeField] private Vector3 _initialDesiredLocation;
#if UNITY_EDITOR
    [SerializeField] private bool _debugDraw;
    [SerializeField] private Color _color;
    [SerializeField] private Vector3 _cameraLookOffset;
#endif

    private Vector3 _targetPosition => _target.position + _cameraLookOffset;
    private Vector3 _initialForward;
    private Vector3 _currentCamPosition;
    private float _currentMaxDistance;

    private void Awake()
    {
        _target.GetComponentInParent<PlayerComponents>().ObjectGrow.OnObjectGrow += RecalculateCameraPosition;
        _initialForward = _cameraPosition.forward;
        _currentCamPosition = _cameraPosition.localPosition;
        _currentMaxDistance = Vector3.Distance(_targetPosition, _cameraPosition.position);
    }

    private void Update()
    {
        _cameraRotation.position = _targetPosition;
        UpdateCameraLocation();
    }

    private void RecalculateCameraPosition()
    {
        _currentCamPosition += _target.localScale.magnitude * -_initialForward;
        _currentMaxDistance = Vector3.Distance(_targetPosition, _cameraPosition.position);
    }

    private void UpdateCameraLocation()
    {
        if (Physics.Raycast(_targetPosition, -_cameraPosition.forward, out RaycastHit hit, _currentMaxDistance))
        {
            _cameraPosition.position = hit.point;
        }
        else
        {
            _cameraPosition.localPosition = _currentCamPosition;
        }
    }
    
#if UNITY_EDITOR
    public void RepositionCam()
    {
        _cameraPosition.position = _target.position + _initialDesiredLocation;
        _cameraPosition.LookAt(_target.position + _cameraLookOffset);
    }
    private void OnDrawGizmosSelected()
    {
        if (_debugDraw && _target)
        {
            Gizmos.color = Color.red;            
            Gizmos.DrawSphere(_targetPosition, .5f);
            Gizmos.color = _color;
            Gizmos.DrawLine(_targetPosition, _initialDesiredLocation + _target.position);
            Gizmos.DrawSphere(_initialDesiredLocation + _target.position, .5f);
        }
    }
#endif
}
