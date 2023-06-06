using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LevelSelectionButton : MonoBehaviour
{
    private DebugLevelSelection.LevelButtonSelectionInfo _buttonInfo;
    private Button _button { get { return GetComponent<Button>(); } }

    public void SetupButton(DebugLevelSelection.LevelButtonSelectionInfo info)
    {
        _buttonInfo = info;
        _button.GetComponentInChildren<Text>().text = info.LevelDisplayName;
        _button.onClick.AddListener(GoToLevel);
    }

    private void GoToLevel()
    {
        PlayerReference.Instance.PlayerComponents.PlayerRigidbody.angularVelocity = Vector3.zero;
        PlayerReference.Instance.PlayerComponents.PlayerRigidbody.velocity = Vector3.zero;
        PlayerReference.Instance.PlayerComponents.PlayerTransform.position = _buttonInfo.PlayerPosition;
    }
}
