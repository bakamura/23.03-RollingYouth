using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : BaseSingleton<SaveManager>
{
    [SerializeField] private bool _debugText;
    private const string _dataKey = "save";
    private SaveData _currentData;
#if UNITY_EDITOR
    [SerializeField] private bool _canLoadData;
#endif
    public SaveData LoadedData => _currentData;

    protected override void Awake()
    {
        base.Awake();
#if UNITY_EDITOR
        if (!_canLoadData) return;
#endif
        Load();
    }

    public void Save()
    {
        PlayerPrefs.SetString(_dataKey, DataSerializer.Serialize<SaveData>(_currentData));
    }

    public void Load()
    {
        if (PlayerPrefs.HasKey(_dataKey))
        {
            _currentData = DataSerializer.Deserialize<SaveData>(PlayerPrefs.GetString(_dataKey));
            if(_debugText) Debug.Log(DataSerializer.Serialize<SaveData>(_currentData));
        }
        else
        {
            _currentData = new SaveData();
            Save();
            if (_debugText) Debug.Log("save doesn't exist, creating a new save");
        }        
    }

    public void UpdateCurrentData(SaveData data)
    {
        _currentData = data;
    }

#if UNITY_EDITOR
    [ContextMenu("Load")]
    private void DebugLoad()
    {
        Load();
    }

    [ContextMenu("Save")]
    private void DebugSave()
    {
        Save();
    }

    [ContextMenu("EraseData")]
    private void EraseData()
    {
        PlayerPrefs.DeleteKey(_dataKey);
    }
#endif
}
public class SaveData
{
    public float PlayerMass;
    public Vector3 PlayerPosition = Vector3.one;
    public Vector3 PlayerScale = Vector3.back;
    public List<string> MemoryCollectables = new List<string>();
}
