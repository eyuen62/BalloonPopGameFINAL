using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject pinPrefab;
    private SimplePlayerAnim animScript;

    void Start()
    {
        // Get or add animation script
        animScript = GetComponent<SimplePlayerAnim>();
        if (animScript == null)
        {
            animScript = gameObject.AddComponent<SimplePlayerAnim>();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire1"))
        {
            // Play animation
            if (animScript != null)
            {
                animScript.PlayShootAnimation();
            }

            // Get the shooting direction
            Vector2 shootDirection = GetShootDirection();

            // Create a pin at player's position
            GameObject newPin = Instantiate(pinPrefab, transform.position, Quaternion.identity);

            // Set the pin's direction
            PinMovement pinMovement = newPin.GetComponent<PinMovement>();
            if (pinMovement != null)
            {
                pinMovement.SetDirection(shootDirection);
            }
        }
    }

    // Determine shooting direction based on keys currently pressed
    private Vector2 GetShootDirection()
    {
        Vector2 direction = Vector2.zero;

        // Check each key and add to direction
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            direction += Vector2.up; // Up
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            direction += Vector2.down; // Down
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            direction += Vector2.left; // Left
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            direction += Vector2.right; // Right

        // If no direction keys are pressed, default to shooting up
        if (direction == Vector2.zero)
        {
            direction = Vector2.up;
        }

        // Normalize to prevent faster diagonal shooting
        return direction.normalized;
    }
}