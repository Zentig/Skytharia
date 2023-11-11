using UnityEngine;
using System.Collections.Generic;

public class QuestTeleportSystem : MonoBehaviour
{
    public Transform[] questPlaces;
    public bool q = true;
    public GameObject clone;
    public GameObject canvasQuests;
    public GameObject[] questObjectsToTeleport;

    void Start()
    {
        CloneAndTeleportToLastPlace();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (!q)
            {
                q = true;
                CloneAndTeleportToLastPlace();
            }
            else
            {
                q = false;
                Destroy(clone);
                clone = null;
            }
        }
        else
        {
            TeleportQuestObjects();
        }
    }

    void TeleportQuestObjects()
    {
        int activeObjectIndex = 0;

        for (int i = 0; i < questPlaces.Length; i++)
        {
            while (activeObjectIndex < questObjectsToTeleport.Length && !questObjectsToTeleport[activeObjectIndex].activeSelf)
            {
                activeObjectIndex++;
            }

            if (activeObjectIndex < questObjectsToTeleport.Length)
            {
                TeleportToPlace(activeObjectIndex, i);
                activeObjectIndex++;
            }
        }
    }

    void TeleportToPlace(int objectIndex, int placeIndex)
    {
        questObjectsToTeleport[objectIndex].transform.position = questPlaces[placeIndex].position;
    }

    public void CloneAndTeleportToLastPlace()
    {
        int lastPlaceIndex = questPlaces.Length - 1;

        for (int i = 0; i < questObjectsToTeleport.Length; i++)
        {
            if (questObjectsToTeleport[i].activeSelf)
            {
                clone = Instantiate(questObjectsToTeleport[i], canvasQuests.transform);
                clone.transform.position = new Vector3(questPlaces[lastPlaceIndex].position.x, questPlaces[lastPlaceIndex].position.y, 0f);
                clone.transform.localScale = new Vector2(800f, 200f);
                questObjectsToTeleport[i].SetActive(true);
                break;
            }
        }
    }
}