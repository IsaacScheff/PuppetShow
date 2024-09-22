using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ShopManager : MonoBehaviour {
    public static ShopManager Instance; 
    public List<UnitData> availableUnits;
    public Transform[] unitSpawnPoints;
    public GameObject unitButtonPrefab;
    public TextMeshProUGUI goldText;
    public int rerollCost = 10;

    private List<GameObject> currentUnitButtons = new List<GameObject>();

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        DisplayPlayerGold();
        GenerateShopUnits();
    }

    private void DisplayPlayerGold() {
        goldText.text = "Gold: " + PlayerManager.Instance.gold;
    }

    private void GenerateShopUnits() {
        ClearShop();

        for (int i = 0; i < unitSpawnPoints.Length; i++) {
            UnitData randomUnit = availableUnits[Random.Range(0, availableUnits.Count)];
            GameObject unitButton = Instantiate(unitButtonPrefab, unitSpawnPoints[i].position, Quaternion.identity, unitSpawnPoints[i]);

            UnitButton unitButtonScript = unitButton.GetComponent<UnitButton>();
            unitButtonScript.Setup(randomUnit);

            currentUnitButtons.Add(unitButton);
        }
    }

    private void ClearShop() {
        foreach (GameObject button in currentUnitButtons) {
            Destroy(button);
        }
        currentUnitButtons.Clear();
    }

    public void RerollShopUnits() {
        if (PlayerManager.Instance.SpendGold(rerollCost)) {
            GenerateShopUnits();
            DisplayPlayerGold(); 
        } else {
            Debug.Log("Not enough gold to reroll!");
        }
    }

    public bool BuyUnit(UnitData unit) {
        if (PlayerManager.Instance.SpendGold(unit.cost)) {
            PlayerManager.Instance.AddUnit(unit);
            DisplayPlayerGold();
            return true;  // Purchase successful
        } else {
            Debug.Log("Not enough gold to buy this unit!");
            return false;  // Purchase failed
        }
    }

    public void ProceedToBattle() {
        SceneManager.LoadScene("BattleScene");
    }
}