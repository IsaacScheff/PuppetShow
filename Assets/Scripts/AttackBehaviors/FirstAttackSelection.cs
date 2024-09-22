using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FirstAttackSelection", menuName = "Attack Selection/First")]
public class FirstAttackSelection : AttackSelectionBehavior {
    public override Attack SelectAttack(Unit attacker, List<Attack> availableAttacks) {
        if (availableAttacks.Count == 0) return null;
        return availableAttacks[0];  // Always select the first attack
    }
}