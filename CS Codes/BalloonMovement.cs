using UnityEngine;

public class BalloonMovement : MonoBehaviour
{
    // Speed at which the balloon moves
    public float speed = 3f;

    // Current direction of movement (starts moving right)
    private Vector3 direction = Vector3.right;

    // Reference to the SpriteRenderer for flipping the sprite
    private SpriteRenderer sr;

    // Screen boundaries
    private float screenLeft, screenRight;

    void Start()
    {
        // Get the SpriteRenderer component
        sr = GetComponent<SpriteRenderer>();

        // Get the main camera
        Camera cam = Camera.main;
        if (cam == null) return; // Safety check

        // Calculate the camera distance from the balloon on the z-axis
        float camDistance = Mathf.Abs(cam.transform.position.z - transform.position.z);

        // Convert viewport coordinates (0 to 1) to world coordinates for screen edges
        Vector3 rightEdge = cam.ViewportToWorldPoint(new Vector3(1f, 0.5f, camDistance));
        Vector3 leftEdge = cam.ViewportToWorldPoint(new Vector3(0f, 0.5f, camDistance));

        // Store x positions of left and right screen edges
        screenRight = rightEdge.x;
        screenLeft = leftEdge.x;
    }

    void Update()
    {
        // Move the balloon in the current direction
        transform.Translate(direction * speed * Time.deltaTime);

        // Get the balloon's current half-width (radius) based on its scale
        // Assuming the balloon sprite is 1 unit wide at scale 1
        float balloonHalfWidth = transform.localScale.x * 0.5f;

        // Check if the balloon's RIGHT EDGE has reached the right screen edge
        if (transform.position.x + balloonHalfWidth > screenRight)
        {
            direction = Vector3.left;       // Reverse direction to left
            if (sr != null) sr.flipX = true; // Flip the sprite horizontally

            // Also nudge it back so edge is exactly at screen edge
            transform.position = new Vector3(
                screenRight - balloonHalfWidth,
                transform.position.y,
                transform.position.z
            );
        }
        // Check if the balloon's LEFT EDGE has reached the left screen edge
        else if (transform.position.x - balloonHalfWidth < screenLeft)
        {
            direction = Vector3.right;      // Reverse direction to right
            if (sr != null) sr.flipX = false; // Reset sprite flip

            // Nudge it back so edge is exactly at screen edge
            transform.position = new Vector3(
                screenLeft + balloonHalfWidth,
                transform.position.y,
                transform.position.z
            );
        }
    }
}