using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "First In List Targeting", menuName = "Targeting/FirstInList")]
public class FirstInListTargeting : TargetingBehavior {
    public override Unit SelectTarget(Unit attacker, List<Unit> potentialTargets) {
        List<Unit> validTargets = potentialTargets.FindAll(target => target.GetCurrentHealth() > 0);

        if (validTargets.Count == 0) {
            Debug.LogWarning($"{attacker.unitData.unitName} cannot find any valid targets.");
            return null;
        }

        // Return the first valid unit in the list
        return validTargets[0];
    }
}