using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class T4DotweenManager : MonoBehaviour
{
    public GameObject frame; // Assign the frame GameObject in the Inspector
    public Button FirstMove; // Assign the first button in the Inspector
    public Button SecondMove; // Assign the second button in the Inspector

    private void Start()
    {
        // Ensure frame starts at x position 0
        SetFrameXPosition(0);

        // Add listeners for button clicks
        FirstMove.onClick.AddListener(() => MoveFrame(-1940f));
        SecondMove.onClick.AddListener(() => MoveFrame(0f));
    }

    private void MoveFrame(float targetX)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(frame.transform.DOLocalMoveX(targetX, 0.8f).SetEase(Ease.OutQuad)); // Smooth transition
        sequence.Append(frame.transform.DOShakePosition(0.5f, new Vector3(10f, 0f, 0f), 10, 90) // Shake on X-axis
            .SetEase(Ease.InOutQuad));
    }

    // Adds a subtle bounce effect for a professional touch
    private void AddBounceEffect(float targetX)
    {
        frame.transform.DOLocalMoveX(targetX + 20f, 0.2f) // Slightly overshoot
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                frame.transform.DOLocalMoveX(targetX, 0.2f) // Return to the final position
                    .SetEase(Ease.InQuad);
            });
    }

    private void SetFrameXPosition(float x)
    {
        // Update frame's local x position instantly
        var currentPos = frame.transform.localPosition;
        frame.transform.localPosition = new Vector3(x, currentPos.y, currentPos.z);
    }
}
