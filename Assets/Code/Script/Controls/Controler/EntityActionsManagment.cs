using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityActionsManagment : MonoBehaviour
{
    [SerializeField] private float _decisionsTick;
    [SerializeField] private float _actionsTick;
    [SerializeField, Tooltip("the first state in the list will be the initial state")] private States[] _behaviourList;

    private WaitForSeconds _actionsDelay;
    private WaitForSeconds _decisionsDelay;
    private States _currentState;
    private Coroutine _decisionsCoroutine;
    private Coroutine _actionsCoroutine;

    [System.Serializable]
    private struct States
    {
        public string StateID;
        public BaseDecisions[] Decisions;
        public BaseActions[] Actions;
        public string StateToGoWhenTrue;
        public string StateToGoWhenFalse;
    }

    private void Awake()
    {
        _actionsDelay = new WaitForSeconds(_actionsTick);
        _decisionsDelay = new WaitForSeconds(_decisionsTick);
    }
    private void OnEnable()
    {
        if (_behaviourList.Length > 0)
        {
            _currentState = _behaviourList[0];
            if(_currentState.Decisions.Length > 0)_decisionsCoroutine = StartCoroutine(DecisionsCoroutine());
            if (_currentState.Actions.Length > 0) _actionsCoroutine = StartCoroutine(ActionsCoroutine());
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void LookForState(bool decisionResult)
    {
        string newStateID = decisionResult ? _currentState.StateToGoWhenTrue : _currentState.StateToGoWhenFalse;
        if (!string.IsNullOrEmpty(newStateID))
        {
            for (int i = 0; i < _behaviourList.Length; i++)
            {
                if (_behaviourList[i].StateID == newStateID)
                {
                    _currentState = _behaviourList[i];
                    if (_currentState.Decisions.Length > 0 && _decisionsCoroutine == null) _decisionsCoroutine = StartCoroutine(DecisionsCoroutine());
                    if (_currentState.Actions.Length > 0 && _actionsCoroutine == null) _actionsCoroutine = StartCoroutine(ActionsCoroutine());
                    break;
                }
            }
        }
    }

    public void LookForState(string stateID)
    {
        for (int i = 0; i < _behaviourList.Length; i++)
        {
            if (_behaviourList[i].StateID == stateID)
            {
                _currentState = _behaviourList[i];
                if (_currentState.Decisions.Length > 0 && _decisionsCoroutine == null) _decisionsCoroutine = StartCoroutine(DecisionsCoroutine());
                if (_currentState.Actions.Length > 0 && _actionsCoroutine == null) _actionsCoroutine = StartCoroutine(ActionsCoroutine());
                break;
            }
        }
    }

    IEnumerator ActionsCoroutine()
    {
        while (_currentState.Actions.Length > 0)
        {
            for (int i = 0; i < _currentState.Actions.Length; i++)
            {
                _currentState.Actions[i].ExecuteAction();
            }
            yield return _actionsDelay;
        }
        _actionsCoroutine = null;
    }

    IEnumerator DecisionsCoroutine()
    {
        while (_currentState.Decisions.Length > 0)
        {
            byte decisionsAmount = 0;
            for (int i = 0; i < _currentState.Decisions.Length; i++)
            {
                if (_currentState.Decisions[i].CheckDecision()) decisionsAmount++;
            }
            LookForState(decisionsAmount == _currentState.Decisions.Length);
            yield return _decisionsDelay;
        }
        _decisionsCoroutine = null;
    }
}
