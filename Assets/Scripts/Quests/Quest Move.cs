using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestMove : MonoBehaviour
{
    [SerializeField] private PlayerMovement _playermovement;
    private bool PlayerhasRun;
    [SerializeField] private GameObject _runquest;
    [SerializeField] private QuestTeleportSystem _questplacement;
    [SerializeField] private AudioSource _audioSource;

    private void Update()
    {
        if (_playermovement._speed > 6)
        {
            PlayerhasRun = true;
        }
        if(PlayerhasRun)
        {
            if (_runquest.activeSelf)
            {
                Destroy(_questplacement.clone);
                _runquest.SetActive(false);
                _questplacement.CloneAndTeleportToLastPlace();
                _audioSource.Play();
            }
        }
    }
}