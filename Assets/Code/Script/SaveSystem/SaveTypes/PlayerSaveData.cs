using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSaveData : MonoBehaviour, ISaveObject
{
    [SerializeField] private PlayerComponents _playerComponents;

    //private void Awake()
    //{
    //    LoadData();
    //}

    public void LoadData()
    {
        SaveData temp = SaveManager.Instance.LoadedData;
        _playerComponents.PlayerTransform.position = temp.PlayerPosition;
        _playerComponents.PlayerRigidbody.mass = temp.PlayerMass;
        _playerComponents.ObjectGrow.ObjectToGrow.localScale = temp.PlayerScale;
    }

    public void UpdateSavedData()
    {
        if (_playerComponents)
        {
            SaveData newData = SaveManager.Instance.LoadedData;
            newData.PlayerPosition = _playerComponents.PlayerTransform.position;
            newData.PlayerScale = _playerComponents.ObjectGrow.ObjectToGrow.localScale;
            newData.PlayerMass = _playerComponents.PlayerRigidbody.mass;
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
