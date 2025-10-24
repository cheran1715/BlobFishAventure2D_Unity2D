using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("References")]
    public LevelCompleteUI uiManager;

    [Header("Level Configuration")]
    [Tooltip("The build index of your first playable level (e.g., Level 1).")]
    public int firstLevelBuildIndex = 1;

    private int currentSceneIndex;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        uiManager = FindObjectOfType<LevelCompleteUI>();
        currentSceneIndex = scene.buildIndex;
    }

    public void LevelCompleted()
    {
        // --- START DEBUGGING ---
        Debug.Log("--- Step 1: GameManager.LevelCompleted() has been called. ---");
        //uiManager = FindObjectOfType<LevelCompleteUI>();

        if (uiManager == null)
        {
            Debug.LogError("FIND FAILED: GameManager could not find a LevelCompleteUI in this scene! Make sure the parent GameObject with the script is ACTIVE.");
            return;
        }
        else
        {
            Debug.Log("FIND SUCCESS: GameManager found the uiManager. Calling the Show() method now.");
        }
        // --- END DEBUGGING ---


        // --- UNLOCK LOGIC ---
        int completedLevelNumber = currentSceneIndex - firstLevelBuildIndex + 1;
        int nextLevelNumber = completedLevelNumber + 1;
        int maxLevelUnlocked = PlayerPrefs.GetInt("maxLevelUnlocked", 1);
        if (nextLevelNumber > maxLevelUnlocked)
        {
            PlayerPrefs.SetInt("maxLevelUnlocked", nextLevelNumber);
            PlayerPrefs.Save();
            Debug.Log("Saved new max level: " + nextLevelNumber);
        }

        Time.timeScale = 0f;
        uiManager.Show(currentSceneIndex);
    }

    public void LoadNextLevel()
    {
        Time.timeScale = 1f;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }

    public void LoadSpecificLevel(int index)
    {
        Time.timeScale = 1f;
        if (index > 0 && index < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(index);
        }
    }
}

