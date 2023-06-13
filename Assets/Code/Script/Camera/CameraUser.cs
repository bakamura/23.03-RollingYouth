using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class CameraUser : MonoBehaviour
{
    [Header("Camera Values")]
    [SerializeField] private float _baseCameraLerpDuration;
    [SerializeField] private AnimationCurve _cameraAnimationCurve;

    [Header("EntityManagment")]
    [SerializeField] private EntityStateChanger _stateOnBegining;
    [SerializeField] private EntityStateChanger _stateOnEnd;

    protected PlayerComponents _playerComponents;
    protected Vector3 _initialCameraPosition;
    protected Quaternion _initialCameraRotation;
    protected bool _useLocalPosition;
    protected bool _useLocalRotation;
    private bool _isBegining = true;
    public bool IsBegining => _isBegining;

    protected virtual void Awake()
    {
        
    }

    protected virtual void BeginCameraUseWithFixedPoints(PlayerComponents playerComponents, Transform cameraTransform, Vector3 targetCamPos, Quaternion targetCamRot, bool applytoLocalPosition, bool applyToLocalRotation, float duration)
    {
        _isBegining = true;
        CollectCameraData(playerComponents, cameraTransform.position, cameraTransform.rotation, applytoLocalPosition, applyToLocalRotation);
        _playerComponents.PlayerUI.ToggleControlUI(true);
        UpdateEntityState(_stateOnBegining);
        LerpCamera.Instance.LerpCamWithFixedPoints(_cameraAnimationCurve, playerComponents.CameraPosition, targetCamPos, targetCamRot, duration > 0 ? duration : _baseCameraLerpDuration, applytoLocalPosition, applyToLocalRotation);
    }

    protected virtual void BeginCameraUseWithDynamicPoints(PlayerComponents playerComponents, Transform cameraTransform, Transform targetPosition, Quaternion targetRotation, Vector3 extraPosOffset, bool applytoLocalPosition, bool applyToLocalRotation, float duration)
    {
        _isBegining = true;
        CollectCameraData(playerComponents, cameraTransform.position, cameraTransform.rotation, applytoLocalPosition, applyToLocalRotation);
        _playerComponents.PlayerUI.ToggleControlUI(true);
        UpdateEntityState(_stateOnBegining);
        LerpCamera.Instance.LerpCamWithDynamicPoints(_cameraAnimationCurve, cameraTransform, targetPosition, targetRotation, duration > 0 ? duration : _baseCameraLerpDuration, extraPosOffset, applytoLocalPosition, applyToLocalRotation);
    }

    protected virtual void ContinueCameraLerpWithFixedPoints(Vector3 targetCamPos, Quaternion targetCamRot, float duration)
    {
        LerpCamera.Instance.LerpCamWithFixedPoints(_cameraAnimationCurve, _playerComponents.CameraPosition, targetCamPos, targetCamRot, duration > 0 ? duration : _baseCameraLerpDuration, _useLocalPosition, _useLocalRotation);
    }

    protected virtual void ContinueCameraLerpWithDynamicPoints(Transform targetPosition, Quaternion targetRotation, Vector3 extraPosOffset, float duration)
    {
        LerpCamera.Instance.LerpCamWithDynamicPoints(_cameraAnimationCurve, _playerComponents.CameraPosition, targetPosition, targetRotation, duration > 0 ? duration : _baseCameraLerpDuration, extraPosOffset, _useLocalPosition, _useLocalRotation);
    }

    protected virtual void EndCameraUseWithFixedPoints(float duration)
    {
        _isBegining = false;
        LerpCamera.Instance.LerpCamWithFixedPoints(_cameraAnimationCurve, _playerComponents.CameraPosition, _initialCameraPosition, _initialCameraRotation, duration > 0 ? duration : _baseCameraLerpDuration, _useLocalPosition, _useLocalRotation, () => OnEndCameraLerp(_stateOnEnd));        
    }

    protected virtual void EndCameraUseWithDynamicPoints(Transform targetPosition, Quaternion targetRotation, Vector3 extraPosOffset, float duration)
    {
        _isBegining = false;
        LerpCamera.Instance.LerpCamWithDynamicPoints(_cameraAnimationCurve, _playerComponents.CameraPosition, targetPosition, targetRotation, duration > 0 ? duration : _baseCameraLerpDuration, extraPosOffset, _useLocalPosition, _useLocalRotation, () => OnEndCameraLerp(_stateOnEnd));
    }

    private void CollectCameraData(PlayerComponents playerComponents, Vector3 initialCamPos, Quaternion initialCamRot, bool applytoLocalPosition, bool applyToLocalRotation)
    {
        _playerComponents = playerComponents;
        _initialCameraPosition = initialCamPos;
        _initialCameraRotation = initialCamRot;
        _useLocalPosition = applytoLocalPosition;
        _useLocalRotation = applyToLocalRotation;
    }

    /// <summary>
    /// Will make the Entity go to the state defined in the EntityStateChanger attached to the CameraUser script, will always happend on the end of a CameraLerp
    /// </summary>
    private void UpdateEntityState(EntityStateChanger currentState)
    {
        currentState.UpdateState(_playerComponents.PlayerActionsManagment);
        //_entityStateChanger.UpdateState(_playerComponents.PlayerActionsManagment);
        _playerComponents.CameraFollow.enabled = !_isBegining;
        _playerComponents.CameraRotate.enabled = !_isBegining;
        _playerComponents.PlayerUI.ToggleControlUI(!_isBegining);
    }

    protected virtual void OnEndCameraLerp(EntityStateChanger currentState)
    {
        UpdateEntityState(currentState);
    }
}
