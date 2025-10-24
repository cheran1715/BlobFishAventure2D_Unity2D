using System.Collections;
using UnityEngine;

public class SplineFollowig : MonoBehaviour
{
    [SerializeField] private Transform[] routes;
    public SpriteRenderer spriteRenderer;

    private int routeToGo;
    private float tParam;
    private Vector2 FishPosition;
    private float speedModifier;
    private bool coroutineAllowed;

    void Start()
    {
        routeToGo = 0;
        tParam = 0f;
        speedModifier = 0.25f; 
        coroutineAllowed = true;
    }

    void Update()
    {
        if (coroutineAllowed && routes.Length > 0)
        {
            StartCoroutine(GoByTheRoute(routeToGo));
        }
    }

    private IEnumerator GoByTheRoute(int routeNumber)
    {
        coroutineAllowed = false;

        // Control points
        Vector2 p0 = routes[routeNumber].GetChild(0).position;
        Vector2 p1 = routes[routeNumber].GetChild(1).position;
        Vector2 p2 = routes[routeNumber].GetChild(2).position;
        Vector2 p3 = routes[routeNumber].GetChild(3).position;

        while (tParam < 1)
        {
            tParam += Time.deltaTime * speedModifier;

            Vector2 prevPos = transform.position; // store previous pos

            // Bezier curve calculation
            FishPosition =
                Mathf.Pow(1 - tParam, 3) * p0 +
                3 * Mathf.Pow(1 - tParam, 2) * tParam * p1 +
                3 * (1 - tParam) * Mathf.Pow(tParam, 2) * p2 +
                Mathf.Pow(tParam, 3) * p3;

            transform.position = FishPosition;

            // Flip sprite depending on direction
            if (FishPosition.x < prevPos.x)
                spriteRenderer.flipX = true; // moving left
            else if (FishPosition.x > prevPos.x)
                spriteRenderer.flipX = false; // moving right

            yield return new WaitForEndOfFrame();
        }

        tParam = 0f;
        routeToGo += 1;

        if (routeToGo > routes.Length - 1)
            routeToGo = 0;

        coroutineAllowed = true;
    }
}
