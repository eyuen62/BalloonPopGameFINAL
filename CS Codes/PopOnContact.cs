using UnityEngine;

public class PopOnContact : MonoBehaviour
{
    public AudioClip popSound;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Balloon"))
        {
            // Play pop sound if available
            if (popSound != null)
            {
                AudioSource.PlayClipAtPoint(popSound, transform.position);
            }

            // Let the balloon handle its own pop logic
            BalloonGrowth balloon = other.GetComponent<BalloonGrowth>();
            if (balloon != null)
                balloon.Pop();

            // Destroy the pin
            Destroy(gameObject);
        }
    }
}