using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
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
    public int HP 
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

    private void Start()
    {
        HP = _maxHP;
        Energy = _maxEnergy;
        Coins = 0;

        OnHPChanged += CheckHPUI;
        OnEnergyChanged += CheckEnergyUI;
        OnCoinsChanged += CheckCoinsUI;

        OnHPChanged?.Invoke();
        OnEnergyChanged?.Invoke();
        OnCoinsChanged?.Invoke();
    }
    void CheckHPUI() { _hpImage.fillAmount = (float)HP / _maxHP; }
    void CheckEnergyUI() { _energyImage.fillAmount = (float)Energy / _maxEnergy; }
    void CheckCoinsUI() { _coinsText.text = $"{Coins}"; }
}
