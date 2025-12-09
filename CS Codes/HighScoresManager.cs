using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class HighScoresManager : MonoBehaviour
{
    public static HighScoresManager instance;

    // Store top 5 scores
    private List<int> scores = new List<int>();
    private const int MAX_SCORES = 5;

    void Awake()
    {
        // Make sure only one exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            LoadScores();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void LoadScores()
    {
        scores.Clear();

        // Load saved scores (default to 0)
        for (int i = 0; i < MAX_SCORES; i++)
        {
            int score = PlayerPrefs.GetInt("HighScore_" + i, 0);
            scores.Add(score);
        }

        // Sort highest to lowest
        scores.Sort((a, b) => b.CompareTo(a));
    }

    void SaveScores()
    {
        // Make sure we have exactly MAX_SCORES entries
        while (scores.Count < MAX_SCORES)
        {
            scores.Add(0);
        }

        for (int i = 0; i < MAX_SCORES; i++)
        {
            PlayerPrefs.SetInt("HighScore_" + i, scores[i]);
        }
        PlayerPrefs.Save();
    }

    // Add a new score
    public void AddScore(int newScore)
    {
        // Don't add 0 scores
        if (newScore <= 0) return;

        // Add the new score
        scores.Add(newScore);

        // Sort highest to lowest
        scores.Sort((a, b) => b.CompareTo(a));

        // Keep only top 5
        while (scores.Count > MAX_SCORES)
        {
            scores.RemoveAt(MAX_SCORES); // Remove the 6th and beyond
        }

        SaveScores();
    }

    // Get all scores
    public List<int> GetScores()
    {
        // Return a copy of the list
        return new List<int>(scores);
    }
}