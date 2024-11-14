using System;
using System.Collections;
using System.Collections.Generic;
using Interact;
using UnityEngine;

public class WasteManager : MonoBehaviour
{
   public List<GameObject> wastePrefabs;
   private Dictionary<string, Waste> _wasteMap;
   
   private void Awake()
   {
      _wasteMap = CreateWasteMap();
   }

   private void OnEnable()
   {
      GameEventsManager.Instance.WasteEvents.OnValidateWasteEvent += ValidateWaste;
      GameEventsManager.Instance.WasteEvents.OnSpawnWasteGameObjectEvent += SpawnWaste;
   }
   
   private void OnDisable()
   {
      GameEventsManager.Instance.WasteEvents.OnValidateWasteEvent -= ValidateWaste;
      GameEventsManager.Instance.WasteEvents.OnSpawnWasteGameObjectEvent -= SpawnWaste;
   }

   private Dictionary<string, Waste> CreateWasteMap()
   {
      Dictionary<string, Waste> idToQuestMap = new Dictionary<string, Waste>();

      foreach (var wastePrefab in wastePrefabs)
      {
         WasteInfoSO wasteInfo = wastePrefab.GetComponent<WasteInteract>().WasteInfoSo;
         
         if (idToQuestMap.ContainsKey(wasteInfo.ID))
         {
            Debug.LogWarning($"Skipping, duplicate ID found when creating waste map: {wasteInfo.ID}");
            break;
         }
            
         idToQuestMap.Add(wasteInfo.ID, new Waste(wasteInfo, wastePrefab));
      }

      return idToQuestMap;
   }
   
   private void ValidateWaste(string id)
   {
      if (!_wasteMap.ContainsKey(id)) Debug.LogWarning($"Waste Object is not recognized in manager: {id}");
   }

   private void SpawnWaste(string id, Vector3 pos, Quaternion rot)
   {
      if (!_wasteMap.ContainsKey(id)) return;
      Instantiate(_wasteMap[id].prefab, pos, rot);
   }
}
