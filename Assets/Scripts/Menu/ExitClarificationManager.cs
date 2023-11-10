using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitClarificationManager : MonoBehaviour
{
    private void Awake()
    {
        TogglePanel(false);
    }
    public void TogglePanel(bool state)
    {
        gameObject.SetActive(state);
    }
}
