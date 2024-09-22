using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RandomAttackSelection", menuName = "Attack Selection/Random")]
public class RandomAttackSelection : AttackSelectionBehavior {
    public override Attack SelectAttack(Unit attacker, List<Attack> availableAttacks) {
        if (availableAttacks.Count == 0) return null;
        return availableAttacks[Random.Range(0, availableAttacks.Count)];
    }
}