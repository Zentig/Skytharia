using Inventory.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Item : MonoBehaviour, IInteractable
{
    [field: SerializeField] public ItemSO InventoryItem { get; private set; }

    [field: SerializeField] public int Quantity { get; set; } = 1;

    [SerializeField] private AudioSource audioSource;

    [SerializeField] private float duration = 0.3f;

    public string InteractText { get; set; } = "get";

    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = InventoryItem.ItemImage;
        
    }
    public void Interact(){
        DestroyItem();
    }
    private void DestroyItem()
    {
        //GetComponent<Collider2D>().enabled = false;
        //StartCoroutine(AnimateItemPickup());
        Destroy(gameObject);
    }
    private IEnumerator AnimateItemPickup()
    {
        audioSource.Play();
        Vector3 startScale = transform.localScale;
        Vector3 endScale = Vector3.zero;
        float currentTime = 0;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            transform.localScale = 
                Vector3.Lerp(startScale, endScale, currentTime / duration);
            yield return null;
        }
        Destroy(gameObject);
    }
}
