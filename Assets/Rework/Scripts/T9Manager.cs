using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;


public class T9Manager : MonoBehaviour
{
    public Button[] Questions;
    public Text counter;
    public GameObject Frame;

    private int currentCount = 1;
    private bool hasMovedFrame = false;
    private bool[] countedButtons; // To track which buttons were counted

    public GameObject ActivityCompleted;

    void Start()
    {
        countedButtons = new bool[Questions.Length]; // Initialize tracking array
        counter.text = currentCount.ToString();
        InvokeRepeating(nameof(CheckButtons), 0.1f, 0.1f); // Check every 0.1s
    }

    void CheckButtons()
    {
        bool allButtonsInactive = true; // Assume all are inactive initially

        for (int i = 0; i < Questions.Length; i++)
        {
            if (!Questions[i].interactable && !countedButtons[i]) // If button just became false
            {
                countedButtons[i] = true; // Mark as counted
                IncreaseCounter();
            }

            if (Questions[i].interactable) // If at least one button is still interactable
            {
                allButtonsInactive = false;
            }
        }

        if (allButtonsInactive)
        {
            Debug.Log("Game Over");
            ActivityCompleted.SetActive(true);
        }
    }

    void IncreaseCounter()
    {
        if (currentCount < 6)
        {
            currentCount++;
            counter.text = currentCount.ToString();

            if (currentCount > 3 && !hasMovedFrame)
            {
                hasMovedFrame = true;
                StartCoroutine(MoveFrameAfterDelay(0.5f));
            }
        }
    }

    IEnumerator MoveFrameAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Frame.transform.DOLocalMoveX(-1700f, 0.5f).SetEase(Ease.InOutQuad);
    }

}
