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
#endif

    private Vector3 _initialForward;

    private void Awake()
    {
        _target.GetComponentInParent<PlayerComponents>().ObjectGrow.OnObjectGrow += RecalculateCameraPosition;
        _initialForward = _cameraPosition.forward;
    }

    private void Update()
    {
        _cameraRotation.position = _target.position;
    }

    private void RecalculateCameraPosition()
    {
        _cameraPosition.localPosition += _target.localScale.magnitude * -_initialForward;
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
