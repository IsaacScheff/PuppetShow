using System.Collections.Generic;
using UnityEngine;

public abstract class AttackSelectionBehavior : ScriptableObject {
    public abstract Attack SelectAttack(Unit attacker, List<Attack> availableAttacks);
}