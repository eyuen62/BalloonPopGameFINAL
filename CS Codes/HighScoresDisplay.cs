using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HighScoresDisplay : MonoBehaviour
{
    public TextMeshProUGUI[] scoreTexts;

    void Start()
    {
        ShowScores();
    }

    void ShowScores()
    {
        // Find or create HighScoresManager if needed
        EnsureHighScoresManagerExists();

        // Get scores
        List<int> scores = HighScoresManager.instance.GetScores();

        // Display scores
        for (int i = 0; i < scoreTexts.Length; i++)
        {
            if (i < scores.Count)
            {
                scoreTexts[i].text = (i + 1) + ". " + scores[i];
            }
            else
            {
                scoreTexts[i].text = (i + 1) + ". 0";
            }
        }
    }

    void EnsureHighScoresManagerExists()
    {
        // If instance is null, try to find it
        if (HighScoresManager.instance == null)
        {
            // Search for existing manager
            HighScoresManager existingManager = FindFirstObjectByType<HighScoresManager>();
            if (existingManager != null)
            {
                HighScoresManager.instance = existingManager;
            }
            else
            {
                // Create new one if none exists
                GameObject obj = new GameObject("HighScoresManager");
                HighScoresManager manager = obj.AddComponent<HighScoresManager>();
                HighScoresManager.instance = manager;
            }
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}