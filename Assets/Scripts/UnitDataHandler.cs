using System.Collections.Generic;
using UnityEngine;

public class UnitDataHandler : MonoBehaviour {
    public List<UnitData> allUnits;  

    public UnitData GetUnitDataByName(string unitName) {
        foreach (UnitData unitData in allUnits) {
            if (unitData.unitName == unitName) {
                return unitData;
            }
        }
        Debug.LogWarning($"UnitData with name {unitName} not found.");
        return null;
    }

    // Method to get a random UnitData (optional, for variety)
    public UnitData GetRandomUnitData() {
        if (allUnits.Count == 0) return null;
        return allUnits[Random.Range(0, allUnits.Count)];
    }
}
