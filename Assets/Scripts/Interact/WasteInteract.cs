using System;
using UnityEditor;
using UnityEngine;

namespace Interact
{
    public class WasteInteract : Interactable
    {
        [Header("Waste Parameters")] 
        [SerializeField] private WasteInfoSO _wasteInfo;

        public WasteInfoSO WasteInfoSo => _wasteInfo;
        private void Start()
        {
           SetUpTrash();
        }

        private void SetUpTrash()
        {
            VerifyWasteInfo();
            ui.SetInfoText(_wasteInfo.displayName); 
        }

        private void VerifyWasteInfo()
        {
            GameEventsManager.Instance.WasteEvents.ValidateWaste(_wasteInfo.ID);
        }
       
        public override void OnInteract()
        {
            //add to stack intentory
            
            //gameObject.SetActive(false);
            // or destroy
            GameEventsManager.Instance.PlayerInteractEvents.WastePickUp(_wasteInfo, gameObject);
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


        public void Rename()
        {
            gameObject.name = _wasteInfo.ID;
        }
    }
}
