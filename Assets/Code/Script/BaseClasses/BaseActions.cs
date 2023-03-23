using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseActions : MonoBehaviour
{
    [SerializeField, Range(0f, 50f)] protected float _sensitivity;

    protected Transform _currentTarget;
    protected Rigidbody _rb;

    public virtual void SetTarget(Transform target, Rigidbody rb = null)
    {
        _currentTarget = target;
        _rb = rb;
    }

    public abstract void ExecuteAction();
}
