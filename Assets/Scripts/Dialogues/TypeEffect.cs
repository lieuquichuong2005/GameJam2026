using TMPro;
using UnityEngine;
using System.Collections;
public class TypeEffect : MonoBehaviour
{
    // Assign your TextMeshProUGUI component here in the Inspector
    public TMP_Text textComponent;
    public string fullText;
    public float delay = 0.1f; // Time between letters

    void OnEnable()
    {
        // Get the component if not assigned in the Inspector
        if (textComponent == null)
        {
            textComponent = GetComponent<TMP_Text>();
        }

        // Ensure you have text to display, or set it dynamically
        if (string.IsNullOrEmpty(fullText))
        {
            fullText = textComponent.text;
        }

        // Set initial visible characters to 0
        textComponent.maxVisibleCharacters = 0;

        // Start the coroutine
        StartCoroutine(ShowTextRoutine());
    }

    IEnumerator ShowTextRoutine()
    {
        // Set the text component's full text first
        textComponent.text = fullText;

        // Iterate through each character and make it visible
        for (int i = 0; i <= fullText.Length; i++)
        {
            textComponent.maxVisibleCharacters = i;
            yield return new WaitForSeconds(delay);
        }
        yield return new WaitForSeconds(2f);
        textComponent.text = null;
    }
}
