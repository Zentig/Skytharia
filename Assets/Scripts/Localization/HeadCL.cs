using Newtonsoft.Json;
using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using Skytharia.SaveManagement;

public class HeadCL : MonoBehaviour, IHasPersistentData
{
    public static HeadCL Instance;
    public event Action<LocalizationTypes> OnLanguageChanged;
    private LocalizationTypes _language;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void OnLoadGame()
    {
        var settingsConfig = SettingsConfig.GetInstance();
        SetLanguage(Enum.Parse<LocalizationTypes>(settingsConfig.Language));
    }
    public void OnSaveGame()
    {
        return;
    }
    public void SetLanguage(LocalizationTypes language)
    {
        _language = language;

        var settingsConfig = SettingsConfig.GetInstance();
        settingsConfig.Language = language.ToString();
        SettingsConfig.SaveInstance(settingsConfig);

        Debug.Log(settingsConfig.Language);
        OnLanguageChanged?.Invoke(_language);
    }
}
