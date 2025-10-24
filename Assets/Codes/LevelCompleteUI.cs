using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelCompleteUI : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject completePanel;
    public TextMeshProUGUI levelText;
    public Button previousLevelButton;
    public Button nextLevelButton;
    public Button restartButton;
    public Button homeButton;

    [Header("Game Data")]
    [Tooltip("The build index of the VERY LAST level of your game.")]
    public int lastLevelBuildIndex = 4; // Set this to the build index of your final level
    private int currentLevelIndex;

    public void Show(int levelIndex)
    {
        if (completePanel == null)
        {
            Debug.LogError("LINK FAILED: The 'Complete Panel' field is empty in the Inspector!");
            return;
        }

        currentLevelIndex = levelIndex;

        // --- NEW LOGIC: Check if this is the last level ---
        if (levelIndex == lastLevelBuildIndex)
        {
            // This is the final level of the game
            if (levelText != null)
            {
                levelText.text = "You Won!";
            }
            if (nextLevelButton != null)
            {
                // Hide the "Next Level" button
                nextLevelButton.gameObject.SetActive(false);
            }
            // We still show the previous level button on the win screen
            if (previousLevelButton != null && GameManager.Instance != null)
            {
                previousLevelButton.gameObject.SetActive(levelIndex > GameManager.Instance.firstLevelBuildIndex);
            }
        }
        else
        {
            // This is a normal level, not the last one
            if (levelText != null && GameManager.Instance != null)
            {
                int levelNumber = levelIndex - GameManager.Instance.firstLevelBuildIndex + 1;
                levelText.text = "Level " + levelNumber.ToString();
            }
            if (nextLevelButton != null)
            {
                // Make sure the "Next Level" button is visible
                nextLevelButton.gameObject.SetActive(true);
            }
            if (previousLevelButton != null && GameManager.Instance != null)
            {
                previousLevelButton.gameObject.SetActive(levelIndex > GameManager.Instance.firstLevelBuildIndex);
            }
        }
        // --- END OF NEW LOGIC ---

        // Finally, show the entire panel
        completePanel.SetActive(true);
    }

    // --- Button Click Handlers (No changes needed here) ---

    public void OnNextLevelClicked()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.LoadNextLevel();
        }
    }

    public void OnRestartLevelClicked()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnHomeClicked()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void OnPreviousLevelClicked()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.LoadSpecificLevel(currentLevelIndex - 1);
        }
    }
}

