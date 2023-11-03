using Newtonsoft.Json;
using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class HeadCL : MonoBehaviour
{
    public static HeadCL Instance;
    public event Action<LocalizationTypes> OnLanguageChanged;

    private string _settingsConfigPath = Application.streamingAssetsPath + "/SettingsConfig.json";
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
    private void Start()
    {
        var settingsConfig = JsonConvert.DeserializeObject<SettingsConfig>(File.ReadAllText(_settingsConfigPath, Encoding.UTF8));
        SetLanguage(Enum.Parse<LocalizationTypes>(settingsConfig.Language));
    }
    public void SetLanguage(LocalizationTypes language)
    {
        _language = language;

        var settingsConfig = JsonConvert.DeserializeObject<SettingsConfig>(File.ReadAllText(_settingsConfigPath, Encoding.UTF8));
        settingsConfig.Language = language.ToString();
        File.WriteAllText(_settingsConfigPath, JsonConvert.SerializeObject(settingsConfig));

        Debug.Log(settingsConfig.Language);
        OnLanguageChanged?.Invoke(_language);
    }
}
