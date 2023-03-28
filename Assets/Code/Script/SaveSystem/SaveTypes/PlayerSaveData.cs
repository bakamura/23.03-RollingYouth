using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSaveData : MonoBehaviour, ISaveObject
{
    [SerializeField] private ObjectGrow _playerGrow;

    public void LoadData()
    {
        SaveData temp = SaveManager.Instance.LoadedData;
        _playerGrow.ObjectPhysics.transform.position = temp.PlayerPosition;
        _playerGrow.ObjectPhysics.mass = temp.PlayerMass;
        _playerGrow.ObjectToGrow.localScale = temp.PlayerScale;
    }

    public void UpdateSavedData()
    {
        if (_playerGrow)
        {
            SaveData newData = SaveManager.Instance.LoadedData;
            newData.PlayerPosition = _playerGrow.ObjectPhysics.transform.position;
            newData.PlayerScale = _playerGrow.ObjectToGrow.localScale;
            newData.PlayerMass = _playerGrow.ObjectPhysics.mass;
            SaveManager.Instance.UpdateCurrentData(newData);
        }
        else Debug.LogError("inspector reference is empty");
    }

#if UNITY_EDITOR
    [ContextMenu("Save")]
    private void Save()
    {
        UpdateSavedData();
    }

    [ContextMenu("Load")]
    private void Load()
    {
        LoadData();
    }
#endif
}
