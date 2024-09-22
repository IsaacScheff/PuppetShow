using UnityEngine;

[CreateAssetMenu(fileName = "New Basic Attack", menuName = "Attacks/BasicAttack")]
public class BasicAttack : Attack {
    public override void Execute(Unit attacker, Unit target) {
        int damage = Mathf.Max(attacker.unitData.attack + value - target.unitData.defense, 0);
        target.TakeDamage(damage);
    }
}
