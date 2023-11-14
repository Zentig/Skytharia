using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestDisplay : MonoBehaviour
{
    public Quests questRun;
    public Text questNameText;
    public Text descriptionText;
    public Image artImage;
    public bool complete;

    void Start()
    {
        if (questRun != null)
        {
            questNameText.text = questRun.Name;
            descriptionText.text = questRun.Description;
            artImage.sprite = questRun.QuestLogo;
            complete = questRun.Complete;
        }
    }
}