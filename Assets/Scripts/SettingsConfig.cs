using System;
using Skytharia.SaveManagement;

// Later there'll be other settings (volume, maybe lvl, etc.)
// it's struct of convertable object to json (check SettingsConfig.json)
[Serializable]
public struct SettingsConfig
{
    public string Language;
    public PlayerStats playerStats;

    public static SettingsConfig GetInstance()
    {
        SettingsConfig? loadedConfig = (SettingsConfig)SaveManager.Load<SettingsConfig>("Settings/settingsConfig.data", true);
        if (loadedConfig == null)
        {
            return (SettingsConfig)loadedConfig;
        } else
        {
            return new SettingsConfig();
        }
    }
    public static void SaveInstance(SettingsConfig config)
    {
        SaveManager.Save(config, "Settings/settingsConfig.data", true);
    }
}