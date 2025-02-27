using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class T6DoTween : MonoBehaviour
{
    public GameObject QuestionObject; // Reference to the question object

    void Start()
    {
        if (QuestionObject != null)
        {
            // Get the CanvasGroup component
            CanvasGroup canvasGroup = QuestionObject.GetComponent<CanvasGroup>();
            if (canvasGroup != null)
            {
                // Ensure CanvasGroup starts at 0 alpha
                canvasGroup.alpha = 0f;

                // Fade alpha to 1 over 1 second
                canvasGroup.DOFade(1f, 1f);
            }
        }
    }
}
