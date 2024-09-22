using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {
    public static PlayerManager Instance; 

    public int gold = 100; 
    public List<UnitData> ownedUnits = new List<UnitData>(); 

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Persist between scenes
        } else {
            Destroy(gameObject);
        }
    }

    public void AddGold(int amount) {
        gold += amount;
    }

    public bool SpendGold(int amount) {
        if (gold >= amount) {
            gold -= amount;
            return true;
        }
        return false;
    }

    public void AddUnit(UnitData unit) {
        ownedUnits.Add(unit);
    }
}