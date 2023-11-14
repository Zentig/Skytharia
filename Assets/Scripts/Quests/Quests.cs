using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new quest", menuName = "Quest/minion")]
public class Quests : ScriptableObject
{
    public string Name;
    public string Description;

    public Sprite QuestLogo;
    public bool Complete;
}
