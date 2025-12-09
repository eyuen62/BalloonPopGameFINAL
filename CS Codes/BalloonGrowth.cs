using UnityEngine;

public class BalloonGrowth : MonoBehaviour
{
    public float growRate = 0.1f;
    public float maxSize = 3f;
    private SimpleBalloonAnim animScript;
    private GameManager gameManager;
    private bool isPopped = false;

    void Start()
    {
        // Tag this GameObject as Balloon
        gameObject.tag = "Balloon";

        // Get or add animation script
        animScript = GetComponent<SimpleBalloonAnim>();
        if (animScript == null)
        {
            animScript = gameObject.AddComponent<SimpleBalloonAnim>();
        }

        gameManager = FindFirstObjectByType<GameManager>();
        InvokeRepeating(nameof(Grow), 1f, 1f);
    }

    void Grow()
    {
        transform.localScale += Vector3.one * growRate;

        if (transform.localScale.x >= maxSize)
        {
            CancelInvoke(nameof(Grow));

            // Notify GameManager that balloon grew too big
            if (gameManager != null)
            {
                gameManager.BalloonGrewTooBig();
            }

            Destroy(gameObject);
        }
    }

    public void Pop()
    {
        if (isPopped) return;
        isPopped = true;
        CancelInvoke(nameof(Grow));

        // Play pop animation
        if (animScript != null)
        {
            animScript.PlayPopAnimation();
        }

        // Add score
        if (gameManager != null)
        {
            gameManager.AddScore(transform.localScale.x);
            gameManager.BalloonPopped(gameObject);
        }

        // Destroy balloon after animation plays (0.25 seconds)
        Destroy(gameObject, 0.25f);
    }
}