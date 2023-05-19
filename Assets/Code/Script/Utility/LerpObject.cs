using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LerpObject : BaseSingleton<LerpObject>
{
    private WaitForSeconds _delay;
    private const float _tickFrequence = .02f;
    private Coroutine _lerpPositionCoroutine;
    private Coroutine _slerpPositionCoroutine;

    protected override void Awake()
    {
        base.Awake();
        _delay = new WaitForSeconds(_tickFrequence);
    }
    public void LerpObjectPosition(Transform target, float duration, Vector3 finalPosition, Action onLerpEnd)
    {
        if (_lerpPositionCoroutine == null) _lerpPositionCoroutine = StartCoroutine(LerpCoroutine(target, duration, finalPosition, onLerpEnd));
    }

    public void SlerpObjectPosition(Transform target, float duration, Vector3 finalPosition, Action onLerpEnd)
    {
        if (_slerpPositionCoroutine == null) _slerpPositionCoroutine = StartCoroutine(SlerpCoroutine(target, duration, finalPosition, onLerpEnd));
    }

    private IEnumerator LerpCoroutine(Transform target, float duration, Vector3 finalPosition, Action onLerpEnd)
    {
        float delta = 0;
        Vector3 initialPosition = target.position;
        while (delta < 1)
        {
            target.position = Vector3.Lerp(initialPosition, finalPosition, delta);
            delta += _tickFrequence / duration;
            yield return _delay;
        }
        onLerpEnd?.Invoke();
        _lerpPositionCoroutine = null;
    }

    private IEnumerator SlerpCoroutine(Transform target, float duration, Vector3 finalPosition, Action onLerpEnd)
    {
        float delta = 0;
        Vector3 initialPosition = target.position;
        while (delta < 1)
        {
            target.position = Vector3.Slerp(initialPosition, finalPosition, delta);
            delta += _tickFrequence / duration;
            yield return _delay;
        }
        onLerpEnd?.Invoke();
        _slerpPositionCoroutine = null;
    }
}
