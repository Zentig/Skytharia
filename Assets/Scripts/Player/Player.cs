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
    private int _energy;
    private int _coins;

    #region Properties
    public int HP 
    { 
        get => _hp; 
        set
        {
            if (value < _maxHP)
               _hp = value;
                OnHPChanged?.Invoke();
        }
    }
    public int Energy
    {
        get => _energy;
        set
        {
            if (value < _maxEnergy)
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
        OnHPChanged += () => { _hpImage.fillAmount = HP / _maxHP; };
        OnEnergyChanged += () => _energyImage.fillAmount = Energy / _maxEnergy;
        OnCoinsChanged += () => _coinsText.text = $"{Coins}";
    }
}
