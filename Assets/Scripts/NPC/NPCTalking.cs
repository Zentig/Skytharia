using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTalking : MonoBehaviour, IInteractable
{
    public string InteractText { get; set; } = "talk";
    public bool CanContinueDialogue { get; private set; }
    [SerializeField] private List<AudioClip> _audios;
    private AudioSource _src;
    private int _indexOfNextPhrase;
    private Collider2D _talkTrigger;

    private void Start()
    {
        _src = GetComponent<AudioSource>();
        _talkTrigger = GetComponent<Collider2D>();
        _indexOfNextPhrase = 0;
        CanContinueDialogue = _indexOfNextPhrase < _audios.Count;
    }
    private void PlayNextPhrase()
    {
        _src.PlayOneShot(_audios[_indexOfNextPhrase]);
        _indexOfNextPhrase++;       
        CanContinueDialogue = _indexOfNextPhrase < _audios.Count;
        if (!CanContinueDialogue) 
        {
            _talkTrigger.enabled = false;
            return;
        }
    }
    public void Interact()
    {
        if (!_src.isPlaying && CanContinueDialogue) 
        {
            PlayNextPhrase();
        }
    }
}
