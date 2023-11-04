using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private string _pickupableTag = "Pickupable Item";
    [SerializeField] private GameObject _grabText; // Reference to the "Press E to grab" text object

    private bool canGrab;
    private GameObject nearbyItem;

    void Start()
    {
        // Hide the "Press E to grab" text at the start
        if (_grabText != null)
        {
            _grabText.SetActive(false);
        }
    }

    void Update()
    {
        if (canGrab && Input.GetKeyDown(KeyCode.E))
        {
            if (nearbyItem != null)
            {
                Debug.Log("Item grabbed: " + nearbyItem.name);

                // Destroy the item
                Destroy(nearbyItem);

                // Clear the reference to the nearby item
                nearbyItem = null;

                // Hide the grab text
                if (_grabText != null)
                {
                    _grabText.SetActive(false);
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(_pickupableTag))
        {
            canGrab = true;
            nearbyItem = other.gameObject;
            if (_grabText != null)
            {
                _grabText.SetActive(true);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(_pickupableTag))
        {
            canGrab = false;
            nearbyItem = null;
            if (_grabText != null)
            {
                _grabText.SetActive(false);
            }
        }
    }
}
