using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CycleAttackSelection", menuName = "Attack Selection/Cycle")]
public class CycleAttackSelection : AttackSelectionBehavior {
    private int currentIndex = 0;  // Keeps track of which attack to use next

    public override Attack SelectAttack(Unit attacker, List<Attack> availableAttacks) {
        if (availableAttacks.Count == 0) return null;
        
        Attack selectedAttack = availableAttacks[currentIndex];
        currentIndex = (currentIndex + 1) % availableAttacks.Count;  // Cycle to the next attack
        
        return selectedAttack;
    }
}