using UnityEngine;
using UnityEngine.SceneManagement;

public class BalloonManager : MonoBehaviour
{
    public static BalloonManager instance;

    private int balloonsInLevel;
    private int balloonsPopped;
    private GameManager gameManager;

    void Awake()
    {
        // Make sure only one BalloonManager exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // FIXED: Use FindFirstObjectByType instead of FindObjectOfType
        gameManager = FindFirstObjectByType<GameManager>();
        CountBalloonsInScene();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reset counters when new level loads
        CountBalloonsInScene();
    }

    void CountBalloonsInScene()
    {
        // FIXED: Use FindObjectsByType instead of FindObjectsOfType
        BalloonGrowth[] balloons = FindObjectsByType<BalloonGrowth>(FindObjectsSortMode.None);
        balloonsInLevel = balloons.Length;
        balloonsPopped = 0;

    }

    public void BalloonPopped()
    {
        balloonsPopped++;

        // Check if all balloons are popped
        if (balloonsPopped >= balloonsInLevel)
        {
            if (gameManager != null)
            {
                gameManager.LoadNextLevel();
            }
        }
    }
}