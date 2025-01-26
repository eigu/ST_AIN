using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapIcon : MonoBehaviour
{
    public static event Action<MapIcon> OnIconSelectEvent; //not on GEM because only on same script.
   
    public Transform objectRepresent;
    public Image Image;
    public RectTransform RectTransform;
    public RectTransform IconRectTransform;
    public bool isHighlight;
    public bool isMoving;
    public bool isRotating;
    public Vector2 originalPosition;
    public bool isCancelRotation;
    public string iconName;
    public TextMeshProUGUI labelText;
    public GameObject labelContainer;
    public bool isClickable;
    public float overrideScaleMultiplier;
    
    private void OnEnable()
    {
        OnIconSelectEvent += HideLabel;
    }

    private void OnDisable()
    {
        OnIconSelectEvent -= HideLabel;
    }
    
    private void HideLabel(MapIcon icon)
    {
        if(icon != this) labelContainer.SetActive(false);
    }

    public void OnIconSelect()
    {
        if (!isClickable) return;
        OnIconSelectEvent?.Invoke(this);
        labelText.text = iconName;
        labelContainer.SetActive(!labelContainer.activeInHierarchy);
    }

    public void ApplyScale()
    {
        RectTransform.sizeDelta *= overrideScaleMultiplier;
    }

}