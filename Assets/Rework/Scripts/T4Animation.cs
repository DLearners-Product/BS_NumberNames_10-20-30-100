using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class T4Animation : MonoBehaviour
{
    public GameObject toMove;
    public Animator BlinkAnimation; // Animator for blink animation
    public AudioSource source;
    public AudioClip Clip;

    void Start()
    {
        // Start triggering BlinkAnimation every 4 seconds
        // InvokeRepeating(nameof(TriggerBlink), 0f, 4f);
    }

    public void MoveUpAndDown()
    {
        if (toMove == null || source == null || Clip == null) return;
        TriggerBlink();

        // Move to -98 instantly
        toMove.transform.DOLocalMoveY(-98, 0.5f).SetEase(Ease.OutBack); // Gives a slight bounce effect


        // Play the audio
        source.clip = Clip;
        source.Play();

        // Wait until the audio finishes, then move back to -139
        Invoke(nameof(ResetPosition), Clip.length);
    }

    void ResetPosition()
    {
        if (toMove == null) return;

        // Move back to -139
        toMove.transform.DOLocalMoveY(-139, 0.5f).SetEase(Ease.InBack); // Smoothly pulls back
    }

    void TriggerBlink()
    {
        if (BlinkAnimation != null)
        {
            BlinkAnimation.SetTrigger("Blink");
        }
    }
}
