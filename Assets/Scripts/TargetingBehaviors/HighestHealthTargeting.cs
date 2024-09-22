using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Highest Health Targeting", menuName = "Targeting/HighestHealth")]
public class HighestHealthTargeting : TargetingBehavior {
    public override Unit SelectTarget(Unit attacker, List<Unit> potentialTargets) {
        List<Unit> validTargets = potentialTargets.FindAll(target => target.GetCurrentHealth() > 0);

        if (validTargets.Count == 0) {
            Debug.LogWarning($"{attacker.unitData.unitName} cannot find any valid targets.");
            return null;
        }

        // Find the unit with the highest health
        Unit highestHealthTarget = validTargets[0];
        foreach (Unit target in validTargets) {
            if (target.GetCurrentHealth() > highestHealthTarget.GetCurrentHealth()) {
                highestHealthTarget = target;
            }
        }

        return highestHealthTarget;
    }
}