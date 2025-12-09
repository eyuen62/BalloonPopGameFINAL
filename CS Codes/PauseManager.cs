using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public static PauseManager instance;
    private GameObject pauseMenuUI;
    private bool isPaused = false;
    private bool initialized = false;

    void Awake()
    {
        // Singleton pattern - ensures only one PauseManager exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        // Initialize if starting in a level scene (for testing)
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name.StartsWith("Level"))
        {
            InitializePauseManager();
        }
    }

    void OnEnable()
    {
        // Subscribe to scene loaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        // Unsubscribe from scene loaded event
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// Called whenever a new scene is loaded
    /// </summary>
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reset pause state for any new scene
        isPaused = false;
        Time.timeScale = 1f;
        AudioListener.pause = false;

        // Only initialize pause manager for level scenes
        if (scene.name.StartsWith("Level"))
        {
            InitializePauseManager();
        }
    }

    /// <summary>
    /// Initializes the pause manager by finding the pause panel and setting up buttons
    /// </summary>
    void InitializePauseManager()
    {
        if (initialized) return;

        FindPausePanel();

        if (pauseMenuUI != null)
        {
            initialized = true;
        }
    }

    /// <summary>
    /// Finds the pause panel UI element in the current scene's Canvas
    /// </summary>
    void FindPausePanel()
    {
        GameObject canvas = GameObject.Find("Canvas");
        if (canvas != null)
        {
            Transform pausePanelTransform = canvas.transform.Find("PausePanel");
            if (pausePanelTransform != null)
            {
                pauseMenuUI = pausePanelTransform.gameObject;
                pauseMenuUI.SetActive(false);

                // Setup button event listeners
                SetupButtonListeners();
            }
        }
    }

    /// <summary>
    /// Sets up click listeners for all pause menu buttons
    /// </summary>
    void SetupButtonListeners()
    {
        if (pauseMenuUI == null) return;

        // Find all button components
        Button resumeBtn = pauseMenuUI.transform.Find("ResumeButton")?.GetComponent<Button>();
        Button restartBtn = pauseMenuUI.transform.Find("RestartButton")?.GetComponent<Button>();
        Button menuBtn = pauseMenuUI.transform.Find("MainMenuButton")?.GetComponent<Button>();

        // Clear existing listeners and add new ones
        if (resumeBtn != null)
        {
            resumeBtn.onClick.RemoveAllListeners();
            resumeBtn.onClick.AddListener(ResumeGame);
        }

        if (restartBtn != null)
        {
            restartBtn.onClick.RemoveAllListeners();
            restartBtn.onClick.AddListener(RestartLevel);
        }

        if (menuBtn != null)
        {
            menuBtn.onClick.RemoveAllListeners();
            menuBtn.onClick.AddListener(GoToMainMenu);
        }
    }

    void Update()
    {
        // Only process ESC key in level scenes
        Scene currentScene = SceneManager.GetActiveScene();
        if (!currentScene.name.StartsWith("Level")) return;

        // Toggle pause state when ESC is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    /// <summary>
    /// Pauses the game and shows the pause menu
    /// </summary>
    public void PauseGame()
    {
        // Find pause panel if not already found
        if (pauseMenuUI == null)
        {
            FindPausePanel();
        }

        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
            isPaused = true;
            AudioListener.pause = true;
        }
    }

    /// <summary>
    /// Resumes the game and hides the pause menu
    /// </summary>
    public void ResumeGame()
    {
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
        }

        Time.timeScale = 1f;
        isPaused = false;
        AudioListener.pause = false;
    }

    /// <summary>
    /// Restarts the current level
    /// </summary>
    public void RestartLevel()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        Scene current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.buildIndex);
    }

    /// <summary>
    /// Returns to the main menu
    /// </summary>
    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        SceneManager.LoadScene("MainMenu");
    }
}