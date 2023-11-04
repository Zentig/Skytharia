using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;


public class InventoryUI : MonoBehaviour {

Inventory inventory;

	void Start ()
	{
		inventory = Inventory.instance;
		inventory.onItemChangedCallback += UpdateUI;
	}

	void Update ()
	{
		
	}

	public void UpdateUI ()
	{
		Debug.Log("UPDATING UI");
	}
	
    }
	
