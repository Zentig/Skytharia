using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestMove : MonoBehaviour
{
    public PlayerMovement playermovement;
    public bool ran;
    [SerializeField] private GameObject ranquest;
    public QuestTeleportSystem questplacement;
    public AudioSource audioSource;

    private void Update()
    {
        if (playermovement._speed > 6)
        {
          ran = true;
        }
        if(ran)
        {
            if (ranquest.activeSelf)
            {
                Destroy(questplacement.clone);
                ranquest.SetActive(false);
                questplacement.CloneAndTeleportToLastPlace();
                audioSource.Play();
            }
        }
    }
}