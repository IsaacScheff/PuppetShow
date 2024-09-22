using UnityEngine;

public enum TargetType {
    Enemies,
    Allies
}

public abstract class Attack : ScriptableObject {
    public int value;
    public TargetType targetType;  
    public abstract void Execute(Unit attacker, Unit target);
}