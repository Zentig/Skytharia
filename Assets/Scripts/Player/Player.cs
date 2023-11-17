using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Inventory.Model;

public class Player : MonoBehaviour, IHasPersistentData
{
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
    }
    private void Start()
    {
        OnHPChanged?.Invoke();
        OnEnergyChanged?.Invoke();
        OnCoinsChanged?.Invoke();
    }
    void CheckHPUI() { _hpImage.fillAmount = (float)Health / _maxHP; }
    void CheckEnergyUI() { _energyImage.fillAmount = (float)Energy / _maxEnergy; }
    void CheckCoinsUI() { _coinsText.text = $"{Coins}"; }
    public void OnSaveGame()
    {
        var settingsConfig = SettingsConfig.GetInstance();
        settingsConfig.playerStats.Health = Health;
        settingsConfig.playerStats.Energy = Energy;
        settingsConfig.playerStats.Coins = Coins;
        SettingsConfig.SaveInstance(settingsConfig);
    }
    public void OnLoadGame()
    {
        var settingsConfig = SettingsConfig.GetInstance();
        Health = settingsConfig.playerStats.Health;
        Energy = settingsConfig.playerStats.Energy;
        Coins = settingsConfig.playerStats.Coins;
    }
}
