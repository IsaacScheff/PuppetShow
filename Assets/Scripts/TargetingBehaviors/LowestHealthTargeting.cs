using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Lowest Health Targeting", menuName = "Targeting/LowestHealth")]
public class LowestHealthTargeting : TargetingBehavior {
    public override Unit SelectTarget(Unit attacker, List<Unit> potentialTargets) {
        List<Unit> validTargets = potentialTargets.FindAll(target => target.GetCurrentHealth() > 0);

        if (validTargets.Count == 0) {
            Debug.LogWarning($"{attacker.unitData.unitName} cannot find any valid targets.");
            return null;
        }

        // Find the unit with the lowest health
        Unit lowestHealthTarget = validTargets[0];
        foreach (Unit target in validTargets) {
            if (target.GetCurrentHealth() < lowestHealthTarget.GetCurrentHealth()) {
                lowestHealthTarget = target;
            }
        }

        return lowestHealthTarget;
    }
}

