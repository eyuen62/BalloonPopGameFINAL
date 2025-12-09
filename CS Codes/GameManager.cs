using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public TextMeshProUGUI scoreText;

    private int currentScore = 0;
    private int scoreAtLevelStart = 0;
    private List<GameObject> balloons = new List<GameObject>();
    private bool levelFailed = false;
    private bool scoreSaved = false; // NEW: Prevent duplicate saves

    void Awake()
    {
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
        SetupLevel();
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
        if (scene.name.Contains("Level"))
        {
            SetupLevel();
        }
    }

    void SetupLevel()
    {
        scoreAtLevelStart = currentScore;
        FindScoreText();
        FindAllBalloons();
        UpdateScoreDisplay();
    }

    void FindScoreText()
    {
        GameObject canvas = GameObject.Find("Canvas");
        if (canvas != null)
        {
            scoreText = canvas.transform.Find("ScoreText")?.GetComponent<TextMeshProUGUI>();
        }
    }

    void FindAllBalloons()
    {
        balloons.Clear();
        levelFailed = false;

        GameObject[] balloonObjects = GameObject.FindGameObjectsWithTag("Balloon");
        foreach (GameObject balloon in balloonObjects)
        {
            balloons.Add(balloon);
        }
    }

    public void AddScore(float balloonSize)
    {
        int points = Mathf.RoundToInt(100 / balloonSize);
        currentScore += points;
        UpdateScoreDisplay();
    }

    public void BalloonPopped(GameObject balloon)
    {
        balloons.Remove(balloon);

        if (balloons.Count == 0 && !levelFailed)
        {
            Invoke("LoadNextLevel", 0.5f);
        }
    }

    public void BalloonGrewTooBig()
    {
        if (!levelFailed)
        {
            levelFailed = true;
            currentScore = scoreAtLevelStart;
            UpdateScoreDisplay();
            Invoke("RestartLevel", 0.5f);
        }
    }

    public void RestartLevel()
    {
        Scene current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.buildIndex);
    }

    public void LoadNextLevel()
    {
        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextIndex);
        }
        else
        {
            // Game Complete - Save score (only once!)
            SaveToHighScores();
            SceneManager.LoadScene("HighScore");
        }
    }

    public void SaveToHighScores()
    {
        // Check if we already saved this score
        if (scoreSaved)
        {
            return; // Don't save again
        }

        // Make sure score is > 0
        if (currentScore <= 0) return;

        if (HighScoresManager.instance != null)
        {
            HighScoresManager.instance.AddScore(currentScore);
            scoreSaved = true; // Mark as saved
        }
        else
        {
            // Try to find or create the manager
            HighScoresManager manager = FindFirstObjectByType<HighScoresManager>();
            if (manager != null)
            {
                manager.AddScore(currentScore);
                scoreSaved = true;
            }
        }
    }

    void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + currentScore;
        }
    }

    public void ResetForNewGame()
    {
        currentScore = 0;
        scoreAtLevelStart = 0;
        scoreSaved = false; // NEW: Reset for new game
        UpdateScoreDisplay();
    }
}