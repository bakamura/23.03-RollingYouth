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
    //[SerializeField] private RotateByTouch _rotateByTouch;
#if UNITY_EDITOR
    [SerializeField] private bool _debugDraw;
    [SerializeField] private Color _color;
#endif
    private void Update()
    {
        RecalculateCameraPosition();
    }

    public void RecalculateCameraPosition()
    {
        Vector3 pos = _target.position + _initialDesiredLocation + _target.localScale.magnitude * -_cameraPosition.forward;
        _cameraPosition.position = pos;
    }
    
#if UNITY_EDITOR
    public void RepositionCam()
    {
        _cameraPosition.position = _target.position + _initialDesiredLocation;
        _cameraPosition.LookAt(_target);
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
