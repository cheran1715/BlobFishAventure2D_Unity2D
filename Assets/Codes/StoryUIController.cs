using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Required for using TextMeshPro

public class StoryUIController : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("The TextMeshPro component where the story will be displayed.")]
    public TextMeshProUGGUI storyTextComponent;

    [Header("Story Content")]
    [Tooltip("Paste the full story you want to display here.")]
    [TextArea(10, 20)] // Makes the text box in the Inspector bigger
    public string fullStoryText;

    // The typingSpeed variable and the coroutine are no longer needed.

    // This is called when the GameObject becomes active
    private void OnEnable()
    {
        // When the story panel is opened, show the full text immediately.
        ShowFullTextInstantly();
    }

    public void ShowFullTextInstantly()
    {
        if (storyTextComponent != null)
        {
            // Set the text component's content to be the full story text.
            storyTextComponent.text = fullStoryText;
        }
    }
}

    
