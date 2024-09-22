using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Unit : MonoBehaviour {
    public UnitData unitData;  
    private int currentHealth;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;  

    private float flashDuration = 0.3f;  

    private TextMeshProUGUI healthText; 
    private List<Attack> availableAttacks; 
    private AttackSelectionBehavior attackSelectionBehavior;  

    public delegate void AttackPerformedHandler(string logMessage);
    public event AttackPerformedHandler OnAttackPerformed;

    public void Initialize(GameObject healthTextPrefab) {
        currentHealth = unitData.maxHealth;

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) {
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        }

        if (unitData.unitImage != null) {
            spriteRenderer.sprite = unitData.unitImage;
        } else {
            Debug.LogWarning("No sprite assigned to unitData for " + unitData.unitName);
        }

        originalColor = spriteRenderer.color;

        GameObject healthTextObject = Instantiate(healthTextPrefab, FindObjectOfType<Canvas>().transform);
        healthText = healthTextObject.GetComponent<TextMeshProUGUI>();

        UpdateHealthTextPosition();
        UpdateHealthText();

        availableAttacks = new List<Attack>(unitData.attacks);
        attackSelectionBehavior = unitData.attackSelectionBehavior;
    }

    private void Update() {
        UpdateHealthTextPosition();
    }

    private void UpdateHealthTextPosition() {
        if (healthText != null) {
            Vector3 unitPosition = transform.position;
            Vector3 textPosition = new Vector3(unitPosition.x, unitPosition.y - 1.5f, unitPosition.z);
            healthText.transform.position = textPosition;
        }
    }

    private void UpdateHealthText() {
        if (healthText != null) {
            healthText.text = $"{currentHealth}/{unitData.maxHealth}";
        }
    }

    public int GetCurrentHealth() {
        return currentHealth;
    }

    public void TakeDamage(int damage) {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, unitData.maxHealth);
        UpdateHealthText();

        StartCoroutine(FlashRed());

        if (currentHealth <= 0) {
            Die();
        }
    }

    public void Heal(int amount) {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, unitData.maxHealth);  
        UpdateHealthText();
        Debug.Log($"{unitData.unitName} healed for {amount}. Current health: {currentHealth}");
    }

    public void PerformAttack(List<Unit> enemies, List<Unit> allies) {
        if (availableAttacks.Count == 0 || attackSelectionBehavior == null) {
            Debug.LogWarning($"{unitData.unitName} has no available attacks or attack selection behavior set!");
            return;
        }

        Attack selectedAttack = attackSelectionBehavior.SelectAttack(this, availableAttacks);

        Unit target = SelectTarget(enemies, allies, selectedAttack.targetType);

        if (selectedAttack != null && target != null) {
            StartCoroutine(WiggleAndAttack(target, selectedAttack));
            
            string attackMessage = FormatAttackMessage(unitData.unitName, selectedAttack.name, target.unitData.unitName);
            OnAttackPerformed?.Invoke(attackMessage);
        } else {
            Debug.LogWarning($"{unitData.unitName} did not select a valid attack or target.");
        }
    }

    private string FormatAttackMessage(string attackerName, string attackName, string targetName) {
        return $"{attackerName} uses {attackName} on {targetName}";
    }

    private IEnumerator WiggleAndAttack(Unit target, Attack selectedAttack) {
        yield return StartCoroutine(Wiggle(0.5f, 10f, 3));
        Debug.Log($"{unitData.unitName} finished wiggling and is now attacking {target.unitData.unitName}");
        selectedAttack.Execute(this, target);
    }
    private Unit SelectTarget(List<Unit> enemies, List<Unit> allies, TargetType targetType) {
        List<Unit> potentialTargets;
        TargetingBehavior targetingBehavior;

        if (targetType == TargetType.Enemies) {
            potentialTargets = enemies;
            targetingBehavior = unitData.enemyTargetingBehavior;
        } else {
            potentialTargets = allies;
            targetingBehavior = unitData.allyTargetingBehavior;
        }

        if (targetingBehavior != null) {
            Unit selectedTarget = targetingBehavior.SelectTarget(this, potentialTargets);
            if (selectedTarget == null) {
                Debug.LogWarning($"{unitData.unitName} cannot find a valid target to attack.");
            }
            return selectedTarget;
        } else {
            Debug.LogWarning($"{unitData.unitName} has no targeting behavior set, defaulting to first in list.");
            return potentialTargets.Count > 0 ? potentialTargets[0] : null;
        }
    }

    private void Die() {
        Debug.Log(unitData.unitName + " has died.");
        if (healthText != null) {
            Destroy(healthText.gameObject);  
        }
        Destroy(gameObject); 
    }

    private IEnumerator FlashRed() {
        float halfDuration = flashDuration / 2f;
        float elapsedTime = 0f;

        while (elapsedTime < halfDuration) {
            spriteRenderer.color = Color.Lerp(originalColor, Color.red, elapsedTime / halfDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        spriteRenderer.color = Color.red;
        elapsedTime = 0f;

        while (elapsedTime < halfDuration) {
            spriteRenderer.color = Color.Lerp(Color.red, originalColor, elapsedTime / halfDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        spriteRenderer.color = originalColor;
    }

    private IEnumerator Wiggle(float duration, float angle, int wiggles) {
        float wiggleDuration = duration / wiggles;
        bool rotateRight = true;

        for (int i = 0; i < wiggles; i++) {
            float startRotation = rotateRight ? -angle : angle;
            float endRotation = rotateRight ? angle : -angle;
            float currentWiggleTime = 0f;

            while (currentWiggleTime < wiggleDuration) {
                float zRotation = Mathf.Lerp(startRotation, endRotation, currentWiggleTime / wiggleDuration);
                transform.rotation = Quaternion.Euler(0, 0, zRotation);
                currentWiggleTime += Time.deltaTime;
                yield return null;
            }

            rotateRight = !rotateRight;
        }

        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public IEnumerator Jump(float duration, float height, int jumps, float delay = 0f) {
        if (delay > 0f) {
            yield return new WaitForSeconds(delay);
        }

        float jumpDuration = duration / jumps;
        Vector3 originalPosition = transform.position;

        for (int i = 0; i < jumps; i++) {
            float startY = originalPosition.y;
            float peakY = originalPosition.y + height;
            float currentJumpTime = 0f;

            // Move up to the peak
            while (currentJumpTime < jumpDuration / 2) {
                transform.position = new Vector3(originalPosition.x, Mathf.Lerp(startY, peakY, currentJumpTime / (jumpDuration / 2)), originalPosition.z);
                currentJumpTime += Time.deltaTime;
                yield return null;
            }

            currentJumpTime = 0f;
            while (currentJumpTime < jumpDuration / 2) {
                transform.position = new Vector3(originalPosition.x, Mathf.Lerp(peakY, startY, currentJumpTime / (jumpDuration / 2)), originalPosition.z);
                currentJumpTime += Time.deltaTime;
                yield return null;
            }
        }

        transform.position = originalPosition;
    }
}