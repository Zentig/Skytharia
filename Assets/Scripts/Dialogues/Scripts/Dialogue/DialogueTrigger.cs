using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour, IInteractable
{
    // for npc
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    public string InteractText { get; set; } = "talk with";

    private void Awake() 
    {
        visualCue.SetActive(false);
    }
    public void Interact()
    {
        if (!DialogueManager.GetInstance().dialogueIsPlaying) 
        {
            DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
        }
    }
    public void SwitchVisualCue(bool v) => visualCue.SetActive(v);
}
