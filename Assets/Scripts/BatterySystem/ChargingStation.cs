using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingStation : MonoBehaviour
{
   private void Start()
   {
      GameEventsManager.Instance.BatteryEvents.InitializeChargingStation(this);
   }

   private void OnTriggerEnter(Collider collision)
   {
      if (collision.TryGetComponent<BatterySystem>(out BatterySystem batterySystem))
      {
         batterySystem.ReplenishBattery();
      }
   }

   private void On(Collision collision)
   {
      Debug.Log("");
      
   }
}
