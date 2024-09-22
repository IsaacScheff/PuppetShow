using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour {
    private UnitDataHandler unitDataHandler;

    private void Awake() {
        unitDataHandler = FindObjectOfType<UnitDataHandler>();

        if (unitDataHandler == null) {
            Debug.LogError("UnitDataHandler not found in the scene.");
        }
    }

    public List<UnitData> GetPlayerUnitsForBattle() {
        List<UnitData> playerUnits = new List<UnitData>();

        PlayerManager playerManager = PlayerManager.Instance;
        if (playerManager != null) {
            playerUnits = playerManager.ownedUnits;
        } else {
            Debug.LogError("PlayerManager instance not found.");
        }

        return playerUnits;
    }

    public List<UnitData> GetEnemyUnitsForBattle() {
        List<UnitData> enemyUnits = new List<UnitData>();

        if (unitDataHandler != null) {
            // for now just makes enemy units a single Fancy Pants
            enemyUnits.Add(unitDataHandler.GetUnitDataByName("Fancy Pants"));
        }

        return enemyUnits;
    }
}
