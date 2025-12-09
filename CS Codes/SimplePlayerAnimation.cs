using UnityEngine;

public class SimplePlayerAnim : MonoBehaviour
{
    private Vector3 originalScale;
    private bool isAnimating = false;
    private float animationTimer = 0f;

    void Start()
    {
        // Remember player's normal size
        originalScale = transform.localScale;
    }

    void Update()
    {
        if (isAnimating)
        {
            animationTimer += Time.deltaTime;

            // First 0.1 seconds: shrink to 80%
            if (animationTimer < 0.1f)
            {
                transform.localScale = originalScale * 0.8f;
            }
            // Next 0.1 seconds: return to normal
            else if (animationTimer < 0.2f)
            {
                transform.localScale = originalScale;
            }
            // Stop animation after 0.2 seconds
            else
            {
                isAnimating = false;
            }
        }
    }

    // Call this from PlayerShoot.cs
    public void PlayShootAnimation()
    {
        isAnimating = true;
        animationTimer = 0f;
    }
}