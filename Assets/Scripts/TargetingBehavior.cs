using UnityEngine;
using System.Collections.Generic;
public enum TargetingType {
    Random,
    LowestHealth,
    HighestHealth,
    FirstInList
}

public abstract class TargetingBehavior : ScriptableObject {
    public abstract Unit SelectTarget(Unit self, List<Unit> potentialTargets);
}

