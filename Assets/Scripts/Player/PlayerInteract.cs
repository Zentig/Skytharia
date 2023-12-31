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
    
    private void Start() {
        _onInteractedText.text = string.Empty;
        _currentlyInteractedObjects = new();
    }
    private void OnTriggerEnter2D(Collider2D other) 
    {
        var interactable = other.GetComponent<IInteractable>();
        if (interactable == null)
            return;
        if (!_currentlyInteractedObjects.Contains(interactable))
        {
            _currentlyInteractedObjects.Enqueue(interactable);
            if (interactable is DialogueTrigger dt) { dt.SwitchVisualCue(true); }
            _onInteractedText.text = $"Able to {interactable.InteractText}!";
        }
    }
    private void OnTriggerExit2D(Collider2D other) 
    {
        var interactable = other.GetComponent<IInteractable>();       
        if (interactable == null || !other.isActiveAndEnabled || !_currentlyInteractedObjects.Contains(interactable))
            return;
        _currentlyInteractedObjects.Dequeue();
        if (interactable is DialogueTrigger dt) { dt.SwitchVisualCue(false); }
        SetNextInteractText();       
    }
    void Update()
    {
        if (InputManager.GetInstance().GetInteractPressed() && _currentlyInteractedObjects.Count != 0)
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
            case DialogueTrigger dialogueTrigger:
                dialogueTrigger.Interact();
                _currentlyInteractedObjects.Dequeue();
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
            _onInteractedText.text = $"Able to {nextInQueue.InteractText}!"; 
        } 
        else
        {
            _onInteractedText.text = "";
        }
    }
}
