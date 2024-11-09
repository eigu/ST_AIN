using UnityEngine;

public class Waste
{
    public WasteInfoSO info;
    public GameObject prefab;

    public Waste(WasteInfoSO info, GameObject prefab)
    {
        this.info = info;
        this.prefab = prefab;
    }
}