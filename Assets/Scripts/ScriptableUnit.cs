using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Unit", menuName = "Unit")]
public class UnitData : ScriptableObject {
    public string unitName;
    public int maxHealth;
    public int attack;
    public int defense;
    public float initiative;
    public int cost;
    public Sprite unitImage;
    public List<Attack> attacks;  
    public TargetingBehavior enemyTargetingBehavior;  
    public TargetingBehavior allyTargetingBehavior;
    public AttackSelectionBehavior attackSelectionBehavior;  
}