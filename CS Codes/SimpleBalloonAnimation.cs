using UnityEngine;

public class SimpleBalloonAnim : MonoBehaviour
{
    private Vector3 originalScale;
    private bool isPopping = false;
    private float popTimer = 0f;

    void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        if (isPopping)
        {
            popTimer += Time.deltaTime;

            // First 0.1 seconds: expand to 150%
            if (popTimer < 0.1f)
            {
                transform.localScale = originalScale * 1.5f;
            }
            // Next 0.1 seconds: shrink to nothing
            else if (popTimer < 0.2f)
            {
                transform.localScale = Vector3.zero;
            }
            // Animation complete
            else
            {
                // Nothing - balloon will be destroyed
            }
        }
    }

    public void PlayPopAnimation()
    {
        isPopping = true;
        popTimer = 0f;
    }
}