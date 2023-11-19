using System.Collections;
using System.Collections.Generic;
using Inventory.Model;
using TMPro;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private Queue<IInteractable> _currentlyInteractedObjects;
    [Header("Inventory")]
    [SerializeField] private InventorySO _inventoryData;
    [SerializeField] private TextMeshProUGUI _onInteractedText;
    [SerializeField] private KeyCode _keyInteract;
    
    private void Start() {
        _onInteractedText.text = string.Empty;
        _currentlyInteractedObjects = new();
        _keyInteract = _keyInteract == KeyCode.None ? _keyInteract : KeyCode.E;
    }
    private void OnTriggerEnter2D(Collider2D other) 
    {
        var interactable = other.GetComponent<IInteractable>();
        if (interactable == null)
            return;
        if (!_currentlyInteractedObjects.Contains(interactable))
        {
            _currentlyInteractedObjects.Enqueue(interactable);
            _onInteractedText.text = $"Press {_keyInteract} to {interactable.InteractText}!";
        }
    }
    private void OnTriggerExit2D(Collider2D other) 
    {
        var interactable = other.GetComponent<IInteractable>();       
        if (interactable == null || !other.isActiveAndEnabled)
            return;
        _currentlyInteractedObjects.Dequeue();
        SetNextInteractText();       
    }
    void Update()
    {
        if (Input.GetKeyDown(_keyInteract) && _currentlyInteractedObjects.Count != 0)
        {
            InteractAction();
        }
    }
    private void InteractAction()
    {
        var peekedObj = _currentlyInteractedObjects.Peek();
        switch (peekedObj)
        {
            case Item item:   
                _inventoryData.AddItem(item.InventoryItem, item.Quantity);
                item.Interact();
                _currentlyInteractedObjects.Dequeue();
                break;
            case NPCTalking npc:
                npc.Interact();
                if (!npc.CanContinueDialogue) 
                { 
                    _currentlyInteractedObjects.Dequeue();   
                }
                break;
            default:
                Debug.LogError("IInteractable doesn't equal to any type of IInteractable's heirs");
                break;
            }
        SetNextInteractText();
    }
    private void SetNextInteractText()
    {
        if (_currentlyInteractedObjects.Count != 0) 
        {
            var nextInQueue = _currentlyInteractedObjects.Peek();  
            _onInteractedText.text = $"Press E to {nextInQueue.InteractText}!"; 
        } 
        else
        {
            _onInteractedText.text = "";
        }
    }
}
