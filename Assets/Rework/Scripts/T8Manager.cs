using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class T8Manager : MonoBehaviour
{
    public GameObject[] questions; // Array of question GameObjects
    public int[] selectionCounts; // Number of options to select per question
    private int currentQuestionIndex = 0;
    private List<GameObject> selectedOptions = new List<GameObject>();
    public GameObject[] colors; // Color buttons (GameObjects with Image components)
    private Color selectedColor; // The currently selected color
    private bool isColorPicked = false; // Flag to check if a color is picked
    private Button[] currentOptions; // Stores buttons of the current question
    private Dictionary<Image, Color> initialColors = new Dictionary<Image, Color>();

    public AudioSource source;

    public AudioClip correctAnswer;

    public AudioClip wrongAnswer;

    public Text counter;

    public GameObject ActivityCompleted;

    void Start()
    {
        // Store initial colors
        foreach (GameObject question in questions)
        {
            foreach (Image img in question.GetComponentsInChildren<Image>())
            {
                if (!initialColors.ContainsKey(img))
                {
                    initialColors.Add(img, img.color);
                }
            }
        }

        ActivateQuestion(currentQuestionIndex);
    }

    public void SelectOption(GameObject option)
    {
        if (!isColorPicked) return; // Prevent selection if no color is picked

        if (selectedOptions.Contains(option)) return;

        selectedOptions.Add(option);
        source.clip = correctAnswer;
        source.Play();
        option.gameObject.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
        option.GetComponent<Button>().interactable = false; // Disable after selection


    }

    public void checkAnswer()
    {
        if (selectedOptions.Count == selectionCounts[currentQuestionIndex])
        {
            // Correct selection count, move to the next question
            NextQuestion();
            int currentCounterValue = int.Parse(counter.text);
            if (currentCounterValue < 8)
            {
                currentCounterValue++;
                counter.text = currentCounterValue.ToString();
            }
        }
        else
        {
            // Incorrect selection count, reset the current question
            Camera.main.transform.DOShakePosition(0.5f, strength: new Vector3(10, 0, 0), vibrato: 10, randomness: 90);
            ResetCurrentQuestion();
            source.clip = wrongAnswer;
            source.Play();
        }
    }

    void ResetCurrentQuestion()
    {
        foreach (GameObject option in selectedOptions)
        {
            option.GetComponent<Button>().interactable = true; // Enable button again
            option.transform.GetChild(0).GetComponent<ParticleSystem>().Stop(); // Stop particle effect

            // Reset applied color
            Image img = option.GetComponent<Image>();
            if (img != null && initialColors.ContainsKey(img))
            {
                img.color = initialColors[img]; // Restore the initial color
            }
        }
        selectedOptions.Clear(); // Clear selected options
    }


    public void PickColor(Image colorButton)
    {
        selectedColor = colorButton.color;
        isColorPicked = true;

        // Scale-up effect using DOTween
        colorButton.transform.DOScale(1.2f, 0.2f).SetLoops(2, LoopType.Yoyo);
        EnableOptions(); // Enable options after picking a color
    }

    // Function to apply the selected color to a clicked target image
    public void ApplyColor(Image targetImage)
    {
        if (targetImage != null)
        {
            // First, fade out the image
            targetImage.DOFade(0f, 0.2f).OnComplete(() =>
            {
                // Apply new color after fade-out
                targetImage.color = selectedColor;

                // Then fade it back in with a smooth effect
                targetImage.DOFade(1f, 0.3f);
            });

            // Small bounce effect on color change
            targetImage.transform.DOPunchScale(Vector3.one * 0.2f, 0.3f, 10, 1f);
        }
    }


    void NextQuestion()
    {
        StartCoroutine(DelayForNextQuestion());
    }

    IEnumerator DelayForNextQuestion()
    {
        yield return new WaitForSeconds(0.5f);
        questions[currentQuestionIndex].SetActive(false);
        selectedOptions.Clear();
        isColorPicked = false; // Reset color pick requirement for the next question

        currentQuestionIndex++;
        if (currentQuestionIndex < questions.Length)
        {
            ActivateQuestion(currentQuestionIndex);
        }
        else
        {
            Debug.Log("Game Over");
            ActivityCompleted.SetActive(true);
        }

    }

    void ActivateQuestion(int index)
    {
        questions[index].SetActive(true);
        DisableOptions(); // Disable options at the start
    }

    void DisableOptions()
    {
        currentOptions = questions[currentQuestionIndex].GetComponentsInChildren<Button>();
        foreach (Button btn in currentOptions)
        {
            btn.interactable = false; // Disable buttons
        }
    }

    void EnableOptions()
    {
        foreach (Button btn in currentOptions)
        {
            btn.interactable = true; // Enable buttons after color selection
        }
    }
}
