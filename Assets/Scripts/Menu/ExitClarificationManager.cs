using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitClarificationManager : MonoBehaviour
{
    private static bool s_instanceExists;
    
    [SerializeField]
    private GameObject _menu;
    [SerializeField]
    private Button _quitButton;
    [SerializeField]
    private Button _cancelButton;

    private bool _clarificationManagerActive;
    

    private void Awake()
    {
        s_instanceExists = true;
        _menu.SetActive(false);
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        Application.wantsToQuit += OnTryQuit;
        _quitButton.onClick.AddListener(() => Application.Quit());
        _cancelButton.onClick.AddListener(() => DisableClarificationManager());
    }
    private bool OnTryQuit()
    {
        if (_clarificationManagerActive)
        {
            return true;
        } else
        {
            _menu.SetActive(true);
            _clarificationManagerActive = true;
            return false; // Cancel the quit process
        }
    }
    private void DisableClarificationManager()
    {
        _menu.SetActive(false);
        _clarificationManagerActive = false;
    }


    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void CreateInstanceOnStart()
    {
        if (s_instanceExists) { return; }
        
    }
}