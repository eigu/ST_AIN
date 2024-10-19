using System;
using UnityEngine;

namespace Interact
{
    public class TrashInteract : Interactable
    {

        [Header("Trash Parameters")] 
        [SerializeField] private string _trashName; //must be on scriptable object

        private void Start()
        {
           SetUpTrash();
        }

        public void SetUpTrash()
        {
            ui.SetInfoText(_trashName); 
        }
       
        public override void OnInteract()
        {
            //add to stack intentory
            
            gameObject.SetActive(false);
            // or destroy
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
