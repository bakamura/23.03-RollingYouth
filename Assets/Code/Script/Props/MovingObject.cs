using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    [SerializeField] private Vector3[] _points;
    [SerializeField] private float _tickFrequency;
    [SerializeField] private float _speed = 1f;
    [SerializeField] private bool _willStartActive;

    private Coroutine _movmentCoroutine;
    private WaitForSeconds _delay;
    private Vector3 _initialPosition;

#if UNITY_EDITOR
    [SerializeField] private float _pointsSize;
    [SerializeField] private Color _pointColor = Color.red;
    [SerializeField] private Color _lineColor = Color.black;
#endif

    private void Awake()
    {
        _initialPosition = transform.position;
        _delay = new WaitForSeconds(_tickFrequency);
    }

    private void OnEnable()
    {
        if (_willStartActive) StartMovment();
    }

    private void OnDisable()
    {
        StopMovment();
    }

    private IEnumerator Move()
    {
        float delta = 0;
        byte currentTargetPosition = 0;
        Vector3 currentPosition = transform.position;
        while (true)
        {
            if(Vector3.Equals(transform.position, _points[currentTargetPosition] + _initialPosition))
            {
                delta = 0;
                currentTargetPosition = (byte)(currentTargetPosition == _points.Length - 1 ? 0 : currentTargetPosition + 1);
                currentPosition = transform.position;
            }
            else
            {
                transform.position = Vector3.Lerp(currentPosition, _points[currentTargetPosition] + _initialPosition, delta);
                delta += _tickFrequency * _speed;
            }
            yield return _delay;
        }
    }

    public void StartMovment()
    {
        if (_movmentCoroutine == null) _movmentCoroutine = StartCoroutine(Move());
    }

    public void StopMovment()
    {
        if (_movmentCoroutine != null)
        {
            StopCoroutine(Move());
            _movmentCoroutine = null;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Vector3 pos = UnityEditor.EditorApplication.isPlaying ? _initialPosition : transform.position;
        for(int i = 0; i < _points.Length; i++)
        {
            Gizmos.color = _pointColor;
            Gizmos.DrawSphere(pos + _points[i], _pointsSize);
            Gizmos.color = _lineColor;
            Gizmos.DrawLine(pos + _points[i], pos + _points[i + 1 == _points.Length ? 0 : i + 1]);
        }
    }
#endif
}
