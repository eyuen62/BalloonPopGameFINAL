using UnityEngine;

public class GoombaMovement : MonoBehaviour
{
    public float speed = 4f;
    public float amplitude = 2f; // How high/low the Goomba moves
    public float frequency = 1f; // How fast it moves up/down

    private Vector2 startPosition;
    private float randomOffset;

    void Start()
    {
        startPosition = transform.position;
        randomOffset = Random.Range(0f, 2f * Mathf.PI); // Random start in wave
    }

    void Update()
    {
        // Move left across screen
        float newX = transform.position.x - speed * Time.deltaTime;

        // Create waving up/down motion
        float newY = startPosition.y + Mathf.Sin(Time.time * frequency + randomOffset) * amplitude;

        transform.position = new Vector2(newX, newY);

        // If Goomba goes off screen, destroy it
        if (transform.position.x < -10f)
        {
            Destroy(gameObject);
        }
    }

    // NEW: Collision with pins
    void OnTriggerEnter2D(Collider2D other)
    {
        // If hit by a pin, destroy the pin
        if (other.CompareTag("Pin"))
        {
            Destroy(other.gameObject); // Destroy the pin
            // Note: Goomba continues moving (acts as obstacle)
        }
    }
}