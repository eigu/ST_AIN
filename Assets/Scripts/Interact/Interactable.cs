using UnityEngine;

namespace Interact
{
    public abstract class Interactable : MonoBehaviour
    {
        [Header("Interactable Parameters")] 
        public InteractableUI ui;
        [TextArea] public string guideText = "Press [F] for primary interact. Press [E] for secondary interact.";
            
        public virtual void Awake()
        {
            gameObject.layer = LayerMask.NameToLayer("Interactables");
        }

        public abstract void OnPrimaryInteract();
        
        public abstract void OnSecondaryInteract();
        public abstract void OnFocus();
        public abstract void OnLoseFocus();
        
        public abstract void OnNear();
        
        public abstract void OnLoseNear();
    }
}
