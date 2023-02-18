using System;
using System.Collections;
using System.Collections.Generic;
using PG.Battle;
using Sirenix.Utilities;
using UnityEngine;

public class InspirationCircleTrigger : MonoBehaviour
{
    private List<MobScript> mobList = new List<MobScript>();
    public List<MobScript> InRangeMobList => mobList;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.GetComponent<MobScript>().SafeIsUnityNull())
        {
            mobList.Add(other.GetComponent<MobScript>());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.GetComponent<MobScript>().SafeIsUnityNull())
        {
            mobList.Remove(other.GetComponent<MobScript>());
        }
    }
}
