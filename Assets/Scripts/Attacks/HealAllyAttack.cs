using UnityEngine;

[CreateAssetMenu(fileName = "New Heal Ally Attack", menuName = "Attacks/HealAllyAttack")]
public class HealAllyAttack : Attack {
    public override void Execute(Unit attacker, Unit target) {
        target.Heal(value); // Heal by the attack's value
    }
}