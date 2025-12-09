using UnityEngine;

public class GoombaSpawner : MonoBehaviour
{
    public GameObject goombaPrefab;
    public float spawnRate = 3f; // Time between spawns
    public float spawnHeight = 3f; // Y position range

    void Start()
    {
        // Use InvokeRepeating instead of coroutine (simpler, like professor's style)
        InvokeRepeating("SpawnGoomba", 1f, spawnRate);
    }

    void SpawnGoomba()
    {
        // Spawn Goomba at right side of screen
        Vector2 spawnPos = new Vector2(10f, Random.Range(-spawnHeight, spawnHeight));

        // Create the Goomba
        Instantiate(goombaPrefab, spawnPos, Quaternion.identity);
    }
}