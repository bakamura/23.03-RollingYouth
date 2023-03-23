/// <summary>
/// when using this interface make sure the LoadData occurs after the SaveManager did its Load method 
/// </summary>
public interface ISaveObject
{
    void UpdateSavedData();

    void LoadData();
}