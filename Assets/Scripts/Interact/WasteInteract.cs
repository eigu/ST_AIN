using System;
using UnityEngine;

namespace Interact
{
    public class WasteInteract : Interactable
    {

        [Header("Waste Parameters")] 
        [SerializeField] private string _wasteName; //must be on scriptable object
        [SerializeField] private TestWasteType _wasteType; //must be on scriptable object
        private void Start()
        {
           SetUpTrash();
        }

        public void SetUpTrash()
        {
            ui.SetInfoText(_wasteName); 
        }
       
        public override void OnInteract()
        {
            //add to stack intentory
            
            gameObject.SetActive(false);
            // or destroy
            GameEventsManager.Instance.PlayerInteractEvents.WastePickUp(_wasteType);
        }

        public override void OnFocus()
        {
            ui.ToggleInteractInformation(true);
            ui.ToggleInteractIndicator(false);
        }

        public override void OnLoseFocus()
        {
            ui.ToggleInteractInformation(false);
            ui.ToggleInteractIndicator(true);
        }

        public override void OnNear()
        {
            ui.ToggleInteractIndicator(true);
        }

        public override void OnLoseNear()
        {
            ui.ToggleInteractIndicator(false);
        }

        
    }
}
