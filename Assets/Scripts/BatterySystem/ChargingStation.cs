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
}
