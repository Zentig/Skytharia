using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour, IInteractable
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject _visualCue;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset _inkJSON;

    public string InteractText { get; set; } = "start dialogue";

    private void Awake() 
    {
        _visualCue.SetActive(false);
    }
    public void Interact()
    {
        if (!DialogueManager.GetInstance().dialogueIsPlaying) 
        {
            _visualCue.SetActive(true);
            
            DialogueManager.GetInstance().EnterDialogueMode(_inkJSON);
        }
        else 
        {
            _visualCue.SetActive(false);
        }
    }
    public void SwitchVisualCue(bool state) => _visualCue.SetActive(state);
}
