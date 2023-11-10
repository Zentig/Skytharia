using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private string _settingsConfigPath = Application.streamingAssetsPath + "/SettingsConfig.json";

    public event Action OnHPChanged;
    public event Action OnEnergyChanged;
    public event Action OnCoinsChanged;

    [Header("HP")]
    [SerializeField] private int _maxHP;
    [SerializeField] private Image _hpImage;
    [Header("Energy")]
    [SerializeField] private int _maxEnergy;
    [SerializeField] private Image _energyImage;
    [Header("Coins")]
    [SerializeField] private TextMeshProUGUI _coinsText;

    private int _hp;
    private float _energy;
    private int _coins;

    public PlayerState PlayerState = PlayerState.Idle;

    #region Properties
    public int Health 
    { 
        get => _hp; 
        set
        {
            if (value <= _maxHP && value > 0)
               _hp = value;
               OnHPChanged?.Invoke();
        }
    }
    public float Energy
    {
        get => _energy;
        set
        {
            if (value <= _maxEnergy && value > 0)
                _energy = value;
                OnEnergyChanged?.Invoke();
        }
    }
    public int Coins { get => _coins; 
        set 
        {
            _coins = value; 
            OnCoinsChanged?.Invoke();
        } 
    }
    #endregion
    private void Awake()
    {
        OnHPChanged += CheckHPUI;
        OnEnergyChanged += CheckEnergyUI;
        OnCoinsChanged += CheckCoinsUI;
        InitStats();
    }
    private void InitStats()
    {
        var playerStats = LoadPlayerData();
        Health = playerStats.Health;
        Energy = playerStats.Energy;
        Coins = playerStats.Coins;
    }
    private void Start()
    {
        OnHPChanged?.Invoke();
        OnEnergyChanged?.Invoke();
        OnCoinsChanged?.Invoke();
    }
    private void OnDestroy() => SavePlayerData();

    void CheckHPUI() { _hpImage.fillAmount = (float)Health / _maxHP; }
    void CheckEnergyUI() { _energyImage.fillAmount = (float)Energy / _maxEnergy; }
    void CheckCoinsUI() { _coinsText.text = $"{Coins}"; }
    public void SavePlayerData()
    {
        var settingsConfig = JsonConvert.DeserializeObject<SettingsConfig>(File.ReadAllText(_settingsConfigPath, Encoding.UTF8));
        settingsConfig.playerStats.Health = Health;
        settingsConfig.playerStats.Energy = Energy;
        settingsConfig.playerStats.Coins = Coins;
        File.WriteAllText(_settingsConfigPath, JsonConvert.SerializeObject(settingsConfig));
    }
    public PlayerStats LoadPlayerData()
    {
        var settingsConfig = JsonConvert.DeserializeObject<SettingsConfig>(File.ReadAllText(_settingsConfigPath, Encoding.UTF8));
        Health = settingsConfig.playerStats.Health;
        Energy = settingsConfig.playerStats.Energy;
        Coins = settingsConfig.playerStats.Coins;
        return new PlayerStats { Health = Health, Energy = Energy, Coins = Coins};
    }
}
