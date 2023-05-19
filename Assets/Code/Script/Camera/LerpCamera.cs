using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LerpCamera : BaseSingleton<LerpCamera>
{
    private Transform _targetTransform;
    private Vector3 _finalFixedPosition;
    private Quaternion _finalFixedRotation;
    private float _lerpDuration;
    private Vector3 _initialPosition;
    private Quaternion _initialRotation;
    private Transform _finalDynamicPositionAndRotation;
    private bool _isAnimating;
    private Action _onEndLerp;
    private const float _tickFrequence = .02f;
    private WaitForSeconds _delay;
    private AnimationCurve _animationCurve;

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
            _animationCurve = animationCurve;
            _targetTransform = camera;
            _finalFixedPosition = finalPosition;
            _finalFixedRotation = finalRotation;
            _initialPosition = camera.position;
            _initialRotation = camera.rotation;
            _lerpDuration = speed;
            _onEndLerp = null;
            _onEndLerp += OnEndLerp;
            _isAnimating = true;
            StartCoroutine(LerpWithFixedPointsCoroutine());
        }
    }

    public void LerpCamWithDynamicPoints(AnimationCurve animationCurve, Transform camera, Transform finalPositionAndRotation, float speed, Action OnEndLerp = null)
    {
        if (!_isAnimating)
        {
            _animationCurve = animationCurve;
            _targetTransform = camera;
            _finalDynamicPositionAndRotation = finalPositionAndRotation;
            _initialPosition = camera.position;
            _initialRotation = camera.rotation;
            _lerpDuration = speed;
            _onEndLerp = null;
            _onEndLerp += OnEndLerp;
            _isAnimating = true;
            StartCoroutine(LerpWithDynamicPoints());
        }
    }

    private IEnumerator LerpWithFixedPointsCoroutine()
    {
        float delta = 0;
        while (delta < 1)
        {
            delta += _tickFrequence / _lerpDuration;
            _targetTransform.SetPositionAndRotation(Vector3.Lerp(_initialPosition, _finalFixedPosition, _animationCurve.Evaluate(delta)), Quaternion.Lerp(_initialRotation, _finalFixedRotation, _animationCurve.Evaluate(delta)));
            yield return _delay;
        }
        _isAnimating = false;
        _onEndLerp?.Invoke();
    }

    private IEnumerator LerpWithDynamicPoints()
    {
        float delta = 0;
        while (delta < 1)
        {
            delta += _tickFrequence / _lerpDuration;
            _targetTransform.SetPositionAndRotation(Vector3.Lerp(_initialPosition, _finalDynamicPositionAndRotation.position, _animationCurve.Evaluate(delta)), Quaternion.Lerp(_initialRotation, _finalDynamicPositionAndRotation.rotation, _animationCurve.Evaluate(delta)));
            yield return _delay;
        }
        _isAnimating = false;
        _onEndLerp?.Invoke();
    }
}
