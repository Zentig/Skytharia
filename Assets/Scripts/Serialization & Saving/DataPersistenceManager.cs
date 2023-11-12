using System.Linq;
using UnityEngine;

public class DataPersistenceManager : MonoBehaviour
{
    private static bool s_instanceExists;

    private void Awake()
    {
        s_instanceExists = true;
        IHasPersistentData[] objectsWithPersistentData = FindObjectsOfType<MonoBehaviour>().OfType<IHasPersistentData>().ToArray();
        foreach (var toCall in objectsWithPersistentData) 
        {
            toCall.OnLoadGame();
        } 
        Application.quitting += OnQuit;
    }
    private void OnQuit()
    {
        IHasPersistentData[] objectsWithPersistentData = FindObjectsOfType<MonoBehaviour>().OfType<IHasPersistentData>().ToArray();
        foreach (var toCall in objectsWithPersistentData) 
        {
            toCall.OnSaveGame();
        } 
    }
    private void OnDestroy()
    {
        s_instanceExists = false;
    }
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void CreateInstanceOnStart()
    {
        if (s_instanceExists)
        {
            return;
        }
        GameObject o = new("Data Persistence Manager");
        o.AddComponent<DataPersistenceManager>();
        s_instanceExists = true;
    }
}
