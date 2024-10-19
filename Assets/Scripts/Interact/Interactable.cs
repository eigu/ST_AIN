using UnityEngine;

namespace Interact
{
    public abstract class Interactable : MonoBehaviour
    {
        [Header("Interactable Parameters")] 
        public InteractableUI ui;
            
        public virtual void Awake()
        {
            gameObject.layer = LayerMask.NameToLayer("Interactables");
        }

        public abstract void OnInteract();
        public abstract void OnFocus();
        public abstract void OnLoseFocus();
        
        public abstract void OnNear();
        
        public abstract void OnLoseNear();
    }
}
