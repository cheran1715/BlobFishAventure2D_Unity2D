using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 3;
    private int currentHealth;

    [Header("UI Hearts")]
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    [Header("References")]
    public Animator animator;
    public MonoBehaviour dragShot;
    public GameObject gameOverUI; // Assign your Game Over UI panel

    [Header("Damage Settings")]
    public float damageCooldown = 0.5f;
    private bool canTakeDamage = true;

    [Header("Hit Animation")]
    public float hitAnimationDuration = 0.5f;

    [Header("Platform Hazard Settings")]
    public float platformDamageDelay = 1f;
    private bool isOnPlatform = false;
    private float platformTimer = 0f;

    private bool isDead = false;
    private Transform currentCheckpoint;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHeartsUI();

        if (gameOverUI != null)
            gameOverUI.SetActive(false);

        SetNearestCheckpoint();
    }

    void Update()
    {
        if (isOnPlatform && !isDead)
        {
            platformTimer += Time.deltaTime;
            if (platformTimer >= platformDamageDelay)
            {
                TakeDamageAndTeleport();
                platformTimer = 0f;
            }
        }
    }

    #region Checkpoint Handling
    private void SetNearestCheckpoint()
    {
        if (CheckpointManager.Instance != null)
        {
            Transform nearest = CheckpointManager.Instance.GetNearestCheckpoint(transform.position);
            if (nearest != null)
            {
                currentCheckpoint = nearest;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Checkpoint"))
        {
            currentCheckpoint = collision.transform;
        }
    }
    #endregion

    #region Damage Methods

    private void TakeDamageAndTeleport(int amount = 1)
    {
        if (!canTakeDamage || isDead) return;

        // --- THIS IS THE NEW LINE ---
        // Tell the SFXManager to play the "PlayerHit" sound.
        if (SFXManager.Instance != null)
        {
            SFXManager.Instance.PlaySFX("PlayerHit");
        }
        // --- END OF NEW CODE ---

        currentHealth -= amount;
        UpdateHeartsUI();

        if (currentHealth > 0)
        {
            if (animator != null)
            {
                animator.SetBool("IsHit", true);
            }
            StartCoroutine(HandleHitAndTeleport());
            StartCoroutine(DamageCooldown());
        }
        else
        {
            isDead = true;
            if (dragShot != null)
                dragShot.enabled = false;
            if (animator != null)
                animator.SetTrigger("IsDead");
            StartCoroutine(ShowGameOverUI(hitAnimationDuration));
        }
    }

    public void TakeDamage(int amount = 1)
    {
        TakeDamageAndTeleport(amount);
    }

    private IEnumerator DamageCooldown()
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(damageCooldown);
        canTakeDamage = true;
    }

    private IEnumerator HandleHitAndTeleport()
    {
        yield return new WaitForSeconds(hitAnimationDuration);
        if (animator != null)
            animator.SetBool("IsHit", false);
        TeleportToCheckpoint();
    }

    private IEnumerator ShowGameOverUI(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (gameOverUI != null)
            gameOverUI.SetActive(true);
    }
    #endregion

    #region Teleport
    private void TeleportToCheckpoint()
    {
        if (currentCheckpoint != null)
            transform.position = currentCheckpoint.position;
    }
    #endregion

    #region Hearts UI
    private void UpdateHeartsUI()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].sprite = i < currentHealth ? fullHeart : emptyHeart;
        }
    }
    #endregion

    #region Collision Handling
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Obstacle"))
        {
            TakeDamageAndTeleport();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OnPlatform"))
        {
            isOnPlatform = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OnPlatform"))
        {
            isOnPlatform = false;
            platformTimer = 0f;
        }
    }
    #endregion
}   