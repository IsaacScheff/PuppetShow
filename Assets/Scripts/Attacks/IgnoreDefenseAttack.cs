using UnityEngine;

[CreateAssetMenu(fileName = "New Ignore Defense Attack", menuName = "Attacks/IgnoreDefenseAttack")]
public class IgnoreDefenseAttack : Attack {
    public override void Execute(Unit attacker, Unit target) {
        int damage = attacker.unitData.attack; 
        target.TakeDamage(damage);
    }
}