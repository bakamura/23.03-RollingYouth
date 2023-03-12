using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckGround : BaseDecisions
{
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField, Tooltip("Uses the collider to calculate distance from object to ground")] private Collider _objectCollider;
    [SerializeField, Range(0f, 2f)] private float _rangeCorrection = 1f;
    [SerializeField] private Vector3 _boxDimensions;
    [SerializeField, Tooltip("If null will use its own transform")] private Transform _origin;
#if UNITY_EDITOR
    [SerializeField] private bool _debugDraw;
    [SerializeField] private Color _debugColor;
#endif

    private void Awake()
    {
        if (!_origin) _origin = transform;
    }

    private bool CheckForGround()
    {
        return Physics.BoxCast(_origin.position, _boxDimensions / 2f, -Vector3.up, Quaternion.identity, _objectCollider.bounds.extents.magnitude * _rangeCorrection, _groundLayer);
    }

    public override bool CheckDecision()
    {
        return CheckForGround();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (_debugDraw)
        {
            Transform origin = _origin ? _origin : transform; 
            Gizmos.color = _debugColor;
            Gizmos.DrawWireCube(origin.position + _objectCollider.bounds.extents.magnitude * _rangeCorrection * -origin.up, _boxDimensions);
        }
    }
#endif
}
