using TMPro;
using UnityEngine;

namespace Interact
{
    public class InteractableUI : MonoBehaviour
    {
        private Camera _mainCam;
        public GameObject interactCircle;
        public GameObject interactInformation;
        public TextMeshProUGUI textInformation;
    
        private void Awake()
        {
            _mainCam = Camera.main;
        }

        void Update()
        {
            transform.rotation = _mainCam.transform.rotation;
        }

        public void ToggleInteractIndicator(bool show)
        {
            //animation effect
            interactCircle.SetActive(show);
        }
        
        public void ToggleInteractInformation(bool show)
        {
            //animation effect
            interactInformation.SetActive(show);
        }

        public void SetInfoText(string text)
        {
            textInformation.text = text;
        }
    }
}
