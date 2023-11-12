using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHasPersistentData
{
    public void OnLoadGame();
    public void OnSaveGame();
}