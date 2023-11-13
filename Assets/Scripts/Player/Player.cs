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
using Inventory.Model;

public class Player : MonoBehaviour, IHasPersistentData
{
    public event Action OnHPChanged;
    public event Action OnEnergyChanged;
    public event Action OnCoinsChanged;
    public Queue<IInteractable> CurrentlyInteractedObjects;

    [SerializeField] private TextMeshProUGUI _onInteractedText;
    [Header("HP")]
    [SerializeField] private int _maxHP;
    [SerializeField] private Image _hpImage;
    [Header("Energy")]
    [SerializeField] private int _maxEnergy;
    [SerializeField] private Image _energyImage;
    [Header("Coins")]
    [SerializeField] private TextMeshProUGUI _coinsText;
    [Header("Inventory")]
    [SerializeField] private InventorySO _inventoryData;

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
        _onInteractedText.text = string.Empty;
        OnHPChanged?.Invoke();
        OnEnergyChanged?.Invoke();
        OnCoinsChanged?.Invoke();
        CurrentlyInteractedObjects = new();
    }
    private void Update() 
    {
        if (Input.GetKeyDown(KeyCode.E) && CurrentlyInteractedObjects.Count != 0)
        {
            var peekedObj = CurrentlyInteractedObjects.Dequeue();
            switch (peekedObj)
            {
                case Item item:   
                    _inventoryData.AddItem(item.InventoryItem, item.Quantity);
                    item.Interact();
                    break;
                default:
                    Debug.LogError("IInteractable doesn't equal to any type of IInteractable's heirs");
                    break;
            }
            if (CurrentlyInteractedObjects.Count != 0) 
            {
                var nextInQueue = CurrentlyInteractedObjects.Peek();  
                _onInteractedText.text = $"Press E to {nextInQueue.InteractText}!";
            } 
            else
            {
                _onInteractedText.text = "";
            } 
        }
    }
    private void OnTriggerEnter2D(Collider2D other) 
    {
        var interactable = other.GetComponent<IInteractable>();
        if (interactable == null)
            return;
        if (!CurrentlyInteractedObjects.Contains(interactable))
        {
            CurrentlyInteractedObjects.Enqueue(interactable);
            _onInteractedText.text = $"Press E to {interactable.InteractText}!";
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        var interactable = other.GetComponent<IInteractable>();       
        if (interactable == null || !other.gameObject.activeInHierarchy)
            return;
        CurrentlyInteractedObjects.Dequeue();
        if (CurrentlyInteractedObjects.Count != 0) 
        {
            var nextInQueue = CurrentlyInteractedObjects.Peek();  
            _onInteractedText.text = $"Press E to {nextInQueue.InteractText}!"; 
        } 
        else
        {
            _onInteractedText.text = "";
        } 
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
