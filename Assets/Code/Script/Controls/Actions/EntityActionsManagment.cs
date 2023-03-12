using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityActionsManagment : MonoBehaviour
{
    [SerializeField] private float _decisionsTick;
    [SerializeField] private float _actionsTick;
    [SerializeField] private BaseDecisions[] _decisions;
    [SerializeField] private BaseActions[] _actions;
    private WaitForSeconds _actionsDelay;
    private WaitForSeconds _decisionsDelay;
    private bool _isActionsPossible;
    private Coroutine _actionsCoroutine;

    private void Awake()
    {
        _actionsDelay = new WaitForSeconds(_actionsTick);
        _decisionsDelay = new WaitForSeconds(_decisionsTick);
    }
    private void OnEnable()
    {
        if (_decisions.Length > 0)
        {
            StartCoroutine(DecisionsCoroutine());
        }
        else
        {
            _actionsCoroutine = StartCoroutine(ActionsCoroutine());
            Debug.Log($"the Entity {name} doesn't have any decisions");
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        _actionsCoroutine = null;
    }
    IEnumerator ActionsCoroutine()
    {
        while (_isActionsPossible)
        {
            for (int i = 0; i < _actions.Length; i++)
            {
                _actions[i].ExecuteAction();
            }
            yield return _actionsDelay;
        }
        _actionsCoroutine = null;
    }

    IEnumerator DecisionsCoroutine()
    {
        while (true)
        {
            byte decisionsAmount = 0;
            for (int i = 0; i < _decisions.Length; i++)
            {
                if (_decisions[i].CheckDecision()) decisionsAmount++;
            }
            _isActionsPossible = decisionsAmount == _decisions.Length;
            if (_isActionsPossible && _actionsCoroutine == null) _actionsCoroutine = StartCoroutine(ActionsCoroutine());
            yield return _decisionsDelay;
        }
    }
}
