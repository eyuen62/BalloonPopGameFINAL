using UnityEngine;

public class PinMovement : MonoBehaviour
{
    public float speed = 8f;
    private Vector3 direction = Vector3.up;

    void Update()
    {
        // Move the pin in the set direction
        transform.Translate(direction * speed * Time.deltaTime);

        // Check if pin goes off screen
        if (Mathf.Abs(transform.position.x) > 15f || Mathf.Abs(transform.position.y) > 10f)
        {
            Destroy(gameObject);
        }
    }

    // Set the direction of the pin
    public void SetDirection(Vector2 dir)
    {
        // Convert 2D direction to 3D
        direction = new Vector3(dir.x, dir.y, 0).normalized;
    }
}