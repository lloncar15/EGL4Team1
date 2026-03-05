
using System.Collections.Generic;
using UnityEngine;

public class BloodManager : GenericSingleton<BloodManager> {
    [SerializeField] private int maxDrains = 5;
    [SerializeField] public List<BloodType> collectedBlood = new();

    public void AddBlood(BloodType bloodType) {
        if (collectedBlood.Count >= maxDrains)
            return;
        
        collectedBlood.Add(bloodType);
        //TODO: blood UI updating
    }
}