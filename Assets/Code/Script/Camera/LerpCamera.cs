using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LerpCamera : BaseSingleton<LerpCamera>
{
    //private Transform _targetTransform;
    //private Vector3 _finalFixedPosition;
    //private Quaternion _finalFixedRotation;
    //private float _lerpDuration;
    //private Vector3 _initialPosition;
    //private Quaternion _initialRotation;
    //private Transform _finalDynamicPositionAndRotation;
    //private Action _onEndLerp;
    //private AnimationCurve _animationCurve;
    //private Vector3 _lerpDynamicOffset;
    private WaitForSeconds _delay;
    private const float _tickFrequence = .02f;
    private bool _isAnimating;

    public bool IsAnimating => _isAnimating;

    protected override void Awake()
    {
        base.Awake();
        _delay = new WaitForSeconds(_tickFrequence);
    }

    public void LerpCamWithFixedPoints(AnimationCurve animationCurve, Transform camera, Vector3 finalPosition, Quaternion finalRotation, float speed, Action OnEndLerp = null)
    {
        if (!_isAnimating)
        {
            //_animationCurve = animationCurve;
            //_targetTransform = camera;
            //_finalFixedPosition = finalPosition;
            //_finalFixedRotation = finalRotation;
            //_initialPosition = camera.position;
            //_initialRotation = camera.rotation;
            //_lerpDuration = speed;
            //_onEndLerp = null;
            //_onEndLerp += OnEndLerp;
            _isAnimating = true;
            StartCoroutine(LerpWithFixedPointsCoroutine(animationCurve, camera, finalPosition, finalRotation, speed, OnEndLerp));
        }
    }

    public void LerpCamWithDynamicPoints(AnimationCurve animationCurve, Transform camera, Transform finalPosition, Transform finalRotation, float speed, Vector3 extraPosOffset, Action OnEndLerp = null)
    {
        if (!_isAnimating)
        {
            //_animationCurve = animationCurve;
            //_targetTransform = camera;
            //_finalDynamicPositionAndRotation = finalPositionAndRotation;
            //_initialPosition = camera.position;
            //_initialRotation = camera.rotation;
            //_lerpDynamicOffset = extraOffset;
            //_lerpDuration = speed;
            //_onEndLerp = null;
            //_onEndLerp += OnEndLerp;
            _isAnimating = true;
            StartCoroutine(LerpWithDynamicPoints(animationCurve, camera, finalPosition, finalRotation, speed, extraPosOffset, OnEndLerp));
        }
    }

    private IEnumerator LerpWithFixedPointsCoroutine(AnimationCurve animationCurve, Transform camera, Vector3 finalPosition, Quaternion finalRotation, float speed, Action OnEndLerp = null)
    {
        float delta = 0;
        Vector3 initialPos = camera.position;
        Quaternion initialRot = camera.rotation;
        while (delta < 1)
        {
            delta += _tickFrequence / speed;
            camera.SetPositionAndRotation(Vector3.Lerp(initialPos, finalPosition, animationCurve.Evaluate(delta)), Quaternion.Lerp(initialRot, finalRotation, animationCurve.Evaluate(delta)));
            yield return _delay;
        }
        _isAnimating = false;
        OnEndLerp?.Invoke();
    }

    private IEnumerator LerpWithDynamicPoints(AnimationCurve animationCurve, Transform camera, Transform finalPosition, Transform finalRotation, float speed, Vector3 extraPosOffset, Action OnEndLerp = null)
    {
        float delta = 0;
        Vector3 initialPos = camera.position;
        Quaternion initialRot = camera.rotation;
        while (delta < 1)
        {
            delta += _tickFrequence / speed;
            camera.SetPositionAndRotation(Vector3.Lerp(initialPos, finalPosition.position + extraPosOffset, animationCurve.Evaluate(delta)), Quaternion.Lerp(initialRot, finalRotation.rotation, animationCurve.Evaluate(delta)));
            yield return _delay;
        }
        _isAnimating = false;
        OnEndLerp?.Invoke();
    }
}
