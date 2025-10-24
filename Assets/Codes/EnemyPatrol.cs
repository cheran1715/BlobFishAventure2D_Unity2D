using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Patrol Settings")]
    public Transform pointA;      // Patrol start
    public Transform pointB;      // Patrol end
    public float speed = 2f;

    private Transform target;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Always start by moving toward the farthest point
        if (Vector2.Distance(transform.position, pointA.position) < Vector2.Distance(transform.position, pointB.position))
            target = pointB;
        else
            target = pointA;
    }

    void FixedUpdate()
    {
        if (target == null) return;

        // 1. Calculate direction and set velocity
        Vector2 direction = (target.position - transform.position).normalized;
        rb.velocity = new Vector2(direction.x * speed, rb.velocity.y);

        // 2. Flip sprite based on horizontal velocity (this is the key change)
        if (spriteRenderer != null)
        {
            if (rb.velocity.x > 0.01f) // Moving right
            {
                spriteRenderer.flipX = false;
            }
            else if (rb.velocity.x < -0.01f) // Moving left
            {
                spriteRenderer.flipX = true;
            }
        }

        // 3. Check if we have reached the target and switch points
        if (Vector2.Distance(transform.position, target.position) <= 0.2f)
        {
            target = (target == pointA) ? pointB : pointA;
        }
    }

    void OnDrawGizmos()
    {
        if (pointA != null && pointB != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(pointA.position, pointB.position);
        }
    }
}