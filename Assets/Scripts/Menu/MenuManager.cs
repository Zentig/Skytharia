using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _settingsBG;
    [SerializeField] private GameObject _exitClarification;

    public void StartGame()
    {
        //SceneManager.LoadScene("PickLevel");
    }
    public void ExitGame() => Application.Quit();
    public void SettingsButtonClick() => _settingsBG.SetActive(!_settingsBG.activeInHierarchy);
    public void ShowExitClarification() => _exitClarification.SetActive(!_exitClarification.activeInHierarchy);
}
