using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _settingsBG;

    public void StartGame()
    {
        //SceneManager.LoadScene("PickLevel");
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void SettingsButtonClick() => _settingsBG.SetActive(!_settingsBG.activeInHierarchy);
}
