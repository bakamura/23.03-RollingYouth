using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdRewardSizeIncrease : AdRewardBehaviour
{
    [SerializeField] private float _sizeIncrease;
    [SerializeField] private float _massIncrease;
    public override void ApplyReward()
    {
        PlayerReference.Instance.PlayerComponents.ObjectGrow.UpdateSize(_sizeIncrease, _massIncrease, false);
    }
}
