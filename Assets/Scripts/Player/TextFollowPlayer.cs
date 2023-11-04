using UnityEngine;

public class TextFollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform player; // Reference to the player's head (or the object to follow)

    void Update()
    {
        if (player != null)
        {
            // Use the position of the player to position the text above their head
            transform.position = player.position + Vector3.up * 1.8f; // Adjust the Vector3.up value for the desired height.
        }
    }
}
