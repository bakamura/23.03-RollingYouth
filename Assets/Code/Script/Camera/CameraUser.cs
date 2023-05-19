using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(EntityStateChanger))]
public abstract class CameraUser : MonoBehaviour
{
    [Header("Camera Values")]
    [SerializeField] private float _cameraLerpDuration;
    [SerializeField] private AnimationCurve _cameraAnimationCurve;

    protected PlayerComponents _playerComponents;
    protected Vector3 _initialCameraPosition;
    protected Quaternion _initialCameraRotation;
    protected EntityStateChanger _entityStateChanger;
    private bool _isBegining = true;
    public bool IsBegining => _isBegining;

    protected virtual void Awake()
    {
        _entityStateChanger = GetComponent<EntityStateChanger>();
    }


    protected virtual void BeginCameraUseWithFixedPoints(PlayerComponents playerComponents, Vector3 targetCamPos, Quaternion targetCamRot)
    {
        _isBegining = true;
        CollectCameraData(playerComponents, playerComponents.CurrentCameraPosition.position, playerComponents.CurrentCameraPosition.rotation);
        LerpCamera.Instance.LerpCamWithFixedPoints(_cameraAnimationCurve, playerComponents.CurrentCameraPosition, targetCamPos, targetCamRot, _cameraLerpDuration, OnEndCameraLerp);
    }

    protected virtual void BeginCameraUseWithDynamicPoints(PlayerComponents playerComponents, Transform targetPositionAndRotation)
    {
        _isBegining = true;
        CollectCameraData(playerComponents, playerComponents.CurrentCameraPosition.position, playerComponents.CurrentCameraPosition.rotation);
        LerpCamera.Instance.LerpCamWithDynamicPoints(_cameraAnimationCurve, playerComponents.CurrentCameraPosition, targetPositionAndRotation, _cameraLerpDuration, OnEndCameraLerp);
    }

    protected virtual void ContinueCameraLerpWithFixedPoints(Vector3 targetCamPos, Quaternion targetCamRot)
    {
        LerpCamera.Instance.LerpCamWithFixedPoints(_cameraAnimationCurve, _playerComponents.CurrentCameraPosition, targetCamPos, targetCamRot, _cameraLerpDuration);
    }

    protected virtual void ContinueCameraLerpWithDynamicPoints(Transform targetPositionAndRotation)
    {
        LerpCamera.Instance.LerpCamWithDynamicPoints(_cameraAnimationCurve, _playerComponents.CurrentCameraPosition, targetPositionAndRotation, _cameraLerpDuration);
    }

    protected virtual void EndCameraUseWithFixedPoints()
    {
        _isBegining = false;
        LerpCamera.Instance.LerpCamWithFixedPoints(_cameraAnimationCurve, _playerComponents.CurrentCameraPosition, _initialCameraPosition, _initialCameraRotation, _cameraLerpDuration, OnEndCameraLerp);        
    }

    protected virtual void EndCameraUseWithDynamicPoints(Transform targetPositionAndRotation)
    {
        _isBegining = false;
        LerpCamera.Instance.LerpCamWithDynamicPoints(_cameraAnimationCurve, _playerComponents.CurrentCameraPosition, targetPositionAndRotation, _cameraLerpDuration, OnEndCameraLerp);
    }

    private void CollectCameraData(PlayerComponents playerComponents, Vector3 initialCamPos, Quaternion initialCamRot)
    {
        _playerComponents = playerComponents;
        _initialCameraPosition = initialCamPos;
        _initialCameraRotation = initialCamRot;
    }

    /// <summary>
    /// Will make the Entity go to the state defined in the EntityStateChanger attached to the CameraUser script, will always happend on the end of a CameraLerp
    /// </summary>
    private void UpdateEntityState()
    {
        _entityStateChanger.UpdateState(_playerComponents.PlayerActionsManagment);
        _playerComponents.CameraFollow.enabled = !_isBegining;
        _playerComponents.CameraRotate.enabled = !_isBegining;
    }

    protected virtual void OnEndCameraLerp()
    {
        UpdateEntityState();
    }
}
