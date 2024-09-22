using System.Collections.Generic;
using UnityEngine;

public class UnitFactory : MonoBehaviour {
    public GameObject unitPrefab;  
    public GameObject healthTextPrefab; 

    public Unit CreateUnit(UnitData unitData, Transform spawnPoint) {
        GameObject unitObject = Instantiate(unitPrefab, spawnPoint.position, Quaternion.identity);
        Unit unit = unitObject.GetComponent<Unit>();

        if (unit != null) {
            unit.unitData = unitData;
            unit.Initialize(healthTextPrefab);
        } else {
            Debug.LogError("Unit prefab does not contain a Unit component.");
        }

        return unit;
    }
}
