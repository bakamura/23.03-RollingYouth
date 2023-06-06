using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugLevelSelection : MonoBehaviour
{
    [SerializeField] private CanvasGroup _levelsPanel;
    [SerializeField] private GameObject _buttonPrefab;
    [SerializeField] private LevelButtonSelectionInfo[] _levelsInfo;

    [System.Serializable]
    public struct LevelButtonSelectionInfo
    {
        //public string SceneName;
        public string LevelDisplayName;
        public Vector3 PlayerPosition;
    }

    private void Awake()
    {
        for (int i = 0; i < _levelsInfo.Length; i++)
        {
            Instantiate(_buttonPrefab, _levelsPanel.transform).GetComponent<LevelSelectionButton>().SetupButton(_levelsInfo[i]);
        }
    }

    public void ToggleLevelSelectionMenu()
    {
        _levelsPanel.interactable = !_levelsPanel.interactable;
        _levelsPanel.blocksRaycasts = !_levelsPanel.blocksRaycasts;
        _levelsPanel.alpha = _levelsPanel.alpha == 0 ? 1f : 0f;
    }
}
