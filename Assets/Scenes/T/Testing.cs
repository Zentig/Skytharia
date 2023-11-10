using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField] private ActionOnTimer _actionOnTimer;

    private void Start()
    {
        _actionOnTimer.SetTimer(5f, () => { Debug.Log("Timer complete!"); });
    }
}
