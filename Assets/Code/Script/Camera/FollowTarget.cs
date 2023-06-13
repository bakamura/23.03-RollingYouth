using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] private Transform _playerPosition;
    [SerializeField] private Transform _cameraPosition;
    [SerializeField] private Transform _cameraRotation;
    [SerializeField] private Vector3 _initialDesiredLocation;
    [SerializeField] private Vector3 _cameraLookOffset;
    [SerializeField] private bool _cameraColideWithObjects = true;
    [SerializeField] private LayerMask _cameraColissionLayers;
#if UNITY_EDITOR
    [SerializeField] private bool _debugDraw;
    [SerializeField] private float _cameraTargetPointSize = .5f;
    [SerializeField] private Color _color;
#endif

    private Vector3 _cameraFocusPosition => _playerPosition.position + _cameraLookOffset;
    private Vector3 _currentCamPosition;
    private float _currentMaxDistance;
    private float _lastPlayerSize;

    private void Awake()
    {
        _playerPosition.GetComponentInParent<PlayerComponents>().ObjectGrow.OnObjectGrow += RecalculateCameraPosition;
        _currentCamPosition = _cameraPosition.localPosition;
        _currentMaxDistance = Vector3.Distance(_cameraFocusPosition, _cameraPosition.position);
        _lastPlayerSize = _playerPosition.localScale.sqrMagnitude;
    }

    private void Update()
    {
        _cameraRotation.position = _playerPosition.position;
        UpdateCameraLocation();
    }

    private void RecalculateCameraPosition()
    {
        //Debug.Log(_target.localScale.magnitude * -_initialForward);
        _currentCamPosition += (_playerPosition.localScale.sqrMagnitude - _lastPlayerSize) * -CalculateForwardVector()/*-_initialForward*/;
        _currentCamPosition = new Vector3(0, _currentCamPosition.y, _currentCamPosition.z);
        _currentMaxDistance = Vector3.Distance(_cameraFocusPosition, _cameraPosition.position);
        _lastPlayerSize = _playerPosition.localScale.sqrMagnitude;
    }

    private Vector3 CalculateForwardVector()
    {
        Vector3 temp = (_playerPosition.position - _cameraPosition.position).normalized;
        temp = new Vector3(0, temp.y, temp.z);
        return _cameraPosition.transform.TransformDirection(_cameraPosition.forward);
    }

    private void UpdateCameraLocation()
    {
        if (Physics.Raycast(_cameraFocusPosition, -_cameraPosition.forward, out RaycastHit hit, _currentMaxDistance, _cameraColissionLayers) && _cameraColideWithObjects)
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
        _cameraPosition.localPosition = /*_playerPosition.position + */_initialDesiredLocation;        
        _cameraPosition.LookAt(_playerPosition.position + _cameraLookOffset);
    }
    private void OnDrawGizmosSelected()
    {
        if (_debugDraw && _playerPosition)
        {
            Gizmos.color = Color.red;            
            Gizmos.DrawSphere(_cameraFocusPosition, _cameraTargetPointSize);
            Gizmos.color = _color;
            Gizmos.DrawLine(_cameraFocusPosition, _initialDesiredLocation + _playerPosition.position);
            Gizmos.DrawSphere(_initialDesiredLocation + _playerPosition.position, .5f);
            //Gizmos.color = Color.green;
            //Gizmos.DrawLine(_playerPosition.position, -_cameraPosition.forward * _currentMaxDistance) ;
        }
    }
#endif
}
