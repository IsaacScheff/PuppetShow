using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleSystem : MonoBehaviour {
    public UnitFactory unitFactory; 
    public Transform[] playerSpawnPoints; 
    public Transform[] enemySpawnPoints; 

    public TextMeshProUGUI battleLogText; 
    public Button returnToShopButton; 
    public Button beginBattleButton;

    private List<Unit> playerUnits;   
    private List<Unit> enemyUnits;  
    private List<Unit> allUnits;  
    private Dictionary<Unit, float> initiativeTotals; 
    private float actionDelay = 0.65f;
    public UnitManager unitManager; 
    private const float initiativeToAttack = 100f;

    private bool isPreBattleSetup = true;
    private void Start() {
        InitializePreBattle();
        //StartCoroutine(StartBattleRoutine());

        if (beginBattleButton != null) {
            beginBattleButton.gameObject.SetActive(true);
            beginBattleButton.onClick.AddListener(StartBattle);
        }
    }

    private void InitializePreBattle() {
        allUnits = new List<Unit>();
        playerUnits = new List<Unit>();
        enemyUnits = new List<Unit>();
        initiativeTotals = new Dictionary<Unit, float>();

        List<UnitData> playerUnitsData = unitManager.GetPlayerUnitsForBattle();
        List<UnitData> enemyUnitsData = unitManager.GetEnemyUnitsForBattle();

        Debug.Log("Initializing player units for pre-battle setup...");
        for (int i = 0; i < playerUnitsData.Count; i++) {
            if (i < playerSpawnPoints.Length) {
                Unit unit = unitFactory.CreateUnit(playerUnitsData[i], playerSpawnPoints[i]);
                playerUnits.Add(unit);
                allUnits.Add(unit);
                initiativeTotals[unit] = 0f;
                Debug.Log($"Created Player Unit: {unit.unitData.unitName}, Health: {unit.GetCurrentHealth()}");
            }
        }

        Debug.Log("Initializing enemy units for pre-battle setup...");
        for (int i = 0; i < enemyUnitsData.Count; i++) {
            if (i < enemySpawnPoints.Length) {
                Unit unit = unitFactory.CreateUnit(enemyUnitsData[i], enemySpawnPoints[i]);
                enemyUnits.Add(unit);
                allUnits.Add(unit);
                initiativeTotals[unit] = 0f;
                Debug.Log($"Created Enemy Unit: {unit.unitData.unitName}, Health: {unit.GetCurrentHealth()}");
            }
        }

        if (beginBattleButton != null) {
            beginBattleButton.gameObject.SetActive(true);
        }
    }


    public void StartBattle() {
        // Lock in unit positions and start the battle
        isPreBattleSetup = false;
        StartCoroutine(StartBattleRoutine());

        // Hide Begin Battle button after starting
        if (beginBattleButton != null) {
            beginBattleButton.gameObject.SetActive(false);
        }

        // Ensure the return button is hidden at the start of the battle
        if (returnToShopButton != null) {
            returnToShopButton.gameObject.SetActive(false);
            returnToShopButton.onClick.AddListener(ReturnToShop);
        }
    }
    private IEnumerator StartBattleRoutine() {
        bool battleOngoing = true;

        while (battleOngoing) {
            // Increment initiative for each unit
            foreach (Unit unit in allUnits) {
                if (unit.GetCurrentHealth() > 0) {  // Only consider units that are alive
                    initiativeTotals[unit] += unit.unitData.initiative;  // Add the unit's initiative to its running total

                    if (initiativeTotals[unit] >= initiativeToAttack) {
                        yield return StartCoroutine(ExecuteTurn(unit));  // Execute the turn with a delay
                        initiativeTotals[unit] -= initiativeToAttack;  // Roll over the total after the unit acts
                    }
                }
            }

            // Sort units by their initiative totals and then by their initiative stat for tie-breaking
            allUnits.Sort((a, b) => {
                float aTotal = initiativeTotals[a];
                float bTotal = initiativeTotals[b];

                if (aTotal == bTotal) {
                    return b.unitData.initiative.CompareTo(a.unitData.initiative);  // Higher initiative stat goes first in case of tie
                }
                return bTotal.CompareTo(aTotal);  // Higher total goes first
            });

            battleOngoing = !CheckBattleOver();

            // Add a small delay between each battle step for pacing
            yield return new WaitForSeconds(actionDelay);
        }

        EndBattle();
    }

    // Coroutine to execute a unit's turn with a delay
    private IEnumerator ExecuteTurn(Unit unit) {
        List<Unit> potentialEnemies = playerUnits.Contains(unit) ? enemyUnits : playerUnits;
        List<Unit> potentialAllies = playerUnits.Contains(unit) ? playerUnits : enemyUnits;

        // Call the unit's encapsulated attack method
        unit.PerformAttack(potentialEnemies, potentialAllies);

        // Wait for the action to be observed by the player
        yield return new WaitForSeconds(actionDelay);
    }

    // Method to update the battle log text
    private void UpdateBattleLog(string message) {
        if (battleLogText != null) {
            battleLogText.text = message;
        }
    }

    // Check if the battle is over
    private bool CheckBattleOver() {
        bool playerDefeated = playerUnits.TrueForAll(u => u.GetCurrentHealth() <= 0);
        bool enemiesDefeated = enemyUnits.TrueForAll(u => u.GetCurrentHealth() <= 0);

        return playerDefeated || enemiesDefeated;
    }

    private void EndBattle() {
        string logMessage = "Battle Over!";
        UpdateBattleLog(logMessage);

        bool playerWon = playerUnits.Exists(u => u.GetCurrentHealth() > 0);  // Check if any player units are still alive

        if (playerWon) {
            PlayVictoryAnimation(playerUnits);
            AwardGoldForVictory();  // Award gold if the player wins
        } else {
            PlayVictoryAnimation(enemyUnits);
        }

        // Remove defeated player units permanently
        RemoveDefeatedPlayerUnits();

        // Show the return to shop button
        if (returnToShopButton != null) {
            returnToShopButton.gameObject.SetActive(true);
        }
    }

    // Method to remove defeated player units from the owned units list
    private void RemoveDefeatedPlayerUnits() {
        List<UnitData> defeatedUnits = new List<UnitData>();

        // Identify defeated units
        foreach (Unit unit in playerUnits) {
            if (unit.GetCurrentHealth() <= 0) {
                defeatedUnits.Add(unit.unitData);
            }
        }

        // Remove defeated units from the player's owned units
        foreach (UnitData defeatedUnit in defeatedUnits) {
            if (PlayerManager.Instance.ownedUnits.Contains(defeatedUnit)) {
                PlayerManager.Instance.ownedUnits.Remove(defeatedUnit);
                Debug.Log($"Removed defeated unit: {defeatedUnit.unitName} from player's owned units.");
            }
        }
    }

    // Method to award gold to the player after a win
    private void AwardGoldForVictory() {
        int goldReward = 50;  // Set the amount of gold to award
        PlayerManager.Instance.AddGold(goldReward);
        Debug.Log($"Player awarded {goldReward} gold for victory! Total gold: {PlayerManager.Instance.gold}");
    }

    private void PlayVictoryAnimation(List<Unit> winningTeam) {
        List<Unit> survivingUnits = new List<Unit>();

        // Filter out units that are null or have been destroyed
        foreach (Unit unit in winningTeam) {
            if (unit != null && unit.GetCurrentHealth() > 0) {
                survivingUnits.Add(unit);
            }
        }

        // Play the victory animation only for the surviving units
        for (int i = 0; i < survivingUnits.Count; i++) {
            float delay = (i % 2 == 0) ? 0f : 0.2f;  // Stagger every other unit by 0.2 seconds
            StartCoroutine(survivingUnits[i].Jump(1f, 0.5f, 3, delay));  // Example: 1 second duration, 0.5 units high, 3 jumps
        }
    }

    private void ReturnToShop() {
        SceneManager.LoadScene("ShopScene");
    }
}