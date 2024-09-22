using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Random Targeting", menuName = "Targeting/Random")]
public class RandomTargeting : TargetingBehavior {
    public override Unit SelectTarget(Unit attacker, List<Unit> potentialTargets) {
        List<Unit> validTargets = potentialTargets.FindAll(target => target.GetCurrentHealth() > 0);

        if (validTargets.Count == 0) {
            Debug.LogWarning($"{attacker.unitData.unitName} cannot find any valid targets.");
            return null;
        }

        // Select a random unit from the valid targets
        return validTargets[Random.Range(0, validTargets.Count)];
    }
}