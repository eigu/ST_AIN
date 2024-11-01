using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewQuestInfo", menuName = "QuestInfoSO", order = 1)]
public class QuestInfoSO : ScriptableObject
{
    [field: SerializeField] public string ID { get; private set; }

    [Header("General")] 
    public string displayName;
    
    [Header("Requirements")] 
    //add requirements like chapter requirement
    public QuestInfoSO[] questPrerequisites;

    [Header("Steps")] 
    public GameObject[] questStepsPrefabs;

    [Header("Rewards")]
    public int money;
    // or any other saint 1 modules
    
    private void OnValidate()
    {
        #if UNITY_EDITOR
        ID = name;
        UnityEditor.EditorUtility.SetDirty(this);
        #endif
    }
}
