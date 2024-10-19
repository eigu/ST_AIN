using System;
using System.Collections;
using System.Collections.Generic;
using Interact;
using UnityEngine;

public class InteractNear : MonoBehaviour
{
    [SerializeField] private Interactable _interactable;

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            _interactable.OnNear();
        }
    }
    
    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            _interactable.OnLoseNear();
        }
    }
}
