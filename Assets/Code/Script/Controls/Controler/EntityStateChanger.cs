using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityStateChanger : MonoBehaviour
{
    [SerializeField] private string _stateID;

    public void UpdateState(EntityActionsManagment manager)
    {
        manager.LookForState(_stateID);
    }
}
