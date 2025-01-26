using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapIcon", menuName = "Map Icon Data")]
public class MapIconDataSO : ScriptableObject
{
    public bool isPlayer = false;
    public bool isMoving = false;
    public bool isRotating = false;
    public bool isClickable = true;
    public Sprite minimapIcon;
    public string iconDefaultName;
    public bool isCancelRotation = true;
}