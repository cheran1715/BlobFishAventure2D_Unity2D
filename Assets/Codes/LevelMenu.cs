using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour
{
    // Assign your level buttons here in the Inspector, IN ORDER (Level 1, Level 2, etc.)
    public Button[] levelButtons;

    void Start()
    {
        // --- THIS IS THE FIX ---
        // We now use the correct key "maxLevelUnlocked" to match the GameManager.
        // We default to level 1 being unlocked.
        int maxLevelUnlocked = PlayerPrefs.GetInt("maxLevelUnlocked", 1);
        // --- END OF FIX ---

        // Loop through all the buttons in the array
        for (int i = 0; i < levelButtons.Length; i++)
        {
            // The level number is the button's index + 1 (Button 0 = Level 1)
            int levelNumber = i + 1;

            // We create a temporary variable to capture the correct build index for the listener.
            // The build index is also the level number, assuming your levels start at index 1.
            int capturedBuildIndex = levelNumber;

            // If this level's number is higher than the max level unlocked...
            if (levelNumber > maxLevelUnlocked)
            {
                // ...then disable the button.
                levelButtons[i].interactable = false;
            }

            // We add the button's click event listener through code to be safe.
            levelButtons[i].onClick.AddListener(() => LoadLevel(capturedBuildIndex));
        }
    }

    void LoadLevel(int buildIndex)
    {
        SceneManager.LoadScene(buildIndex);
    }

    public void ResetProgress()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

