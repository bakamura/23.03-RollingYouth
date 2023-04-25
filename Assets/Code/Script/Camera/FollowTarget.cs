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
#if UNITY_EDITOR
    [SerializeField] private bool _debugDraw;
    [SerializeField] private Color _color;
#endif

    private Vector3 _cameraFocusPosition => _playerPosition.position + _cameraLookOffset;
    private Vector3 _initialForward;
    private Vector3 _currentCamPosition;
    private float _currentMaxDistance;

    private void Awake()
    {
        _playerPosition.GetComponentInParent<PlayerComponents>().ObjectGrow.OnObjectGrow += RecalculateCameraPosition;
        _initialForward = new Vector3(0, _cameraPosition.forward.y, _cameraPosition.forward.z);
        //_initialForward = _cameraPosition.forward;
        _currentCamPosition = _cameraPosition.localPosition;
        _currentMaxDistance = Vector3.Distance(_cameraFocusPosition, _cameraPosition.position);
    }

    private void Update()
    {
        _cameraRotation.position = _cameraFocusPosition;
        //UpdateCameraLocation();
    }

    private void RecalculateCameraPosition()
    {
        //Debug.Log(_target.localScale.magnitude * -_initialForward);
        _currentCamPosition += _playerPosition.localScale.magnitude * -_initialForward;
        _currentMaxDistance = Vector3.Distance(_cameraFocusPosition, _cameraPosition.position);
    }

    private void UpdateCameraLocation()
    {
        if (Physics.Raycast(_cameraFocusPosition, -_cameraPosition.forward, out RaycastHit hit, _currentMaxDistance))
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
        _cameraPosition.position = _playerPosition.position + _initialDesiredLocation;
        _cameraPosition.LookAt(_playerPosition.position + _cameraLookOffset);
    }
    private void OnDrawGizmosSelected()
    {
        if (_debugDraw && _playerPosition)
        {
            Gizmos.color = Color.red;            
            Gizmos.DrawSphere(_cameraFocusPosition, .5f);
            Gizmos.color = _color;
            Gizmos.DrawLine(_cameraFocusPosition, _initialDesiredLocation + _playerPosition.position);
            Gizmos.DrawSphere(_initialDesiredLocation + _playerPosition.position, .5f);
            //Gizmos.color = Color.green;
            //Gizmos.DrawLine(_playerPosition.position, -_cameraPosition.forward * _currentMaxDistance) ;
        }
    }
#endif
}
