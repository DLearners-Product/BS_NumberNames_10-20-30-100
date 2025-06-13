using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class T7manager : MonoBehaviour
{
    public GameObject[] Questions;
    public GameObject[] Answers;

    private GameObject selectedQuestion;
    private GameObject selectedAnswer;
    private Color defaultColor = Color.white;

    public Text counter;
    public GameObject activityCompleted;

    public ParticleSystem[] particles;
    public AudioSource source;
    public AudioClip correctAnswer;
    public AudioClip wrongAnswer;

    public T7DoTweenManager doTweenManager;

    void Start()
    {
        // Initialize the button click listeners
        foreach (var question in Questions)
        {
            question.GetComponent<Button>().onClick.AddListener(() => OnQuestionClicked(question));
        }

        foreach (var answer in Answers)
        {
            answer.GetComponent<Button>().onClick.AddListener(() => OnAnswerClicked(answer));
        }
    }

    void OnQuestionClicked(GameObject question)
    {
        // Reset selection if starting a new attempt
        ResetSelections();

        // Set the new selected question
        selectedQuestion = question;

        // Highlight the new selected question
        question.GetComponent<Image>().DOColor(Color.yellow, 0.3f);
    }

    void OnAnswerClicked(GameObject answer)
    {
        if (selectedQuestion == null) return; // No question selected yet

        // Set the selected answer
        selectedAnswer = answer;

        // Highlight the selected answer
        answer.GetComponent<Image>().DOColor(Color.cyan, 0.3f);

        // Validate selection
        ValidateSelection();
    }

    void ValidateSelection()
    {
        if (selectedQuestion != null && selectedAnswer != null)
        {
            if (selectedQuestion.name == selectedAnswer.name)
            {
                // Correct Match
                foreach (var particle in particles)
                {
                    // Example: Activate each particle
                    particle.Play();
                }
                source.clip = correctAnswer;
                source.Play();
                DoCorrectMatchEffect(selectedQuestion, selectedAnswer);

                int currentCounterValue = int.Parse(counter.text);
                if (currentCounterValue < 8)
                {
                    currentCounterValue++;
                    counter.text = currentCounterValue.ToString();
                }
                if (currentCounterValue > 3)
                {
                    doTweenManager.ChangeLocalYPosition();
                }

                if (currentCounterValue == 8)
                {
                    Debug.Log("Game Over");
                    StartCoroutine(smallDelay());
                    

                }

                // Disable interaction
                selectedQuestion.GetComponent<Button>().interactable = false;
                selectedAnswer.GetComponent<Button>().interactable = false;
            }
            else
            {
                // Wrong Match
                source.clip = wrongAnswer;
                source.Play();
                DoWrongMatchEffect(selectedQuestion, selectedAnswer);
            }

            // Reset selections
            ResetSelections();
        }

        IEnumerator smallDelay()
        {
            yield return new WaitForSeconds(1f);
            activityCompleted.SetActive(true);
        }
    }



    void DoCorrectMatchEffect(GameObject question, GameObject answer)
    {
        // question.transform.DOScale(1.2f, 0.3f).OnComplete(() => question.transform.DOScale(1f, 0.2f));
        // answer.transform.DOScale(1.2f, 0.3f).OnComplete(() => answer.transform.DOScale(1f, 0.2f));

        question.GetComponent<Image>().DOColor(Color.green, 0.3f);
        answer.GetComponent<Image>().DOColor(Color.green, 0.3f);
    }

    void DoWrongMatchEffect(GameObject question, GameObject answer)
    {
        question.transform.DOShakePosition(0.5f, 10, 10);
        answer.transform.DOShakePosition(0.5f, 10, 10);

        question.GetComponent<Image>().DOColor(Color.red, 0.3f).OnComplete(() => question.GetComponent<Image>().DOColor(defaultColor, 0.3f));
        answer.GetComponent<Image>().DOColor(Color.red, 0.3f).OnComplete(() => answer.GetComponent<Image>().DOColor(defaultColor, 0.3f));
    }

    void ResetSelections()
    {
        if (selectedQuestion != null)
        {
            selectedQuestion.GetComponent<Image>().DOColor(defaultColor, 0.3f);
            selectedQuestion = null;
        }

        if (selectedAnswer != null)
        {
            selectedAnswer.GetComponent<Image>().DOColor(defaultColor, 0.3f);
            selectedAnswer = null;
        }
    }
}
