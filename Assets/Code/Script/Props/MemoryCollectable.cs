using UnityEngine;

public class MemoryCollectable : MonoBehaviour, ISaveObject
{
    [SerializeField] private string _memoryID;

    private void Start()
    {
        if(SaveManager.Instance.LoadedData != null) LoadData();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) Collect();
    }

    private void Collect()
    {
        UpdateSavedData();
        Destroy(gameObject);
    }

    public void UpdateSavedData()
    {
        SaveData newData = SaveManager.Instance.LoadedData;
        if (!newData.MemoryCollectables.Contains(_memoryID)) newData.MemoryCollectables.Add(_memoryID);
        else Debug.LogWarning($"the object with the ID {_memoryID} is already registered, change the ID for the GameObject {gameObject.name}");
        SaveManager.Instance.UpdateCurrentData(newData);
    }

    public void LoadData()
    {
        SaveData temp = SaveManager.Instance.LoadedData;
        if (temp.MemoryCollectables.Contains(_memoryID)) Destroy(gameObject);
    }
}
