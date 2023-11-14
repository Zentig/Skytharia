using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] private Animator _objectAnimator; // Reference to the Animator component on the object
    [SerializeField] private GameObject _objectToActivate; // Reference to the object to activate/deactivate
    private bool isObjectActive = false;
    [SerializeField] private GameObject _main;
    [SerializeField] private GameObject _Select;

    private void Start()
    {
        Debug.Log("0 0");
        _objectToActivate.transform.localScale = new Vector2(0,0);
    }

    private void Update()
    {
        // Check for "Q" key press
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ToggleAnimationAndObject();
        }
    }

    private void ToggleAnimationAndObject()
    {
        if (isObjectActive)
        {
            // Play the second animation to close the object
            _objectAnimator.SetTrigger("close");
            _Select.SetActive(true);
            _main.SetActive(false);
        }
        else
        {
            // Play the first animation to open the object
            _objectAnimator.SetTrigger("open");

        }

        // Toggle the state of the object
        isObjectActive = !isObjectActive;
    }
}
